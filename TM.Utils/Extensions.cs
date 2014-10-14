using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
using Microsoft.SharePoint;


namespace TM.Utils
{
    public static class Extensions
    {
        public static XElement ToXElement<T>(this object obj)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(mStream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));        
                    serializer.Serialize(textWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(mStream.ToArray()));
                }
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable t = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                t.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
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

        public static SPListItem GetItemOrBreak(this SPList list, int Id)
        {
            SPListItem retVal;

            try 
	        {	        
		        retVal = list.GetItemById(Id);
	        }
            catch (ArgumentException ex)
	        {
                throw new Exception(String.Format("Item with Id = {0} in list named {1} does not exist", Id, list.Title));
	        }

            return retVal;
            
        }

        public static SPListItem GetItemOrNull(this SPList list, int Id)
        {
            SPListItem retVal;

            try
            {
                retVal = list.GetItemById(Id);
            }
            catch (ArgumentException ex)
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
            catch (FileNotFoundException ex)
            {
                throw new Exception(String.Format("List with url = {0} does not exist", url));
            }

            return retVal;

        }

        public static SPFolder CreateSubFolders(this SPFolder parentFolder, params string[] path)
        {
            if (!parentFolder.Exists)
                throw new FileNotFoundException(String.Format("Folder {0} does not exist"), parentFolder.Name);

            // remove illegal characters
            string pattern     = "~|\"|#|%|&|\\*|:|<|>|\\?|\\/|\\\\|{|\\||}|\\W*$|^\\W*";
            string correctPath = Regex.Replace(path[0], pattern, " ").Trim();

            SPFolder newFolder = null;
            if (parentFolder.DocumentLibrary != null)
            {
                newFolder = parentFolder.SubFolders.Add(correctPath);
                parentFolder.Update();
            }
            else
            {
                SPList parentlist = parentFolder.ParentWeb.Lists.GetList(parentFolder.ParentListId, false);
                SPListItem newFolderItem = parentlist.Items.Add(parentFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                newFolderItem["Title"] = correctPath;
                newFolderItem.Update();
                newFolder = newFolderItem.Folder;
            }
            var newPath = path.Skip(1).ToArray();

            return (newPath.Length >= 1) ? newFolder.CreateSubFolders(newPath) : newFolder;
        }
    }
}
