using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel.Collections;
using Microsoft.BusinessData.Runtime;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Infrastructure;

namespace TM.Utils
{
    public class BusinessDataColumnUpdater
    {
        #region private Data members

        protected SPList _list = null;
        protected string _columnName = "";
        protected ILobSystemInstance _lobSysInst = null;
        protected IEntity _entity = null;
        protected IView _specificFinderView = null;
        protected SPListItemCollection _items = null;
        protected StringBuilder _traceMessages = new StringBuilder();
        protected bool _enableTracing = false;
        protected string _batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ows:Batch OnError=\"Return\">{0}</ows:Batch>";
        protected string _batch = string.Empty;
        protected StringBuilder _methodBuilder = new StringBuilder();

        #endregion

        #region Constructors

        public BusinessDataColumnUpdater() { }

        public BusinessDataColumnUpdater(SPList list, string businessDataColumnName)
        {
            _list = list;
            _columnName = businessDataColumnName;
        }

        public BusinessDataColumnUpdater(SPList list, string businessDataColumnName, bool enableTracing)
        {
            _list = list;
            _columnName = businessDataColumnName;
            _enableTracing = enableTracing;
        }

        #endregion

        #region Properties

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public SPList List
        {
            get { return _list; }
            set { _list = value; }
        }

        public bool EnableTracing
        {
            get { return _enableTracing; }
            set { _enableTracing = value; }
        }

        public StringBuilder TraceMessages
        {
            get { return _traceMessages; }
            private set { _traceMessages = value; }
        }

        #endregion

        #region Public Methods

