using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TM.Utils.Sql
{
    public class XmlToSql
    {
        #region [delegates]
        public delegate void NoSuchColumnCallback(DataRow dr, XElement node);
        #endregion

        #region [properties]
        private string ConnectionString { get; set; }
        private string TableName { get; set; }
        private string TableNamespace { get; set; }
        private string XmlNamespace { get; set; }
        private string XmlItemTag { get; set; }
        #endregion

        #region [methods]
        private XmlToSql() {}
        private bool Initialized()
        {
            return !String.IsNullOrEmpty(ConnectionString) 
                && !String.IsNullOrEmpty(TableName) 
                && !String.IsNullOrEmpty(TableNamespace) 
                && !String.IsNullOrEmpty(XmlNamespace) 
                && !String.IsNullOrEmpty(XmlItemTag);
        }
        public static IEnumerable<XElement> SelectXmlElements(string xml, string nsName, string elementTagName)
        {
            XDocument xdoc = XDocument.Parse(xml);
            var ns = xdoc.Root.Attributes()
                .Where(a => a.IsNamespaceDeclaration && a.Name.LocalName == nsName)
                .Select(a => XNamespace.Get(a.Value))
                .First();

            XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace(nsName, ns.NamespaceName);

            var xpath = String.Format("//{0}:{1}", nsName, elementTagName);
            return xdoc.XPathSelectElements(xpath, nsManager);
        }
        public void Transfer(string xml, NoSuchColumnCallback cb)
        {
            if (!Initialized())
                throw new Exception(String.Format(StringsRes.EntityInitErrorFmt, this.GetType().Name));

            using (var carrier = new SqlBulkCopy(ConnectionString) { DestinationTableName = TableName })
            {
                var elements = SelectXmlElements(xml, XmlNamespace, XmlItemTag);
                var dt = SqlHelper.CreateDataTableStub(ConnectionString, TableNamespace, TableName);

                foreach (XElement str in elements)
                {
                    var dr = dt.NewRow();
                    foreach (XElement node in str.Elements())
                    {
                        var column = node.Name.LocalName;
                        if (dt.Columns.Contains(column))
                        {
                            dr[column] = node.Value;
                        }
                        else
                        {
                            if (cb != null) cb.Invoke(dr, node);
                        }
                    }

                    dt.Rows.Add(dr);
                }

                carrier.WriteToServer(dt);
            }
        }
        public static XmlToSql GetForPenaltyV5(string connectionString)
        {
            return new XmlToSql()
            {
                ConnectionString = connectionString,
                TableNamespace   = "dbo",
                TableName        = "GibddPenaltyV5",
                XmlNamespace     = "ns2",
                XmlItemTag       = "Result"
            };
        }

        #endregion
    }
}
