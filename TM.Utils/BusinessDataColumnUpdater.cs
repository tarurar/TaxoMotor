using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.Administration;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel.Collections;
using Microsoft.BusinessData.Runtime;
using Microsoft.SharePoint.Administration;

using TM.ClientUtils.Bcs;

namespace TM.Utils
{
    public class BusinessDataColumnUpdater
    {
        #region private Data members

        protected SPList _list                   = null;
        protected string _columnName             = "";
        protected ILobSystemInstance _lobSysInst = null;
        protected IEntity _entity                = null;
        protected IView _specificFinderView      = null;
        protected SPListItemCollection _items    = null;
        protected StringBuilder _traceMessages   = new StringBuilder();
        protected bool _enableTracing            = false;
        protected string _batchFormat            = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ows:Batch OnError=\"Return\">{0}</ows:Batch>";
        protected string _batch                  = string.Empty;
        protected StringBuilder _methodBuilder   = new StringBuilder();

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
        public virtual void UpdateColumnUsingBatch()
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
            string[] secondaryWssFieldNames = SecondaryFieldNamesHelper.Decode(property);

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
            query.RowLimit = totalListItems < 2000 ? (uint)totalListItems : (uint)2000;
            query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", _columnName, relatedFieldName);
            query.ViewAttributes = "Scope=\"Recursive\""; //you must set this value to recursive to update things in subfolders

            //retrieve SPListItemCollection using paging techniques for optimization
            do
            {
                _items = _list.GetItems(query);
                _methodBuilder = new StringBuilder();
                //loop through list items in list and try to update/refresh with latest BDC value
                foreach (SPListItem item in _items)
                {
                    //make sure the item has data in the column before trying to update
                    if (!(item[_columnName] == null) && !(item[relatedFieldName] == null))
                    {
                        BuildListItemBatchUpdateCAML(item, bizDataField, _lobSysInst, _entity,
                           _specificFinderView, secondaryFieldsNames, secondaryWssFieldNames);
                    }
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
            catch (Microsoft.BusinessData.Runtime.ObjectNotFoundException notFoundEx)
            {

            }
            catch (Exception genericErr)
            {
            }

        } //end method

        #endregion

    } //end class
}