        /* Note: This is the public method that you should call in a production
         * environment. 
         * */
        public virtual void UpdateColumnUsingBatch(Action<int> updateProgress, int itemId = 0)
        {
            if (_enableTracing && _traceMessages.Length > 0)
                _traceMessages = new StringBuilder();

            //check to make sure the field is a valid column name
            if (!(_list.Fields.ContainsField(_columnName)))
            {
                throw new ArgumentException(String.Format("The field '{0}' is not a valid field name in this list - check for typos!", _columnName));
            }

            SPField fieldByInternalName = _list.Fields.GetFieldByInternalName(_columnName);

            //check to make sure that the field is a business data column
            if (!(fieldByInternalName is SPBusinessDataField))
            {
                throw new Exception(String.Format("The field '{0}' is not a business data field", _columnName));
            }

            //get the bdc data column in the list
            SPBusinessDataField bizDataField = (SPBusinessDataField)fieldByInternalName;
            string relatedFieldName = bizDataField.RelatedField;

            //build a list of related fields in the list that derive their data from
            //the bdc data column
            string[] secondaryFieldsNames = bizDataField.GetSecondaryFieldsNames();
            string property = bizDataField.GetProperty("SecondaryFieldWssNames");

            //populate the array of secondary names
            string[] secondaryWssFieldNames = BCS.SecondaryFieldNamesHelper.Decode(property);

            int totalListItems = _list.ItemCount;

            _entity = BCS.GetEntity(SPServiceContext.Current, String.Empty, bizDataField.EntityNamespace, bizDataField.EntityName);
            _lobSysInst = _entity.GetLobSystem().GetLobSystemInstances()[bizDataField.SystemInstanceName];
            _specificFinderView = _entity.GetDefaultSpecificFinderView();

            if (_enableTracing)
            {
                _traceMessages.AppendLine("Connection Info for Clean Up");
                _traceMessages.AppendLine("-------------------------------");
                _traceMessages.AppendLine("LOB SystemInstanceName: " + _lobSysInst.Name);
                _traceMessages.AppendLine("LOB System Entity Name: " + _entity.Name);
                _traceMessages.AppendLine("List: " + _list.Title);
                _traceMessages.AppendLine("List Items Found: " + totalListItems);
                _traceMessages.AppendLine("Mode: " + (itemId == 0 ? "Multiple items" : "Single item"));
                _traceMessages.AppendLine("-------------------------------");
                _traceMessages.AppendLine("");
                _traceMessages.AppendLine("Attempting to update list items....");
                _traceMessages.AppendLine("");
            }

            //get the list item collection from the list limited by the total number
            //of items in the list and return only the main field and its related
            //BDC field that we want to update.  this should be an efficient way to
            //work with a large list in a production environment.
            var query = new SPQuery();
            if (itemId == 0)
            {
                query.RowLimit = totalListItems < 2000 ? (uint) totalListItems : (uint) 2000;
            }
            else
            {
                query.Query = "<Where><Eq><FieldRef Name='ID' /><Value Type='Integer'>" + itemId + "</Value></Eq></Where>";
            }
            
            query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", _columnName, relatedFieldName);
            query.ViewAttributes = "Scope=\"Recursive\""; //you must set this value to recursive to update things in subfolders

            //retrieve SPListItemCollection using paging techniques for optimization
            do
            {
                _items = _list.GetItems(query);
                _methodBuilder = new StringBuilder();
                var lastItemId = 0;
                //loop through list items in list and try to update/refresh with latest BDC value
                foreach (SPListItem item in _items)
                {
                    //make sure the item has data in the column before trying to update
                    if (!(item[_columnName] == null) && !(item[relatedFieldName] == null))
                    {
                        BuildListItemBatchUpdateCAML(item, bizDataField, _lobSysInst, _entity,
                           _specificFinderView, secondaryFieldsNames, secondaryWssFieldNames);
                    }

                    lastItemId = item.ID;
                } //end foreach

                // Put the pieces together.
                _batch = string.Format(_batchFormat, _methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = _list.ParentWeb.ProcessBatchData(_batch);

                if (_enableTracing)
                {
                    _traceMessages.AppendLine("-------------------------------");
                    _traceMessages.AppendLine("OUTCOME for Batch");
                    _traceMessages.AppendLine("-------------------------------");
                    _traceMessages.AppendLine(batchReturn);
                }

                // set the position cursor for the next iteration
                query.ListItemCollectionPosition = _items.ListItemCollectionPosition;

                // updating progress
                if (updateProgress != null)
                {
                    var complete = lastItemId * 100 / _list.ItemCount;
                    updateProgress(complete);
                }
            } while (query.ListItemCollectionPosition != null);

        } //end method

        #endregion

        #region Private methods

        /* Note: this is a more efficient method used by the UpdateColumnUsingBatch()
         * method that builds a dynamic batch CAML fragment for a batch update.
         * */
        protected virtual void BuildListItemBatchUpdateCAML(SPListItem item, SPBusinessDataField bizDataField,
           ILobSystemInstance _lobSysInst, IEntity _entity, IView view, string[] secondaryBdcFieldNames, string[] secondaryWssFieldNames)
        {
            string bdcFieldName = bizDataField.BdcFieldName;
            string encodedId = null;
            object[] objArray = null;
            Identity identifierValues = null;
            object[] objArray2 = null;
            IEnumerator<IField> enumerator;

            encodedId = (string)item[bizDataField.RelatedField];
            StringBuilder methodFormat = new StringBuilder();

            try
            {
                if (encodedId != null)
                {
                    objArray = EntityInstanceIdEncoder.DecodeEntityInstanceId(encodedId);
                    identifierValues = _entity.FindSpecific(new Identity(objArray), _lobSysInst).GetIdentity();
                    objArray2 = new object[identifierValues.IdentifierCount];
                }

                for (int i = 0; i < identifierValues.IdentifierCount; i++)
                {
                    objArray2[i] = identifierValues[i];
                }

                item[bizDataField.RelatedField] = EntityInstanceIdEncoder.EncodeEntityInstanceId(objArray2);
                enumerator = view.Fields.GetEnumerator();

                IEntityInstance instance = _entity.FindSpecific(new Identity(objArray2), _lobSysInst);

                IField field;
                string name;

                //loop through all of the fields for the current list item.  when the bdc
                //business data column is found, obtain the most recent data from the data source
                //and update the list item with that value.  similarly, loop through any related
                //fields and refresh them as well
                while (enumerator.MoveNext())
                {
                    field = enumerator.Current;
                    name = field.Name;
                    if (name == bdcFieldName)
                    {
                        //this sets the current value for the field from the database via the bdc
                        item[bizDataField.InternalName] = Convert.ToString(instance.GetFormatted(field));

                        methodFormat.AppendFormat(
                           "<Method ID=\"{0}\">" +
                           "<SetList>{1}</SetList>" +
                           "<SetVar Name=\"Cmd\">Save</SetVar>" +
                           "<SetVar Name=\"ID\">{2}</SetVar>" +
                           "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{3}\">{4}</SetVar>",
                           item.ID, _list.ID.ToString(), item.ID, bizDataField.InternalName,
                           Convert.ToString(instance.GetFormatted(field)));
                    }

                    if (secondaryBdcFieldNames.Length > 0)
                    {
                        //loop through the secondary fields to refresh those as well
                        for (int i = 0; i < secondaryBdcFieldNames.Length; i++)
                        {
                            //if a secondary field is found, update it
                            if (secondaryBdcFieldNames[i] == field.Name)
                            {
                                item[secondaryWssFieldNames[i]] = Convert.ToString(instance.GetFormatted(field));

                                methodFormat.AppendFormat(
                                   "<SetVar Name=\"urn:schemas-microsoft-com:office:office#{0}\">{1}</SetVar>",
                                   secondaryWssFieldNames[i],
                                   Convert.ToString(instance.GetFormatted(field))
                                   );
                            } //end if
                        } //end for
                    }
                } //end while

                methodFormat.Append("</Method>");
                _methodBuilder.Append(methodFormat.ToString());

            } //end try
            catch (Microsoft.BusinessData.Runtime.ObjectNotFoundException)
            {

            }
            catch (Exception)
            {
            }

        } //end method

        #endregion
    }
    public struct BcsMethodExecutionInfo
    {
        public string lob;
        public string ns;
        public string contentType;
        public string methodName;
        public MethodInstanceType methodType;
    }
    public static class BCS
    {
        #region [classes]
        public static class SecondaryFieldNamesHelper
        {
            #region fields & properties
            private static string[] s_crgstrUrlHexValue = new string[]
        {
            "%00", "%01", "%02", "%03", "%04", "%05", "%06", "%07", "%08", "%09", "%0A", "%0B", "%0C", "%0D", "%0E", "%0F",
            "%10", "%11", "%12", "%13", "%14", "%15", "%16", "%17", "%18", "%19", "%1A", "%1B", "%1C", "%1D", "%1E", "%1F",
            "%20", "%21", "%22", "%23", "%24", "%25", "%26", "%27", "%28", "%29", "%2A", "%2B", "%2C", "%2D", "%2E", "%2F",
            "%30", "%31", "%32", "%33", "%34", "%35", "%36", "%37", "%38", "%39", "%3A", "%3B", "%3C", "%3D", "%3E", "%3F",
            "%40", "%41", "%42", "%43", "%44", "%45", "%46", "%47", "%48", "%49", "%4A", "%4B", "%4C", "%4D", "%4E", "%4F",
            "%50", "%51", "%52", "%53", "%54", "%55", "%56", "%57", "%58", "%59", "%5A", "%5B", "%5C", "%5D", "%5E", "%5F",
            "%60", "%61", "%62", "%63", "%64", "%65", "%66", "%67", "%68", "%69", "%6A", "%6B", "%6C", "%6D", "%6E", "%6F",
            "%70", "%71", "%72", "%73", "%74", "%75", "%76", "%77", "%78", "%79", "%7A", "%7B", "%7C", "%7D", "%7E", "%7F",
            "%80", "%81", "%82", "%83", "%84", "%85", "%86", "%87", "%88", "%89", "%8A", "%8B", "%8C", "%8D", "%8E", "%8F",
            "%90", "%91", "%92", "%93", "%94", "%95", "%96", "%97", "%98", "%99", "%9A", "%9B", "%9C", "%9D", "%9E", "%9F",
            "%A0", "%A1", "%A2", "%A3", "%A4", "%A5", "%A6", "%A7", "%A8", "%A9", "%AA", "%AB", "%AC", "%AD", "%AE", "%AF",
            "%B0", "%B1", "%B2", "%B3", "%B4", "%B5", "%B6", "%B7", "%B8", "%B9", "%BA", "%BB", "%BC", "%BD", "%BE", "%BF",
            "%C0", "%C1", "%C2", "%C3", "%C4", "%C5", "%C6", "%C7", "%C8", "%C9", "%CA", "%CB", "%CC", "%CD", "%CE", "%CF",
            "%D0", "%D1", "%D2", "%D3", "%D4", "%D5", "%D6", "%D7", "%D8", "%D9", "%DA", "%DB", "%DC", "%DD", "%DE", "%DF",
            "%E0", "%E1", "%E2", "%E3", "%E4", "%E5", "%E6", "%E7", "%E8", "%E9", "%EA", "%EB", "%EC", "%ED", "%EE", "%EF",
            "%F0", "%F1", "%F2", "%F3", "%F4", "%F5", "%F6", "%F7", "%F8", "%F9", "%FA", "%FB", "%FC", "%FD", "%FE", "%FF"
        };
            #endregion

