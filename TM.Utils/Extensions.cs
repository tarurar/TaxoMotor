using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
using Microsoft.SharePoint;


namespace TM.Utils
{
    public static class Extensions
    {
        static System.Text.RegularExpressions.Regex tagRegex
                    = new System.Text.RegularExpressions.Regex("<[^>]*>", System.Text.RegularExpressions.RegexOptions.Compiled);
        public static XElement ToXElement<T>(this object obj)
        {
            using (var mStream = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(mStream))
                {
                    var serializer = new XmlSerializer(typeof(T));        
                    serializer.Serialize(textWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(mStream.ToArray()));
                }
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var t = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                t.Columns.Add(prop.Name, prop.PropertyType);
            }
            var values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                t.Rows.Add(values);
            }
            return t;
        }

        public static SPListItem GetItemOrBreak(this SPList list, int id)
        {
            SPListItem retVal;

            try 
	        {	        
		        retVal = list.GetItemById(id);
	        }
            catch (ArgumentException)
	        {
                throw new Exception(String.Format("Item with Id = {0} in list named {1} does not exist", id, list.Title));
	        }

            return retVal;
            
        }

        public static SPListItem GetItemOrNull(this SPList list, int id)
        {
            SPListItem retVal;

            try
            {
                retVal = list.GetItemById(id);
            }
            catch (ArgumentException)
            {
                retVal = null;
            }

            return retVal;
        }

        public static SPList GetListOrBreak(this SPWeb web, string url)
        {
            SPList retVal;

            try
            {
                retVal = web.GetList(url);
            }
            catch (FileNotFoundException)
            {
                throw new Exception(String.Format("List with url = {0} does not exist", url));
            }

            return retVal;

        }

        public static SPFolder CreateSubFolders(this SPFolder parentFolder, params string[] path)
        {
            if (!parentFolder.Exists)
                throw new FileNotFoundException(String.Format("Folder {0} does not exist", parentFolder.Name));

            // remove illegal characters
            const string pattern = "~|\"|#|%|&|\\*|:|<|>|\\?|\\/|\\\\|{|\\||}|\\W*$|^\\W*";
            string correctPath = Regex.Replace(path[0], pattern, " ").Trim();

            SPFolder newFolder;
            if (parentFolder.DocumentLibrary != null)
            {
                newFolder = parentFolder.SubFolders.Add(correctPath);
                parentFolder.Update();
            }
            else
            {
                SPWeb web = parentFolder.ParentWeb;
                // checking existance
                newFolder = web.GetFolder(parentFolder.Url + "/" + correctPath);
                if (!newFolder.Exists)
                {
                    SPList parentList = web.Lists.GetList(parentFolder.ParentListId, false);
                    SPListItem newFolderItem = parentList.AddItem(parentFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                    newFolderItem["Title"] = correctPath;
                    newFolderItem.Update();
                    newFolder = newFolderItem.Folder;
                }
            }
            var newPath = path.Skip(1).ToArray();

            return (newPath.Length >= 1) ? newFolder.CreateSubFolders(newPath) : newFolder;
        }

        public static bool IsJavascriptNullDate(this DateTime date)
        {
            var jsNullDate = new DateTime(1970, 1, 1);
            return jsNullDate == date;
        }

        public static List<SPListItem> GetListItemsByFieldValue(this SPList list, string fn, string match)
        {
            List<SPListItem> matchingItems =
                (from SPListItem listItem in list.Items
                 where
                     listItem.Fields.ContainsField(fn) &&
                     listItem[fn] != null &&
                     listItem[fn].ToString().Equals(match, StringComparison.InvariantCultureIgnoreCase)
                 select listItem).ToList<SPListItem>();

            return matchingItems;
        }

        public static SPListItem GetSingleListItemByFieldValue(this SPList list, string fn, string match)
        {
            List<SPListItem> items = GetListItemsByFieldValue(list, fn, match);
            if (items.Count > 1)
                throw new Exception(String.Format("Expected single value in a list {0} for specified field name {1}", list.Title, fn));

            return items.Count > 0 ? items[0] : null;
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static bool IsFieldChanged(this SPItemEventProperties properties, string fieldName)
        {
            return properties.IsFieldChanged(properties.List.Fields.GetFieldByInternalName(fieldName));
        }

        public static bool IsFieldChanged(this SPItemEventProperties properties, SPField field)
        {
            var after = (string)properties.AfterProperties[field.InternalName];

            if (properties.ListItem == null)
            {
                return false;
            }

            var before = Convert.ToString(properties.ListItem[field.Id]);

            //AfterProperties[fieldname] == null - field not changed
            if (after == null)
            {
                return false;
            }

            //AfterProperties[fieldname] == "" - field set to null
            if (after == "" && string.IsNullOrEmpty(before))
            {
                return false;
            }

            //AfterProperties[fieldname] != "", old value is null or empty - field changed
            if (string.IsNullOrEmpty(before))
            {
                return true;
            }

            var afterValue = field.GetFieldValue(after);
            var beforeValue = field.GetFieldValue(before);

            if (afterValue.Equals(beforeValue))
            {
                return false;
            }

            //Compare SPFieldLookupValue and SPFieldUserValue
            if (afterValue is SPFieldLookupValue)
            {
                return (afterValue as SPFieldLookupValue).LookupId != (beforeValue as SPFieldLookupValue).LookupId;
            }

            //Compare SPFieldMultiChoiceValue
            if (afterValue is SPFieldMultiChoiceValue)
            {
                return !EqualChoices(afterValue as SPFieldMultiChoiceValue, beforeValue as SPFieldMultiChoiceValue);
            }

            //Compare strings
            if (afterValue is string)
            {
                //normalize tags on rich edit fields
                if (field is SPFieldMultiLineText && (field as SPFieldMultiLineText).RichText)
                {
                    var a = tagRegex.Replace(afterValue as string, m => m.Value.ToLower());
                    var b = tagRegex.Replace(beforeValue as string, m => m.Value.ToLower());
                    return !a.Equals(b);
                }
            }

            //Compare SPFieldLookupValueCollection and SPFieldUserValueCollection
            if (field is SPFieldLookup && (field as SPFieldLookup).AllowMultipleValues)
            {
                var hsa = new HashSet<int>((afterValue as SPFieldLookupValueCollection).OfType<SPFieldLookupValue>().Select(l => l.LookupId));
                var hsb = new HashSet<int>((beforeValue as SPFieldLookupValueCollection).OfType<SPFieldLookupValue>().Select(l => l.LookupId));
                return !hsa.SetEquals(hsb);
            }

            return after != before;
        }

        private static bool EqualChoices(SPFieldMultiChoiceValue a, SPFieldMultiChoiceValue b)
        {
            if (a.Count != b.Count) return false;

            var hsa = new HashSet<string>();
            var hsb = new HashSet<string>();
            for (int i = 0; i < a.Count; i++)
            {
                hsa.Add(a[i]);
                hsb.Add(b[i]);
            }

            return hsa.SetEquals(hsb);
        }

        public static T TryGetValue<T>(this SPListItem listItem, string fieldInternalName)
        {
            if (!String.IsNullOrEmpty(fieldInternalName) && 
                listItem != null && 
                listItem.Fields.ContainsField(fieldInternalName))
            {
                object value = listItem[fieldInternalName];
                if (value != null)
                    return (T)value;
            }

            return default(T);
        }

        public static bool TrySetValue<T>(this SPListItem listItem, string fieldInternalName, T value)
        {
            try
            {
                if (!String.IsNullOrEmpty(fieldInternalName) &&
                listItem != null &&
                listItem.Fields.ContainsField(fieldInternalName))
                {
                    listItem[fieldInternalName] = value;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