            #region public methods
            public static bool IsEncodedString(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return false;

                bool res = true;
                try
                {
                    string[] splittedString = SplitStrings(str);
                }
                catch
                {
                    res = false;
                }
                return res;
            }

            public static string Encode(string[] secondaryFieldNames)
            {
                return CombineStrings(secondaryFieldNames);
            }

            public static string[] Decode(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return new string[0];
                return SplitStrings(str);
            }

            public static string ConvertToSP2010(string str)
            {
                if (IsEncodedString(str))
                    return str;

                string[] fieldNames = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                string encodedVal = CombineStrings(fieldNames);
                return encodedVal;
            }
            #endregion

            #region internal methods
            private static string[] SplitStrings(string combinedEncoded)
            {
                string[] array = null;
                ArrayList list = new ArrayList();
                if ("0" == combinedEncoded)
                    return new string[0];
                try
                {
                    string str = UrlKeyValueDecode(combinedEncoded);
                    string[] strArray2 = str.Split(new char[] { ' ' }, StringSplitOptions.None);
                    int result = 0;
                    if ((strArray2 == null) || !int.TryParse(strArray2[strArray2.Length - 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                        throw new ArgumentException(string.Empty, "combinedEncoded");
                    int num2 = str.LastIndexOf(' ');
                    string str2 = str.Substring(result, num2 - result);
                    int length = str2.Length;
                    int index = 0;
                    int startIndex = 0;
                    while (startIndex < length)
                    {
                        string s = strArray2[index];
                        int num6 = 1;
                        if ((s != null) && (s.Length == 0))
                            list.Add(null);
                        else
                        {
                            if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num6))
                                throw new ArgumentException(string.Empty, "combinedEncoded");
                            list.Add(str2.Substring(startIndex, num6 - 1));
                        }
                        startIndex += num6;
                        index++;
                    }
                    array = new string[list.Count];
                    list.CopyTo(array);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException(string.Empty, "combinedEncoded", exception);
                }
                return array;
            }

            private static string UrlKeyValueDecode(string keyOrValueToDecode)
            {
                if (string.IsNullOrEmpty(keyOrValueToDecode))
                    return keyOrValueToDecode;
                return UrlDecodeHelper(keyOrValueToDecode, keyOrValueToDecode.Length, true);
            }

            private static string UrlDecodeHelper(string stringToDecode, int length, bool decodePlus)
            {
                if ((stringToDecode == null) || (stringToDecode.Length == 0))
                    return stringToDecode;
                StringBuilder builder = new StringBuilder(length);
                byte[] bytes = null;
                int nIndex = 0;
                while (nIndex < length)
                {
                    char ch = stringToDecode[nIndex];
                    if (ch < ' ')
                        nIndex++;
                    else
                    {
                        if (decodePlus && (ch == '+'))
                        {
                            builder.Append(" ");
                            nIndex++;
                            continue;
                        }
                        if (IsHexEscapedChar(stringToDecode, nIndex, length))
                        {
                            if (bytes == null)
                                bytes = new byte[(length - nIndex) / 3];
                            int count = 0;
                            do
                            {
                                int num3 = (FromHexNoCheck(stringToDecode[nIndex + 1]) * 0x10) + FromHexNoCheck(stringToDecode[nIndex + 2]);
                                bytes[count++] = (byte)num3;
                                nIndex += 3;
                            }
                            while (IsHexEscapedChar(stringToDecode, nIndex, length));
                            builder.Append(Encoding.UTF8.GetChars(bytes, 0, count));
                            continue;
                        }
                        builder.Append(ch);
                        nIndex++;
                    }
                }
                if (length < stringToDecode.Length)
                    builder.Append(stringToDecode.Substring(length));
                return builder.ToString();
            }

            private static bool IsHexEscapedChar(string str, int nIndex, int nPathLength)
            {
                if ((((nIndex + 2) >= nPathLength) || (str[nIndex] != '%')) || (!IsHexDigit(str[nIndex + 1]) || !IsHexDigit(str[nIndex + 2])))
                    return false;
                if (str[nIndex + 1] == '0')
                    return (str[nIndex + 2] != '0');
                return true;
            }

            private static bool IsHexDigit(char digit)
            {
                if ((('0' > digit) || (digit > '9')) && (('a' > digit) || (digit > 'f')))
                    return (('A' <= digit) && (digit <= 'F'));
                return true;
            }

            private static int FromHexNoCheck(char digit)
            {
                if (digit <= '9')
                    return (digit - '0');
                if (digit <= 'F')
                    return ((digit - 'A') + 10);
                return ((digit - 'a') + 10);
            }

            private static string CombineStrings(string[] strings)
            {
                StringBuilder builder = new StringBuilder();
                int index = 0;
                for (int i = 0; i < strings.Length; i++)
                {
                    string str = strings[i];
                    string str2 = ((str != null) ? ((str.Length + 1)).ToString(CultureInfo.InvariantCulture) : string.Empty) + ' ';
                    builder.Insert(index, str2);
                    index += str2.Length;
                    builder.Append(str + ' ');
                }
                builder.Append(index.ToString(CultureInfo.InvariantCulture));
                return UrlKeyValueEncode(builder.ToString());
            }

            private static string UrlKeyValueEncode(string keyOrValueToEncode)
            {
                if ((keyOrValueToEncode == null) || (keyOrValueToEncode.Length == 0))
                    return keyOrValueToEncode;
                StringBuilder sb = new StringBuilder(0xff);
                HtmlTextWriter output = new HtmlTextWriter(new StringWriter(sb, CultureInfo.InvariantCulture));
                UrlKeyValueEncode(keyOrValueToEncode, output);
                return sb.ToString();
            }

            private static void UrlKeyValueEncode(string keyOrValueToEncode, TextWriter output)
            {
                if (((keyOrValueToEncode != null) && (keyOrValueToEncode.Length != 0)) && (output != null))
                {
                    bool fUsedNextChar = false;
                    int startIndex = 0;
                    int length = 0;
                    int num3 = keyOrValueToEncode.Length;
                    for (int i = 0; i < num3; i++)
                    {
                        char ch = keyOrValueToEncode[i];
                        if (((('0' <= ch) && (ch <= '9')) || (('a' <= ch) && (ch <= 'z'))) || (('A' <= ch) && (ch <= 'Z')))
                            length++;
                        else
                        {
                            if (length > 0)
                            {
                                output.Write(keyOrValueToEncode.Substring(startIndex, length));
                                length = 0;
                            }
                            UrlEncodeUnicodeChar(output, keyOrValueToEncode[i], (i < (num3 - 1)) ? keyOrValueToEncode[i + 1] : '\0', out fUsedNextChar);
                            if (fUsedNextChar)
                                i++;
                            startIndex = i + 1;
                        }
                    }
                    if ((startIndex < num3) && (output != null))
                        output.Write(keyOrValueToEncode.Substring(startIndex));
                }
            }

            private static void UrlEncodeUnicodeChar(TextWriter output, char ch, char chNext, out bool fUsedNextChar)
            {
                bool fInvalidUnicode = false;
                UrlEncodeUnicodeChar(output, ch, chNext, ref fInvalidUnicode, out fUsedNextChar);
            }

            private static void UrlEncodeUnicodeChar(TextWriter output, char ch, char chNext, ref bool fInvalidUnicode, out bool fUsedNextChar)
            {
                int num = 0xc0;
                int num2 = 0xe0;
                int num3 = 240;
                int num4 = 0x80;
                int num5 = 0xd800;
                int num6 = 0xfc00;
                int num7 = 0x10000;
                fUsedNextChar = false;
                int index = ch;
                if (index <= 0x7f)
                    output.Write(s_crgstrUrlHexValue[index]);
                else
                {
                    int num8;
                    if (index <= 0x7ff)
                    {
                        num8 = num | (index >> 6);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | (index & 0x3f);
                        output.Write(s_crgstrUrlHexValue[num8]);
                    }
                    else if ((index & num6) != num5)
                    {
                        num8 = num2 | (index >> 12);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | ((index & 0xfc0) >> 6);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | (index & 0x3f);
                        output.Write(s_crgstrUrlHexValue[num8]);
                    }
                    else if (chNext != '\0')
                    {
                        index = (index & 0x3ff) << 10;
                        fUsedNextChar = true;
                        index |= chNext & 'Ͽ';
                        index += num7;
                        num8 = num3 | (index >> 0x12);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | ((index & 0x3f000) >> 12);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | ((index & 0xfc0) >> 6);
                        output.Write(s_crgstrUrlHexValue[num8]);
                        num8 = num4 | (index & 0x3f);
                        output.Write(s_crgstrUrlHexValue[num8]);
                    }
                    else
                        fInvalidUnicode = true;
                }
            }
            #endregion
        }
        #endregion

        #region [properties]
        public static readonly string LOBRequestSystemName      = "CoordinateV5";
        public static readonly string LOBTaxiSystemName         = "Taxi";
        public static readonly string LOBTaxiV2SystemName       = "TaxiV2";
        public static readonly string LOBUtilitySystemName      = "Utility";

        public static readonly string LOBRequestSystemNamespace = "TM.SP.BCSModels.CoordinateV5";
        public static readonly string LOBTaxiSystemNamespace    = "TM.SP.BCSModels.Taxi";
        public static readonly string LOBTaxiV2SystemNamespace  = "TM.SP.BCSModels.TaxiV2";
        public static readonly string LOBUtilitySystemNamespace = "TM.SP.BCSModels.Utility";
        #endregion

        #region [public methods]
        public static string GetLobSystemProperty(ILobSystemInstance lob, string propertyName)
        {
            INamedPropertyDictionary props = lob.GetProperties();

            if (props.ContainsKey(propertyName))
            {
                return props[propertyName].ToString();
            }
            else
            {
                throw new ArgumentException("Can't find LOB system property", propertyName);
            }
        }

        public static IEntity GetEntity(SPServiceContext context, string bdcServiceName, string @namespace, string name)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            BdcService svcInstance = String.IsNullOrEmpty(bdcServiceName) ? SPFarm.Local.Services.GetValue<BdcService>() : SPFarm.Local.Services.GetValue<BdcService>(bdcServiceName);
            if (svcInstance == null)
                throw new Exception("No BDC Service Application found");

            DatabaseBackedMetadataCatalog catalog = svcInstance.GetDatabaseBackedMetadataCatalog(context);
            return catalog.GetEntity(@namespace, name);
        }

        public static object GetDataFromMethod(string lobSystemName, IEntity externalCT, string methodName, MethodInstanceType methodType, ref object[] args)
        {
            var lob     = externalCT.GetLobSystem();
            var lobi    = lob.GetLobSystemInstances()[lobSystemName];
            var mi      = externalCT.GetMethodInstance(methodName, methodType);

            object[] _args = new object[mi.GetMethod().GetParameters().Count];
            if (args.Length > _args.Length)
                throw new Exception("Parameters count is more than method accepts");
            args.CopyTo(_args, 0);

            object retVal = externalCT.Execute(mi, lobi, ref _args);
            args = _args;
            return retVal;
        }

        public static void SetBCSFieldValue(SPListItem item, string bcsFieldInternalName, object bcsLookupObject, string lookupObjectIdentityName = "Id")
        {
            SPField field = item.Fields.GetFieldByInternalName(bcsFieldInternalName);
            if (field == null)
                throw new Exception("BCS field not found by internal name");

            XmlDocument fieldSchema = new XmlDocument();
            fieldSchema.LoadXml(field.SchemaXml);

            string identityField = fieldSchema.FirstChild.Attributes["RelatedFieldWssStaticName"].Value;
            string valueField = fieldSchema.FirstChild.Attributes["BdcField"].Value;
            string identity = EntityInstanceIdEncoder.EncodeEntityInstanceId(new object[] { Reflection.GetPropertyValue<Object>(bcsLookupObject, lookupObjectIdentityName) });

            item[identityField] = identity;
            item[bcsFieldInternalName] = Reflection.GetPropertyValue<Object>(bcsLookupObject, valueField);
        }

        public static object GetBCSFieldLookupId(SPListItem item, string bcsFieldInternalName)
        {
            var fn = GetLookupIdentityFieldName(item, bcsFieldInternalName);
            if (item[fn] == null)
                throw new Exception(String.Format("Lookup field {0} contains no identity value", bcsFieldInternalName));

            object[] decodedId = EntityInstanceIdEncoder.DecodeEntityInstanceId(item[fn].ToString());

            return decodedId[0];
        }

        public static TEntityType ExecuteBcsMethod<TEntityType>(BcsMethodExecutionInfo methodInfo, object inParam)
        {
            var contentType = GetEntity(SPServiceContext.Current, String.Empty, methodInfo.ns, methodInfo.contentType);

            var args = new List<object>();
            if (inParam != null)
            {
                if (inParam is IEnumerable<object>)
                {
                    var paramList = inParam as IEnumerable<object>;
                    args.AddRange(paramList);
                }
                else
                {
                    args.Add(inParam);
                }
            }
            var parameters = args.ToArray();

            return (TEntityType)GetDataFromMethod(methodInfo.lob, contentType, methodInfo.methodName, methodInfo.methodType, ref parameters);
        }

        #endregion

        #region [private methods]
        private static string GetLookupIdentityFieldName(SPField lookupField)
        {
            XmlDocument schema = new XmlDocument();
            schema.LoadXml(lookupField.SchemaXml);

            return schema.FirstChild.Attributes["RelatedFieldWssStaticName"].Value;
        }

        private static string GetLookupIdentityFieldName(SPListItem item, string bcsFieldInternalName)
        {
            SPFieldCollection fields = item.ParentList.Fields;
            SPField field = fields.GetFieldByInternalName(bcsFieldInternalName);
            if (field == null)
                throw new Exception("BCS field not found by internal name");

            return GetLookupIdentityFieldName(field);
        }
        #endregion
    }
}
