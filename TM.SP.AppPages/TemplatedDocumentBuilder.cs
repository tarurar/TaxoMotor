using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CamlexNET;
using Microsoft.SharePoint;
using TM.Utils;
using Aspose.Words;
using AsposeLicense = Aspose.Words.License;

namespace TM.SP.AppPages
{
    public struct DocumentMetaData
    {
        public int DocumentId;
        public string DocumentUrl;
    }

    public class TemplatedDocumentBuilder
    {
        #region [properties]
        private SPWeb _web;
        private SPListItem _request;
        private SPList _tmplLib;
        private SPList _attachLib;
        private AsposeLicense _asposeLic;
        private SPList _taxiList;

        public SPListItem RequestedDocument
        {
            get
            {
                SPListItem el;

                return Utility.TryGetListItemFromLookupValue(_request["Tm_RequestedDocument"],
                    _request.Fields.GetFieldByInternalName("Tm_RequestedDocument") as SPFieldLookup, out el)
                    ? el
                    : null;
            }
        }

        public SPListItem DenyReason
        {
            get
            {
                SPListItem el;

                return Utility.TryGetListItemFromLookupValue(_request["Tm_DenyReasonLookup"],
                    _request.Fields.GetFieldByInternalName("Tm_DenyReasonLookup") as SPFieldLookup, out el)
                    ? el
                    : null;
            }
        }

        public SPListItem RequestStatus
        {
            get
            {
                SPListItem el;

                return Utility.TryGetListItemFromLookupValue(_request["Tm_IncomeRequestStateLookup"],
                    _request.Fields.GetFieldByInternalName("Tm_IncomeRequestStateLookup") as SPFieldLookup, out el)
                    ? el
                    : null;
            }
        }

        public bool NeedPersonVisit
        {
            get { return Convert.ToBoolean(_request["Tm_NeedPersonVisit"]); }
        }

        public bool RefuseDocuments
        {
            get { return Convert.ToBoolean(_request["Tm_RefuseDocuments"]); }
        }

        public string RegistrationDateText
        {
            get
            {
                return _request["Tm_RegistrationDate"] != null
                    ? DateTime.Parse(_request["Tm_RegistrationDate"].ToString()).ToString("dd.MM.yyyy")
                    : "Дата регистрации не указана";
            }
        }

        #endregion

        #region [methods]

        public TemplatedDocumentBuilder(SPWeb web, int incomeRequestId)
        {
            _web       = web;
            _request   = _web.GetListOrBreak("Lists/IncomeRequestList").GetItemById(incomeRequestId);
            _tmplLib   = _web.GetListOrBreak("DocumentTemplateLib");
            _attachLib = _web.GetListOrBreak("AttachLib");
            _taxiList  = _web.GetListOrBreak("Lists/TaxiList");
            _asposeLic = new AsposeLicense();
            _asposeLic.SetLicense("Aspose.Total.lic");
        }

        private SPListItemCollection GetTaxiItemsInStatus(string statusChoiceValue)
        {
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => x["Tm_TaxiStatus"] == (DataTypes.Choice) statusChoiceValue,
                x =>
                    x["Tm_IncomeRequestLookup"] ==
                    (DataTypes.LookupId) _request.ID.ToString(CultureInfo.InvariantCulture)
            };

            return _taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });
        }

        public DocumentMetaData RenderDocument(int templateNumber)
        {
            var tmplItem = _tmplLib.GetSingleListItemByFieldValue("Tm_ServiceCode",
                templateNumber.ToString(CultureInfo.InvariantCulture));
            if (tmplItem == null)
                throw new Exception(String.Format("Template with number {0} doesn't exist", templateNumber));

            SPListItemCollection taxiList = null;
            switch (templateNumber)
            {
                case 4:
                case 5:
                    taxiList = GetTaxiItemsInStatus("Решено положительно");
                    break;
                case 6:
                    taxiList = GetTaxiItemsInStatus("Отказано");
                    break;
            }

            var scalarValueNames = new string[]
            {
                "DeclarantName", "CreationDate", "SingleNumber", "SubServiceName",
                "RefuseReasonTitle", "RefuseReasonText", "OperatorDepartment", "OperatorName"
            };

            var scalarValues = new object[]
            {
                _request["Tm_RequestAccountBCSLookup"] ?? "",
                RegistrationDateText,
                _request["Tm_SingleNumber"] ?? "",
                RequestedDocument != null ? RequestedDocument.Title : "",
                DenyReason != null ? DenyReason.Title : "",
                _request["Tm_Comment"] ?? "",
                "",
                _web.CurrentUser.Name
            };

            var doc = new Document(tmplItem.File.OpenBinaryStream());

            doc.MailMerge.Execute(scalarValueNames, scalarValues);
            if (taxiList != null && taxiList.Count > 0)
            {
                var dt = taxiList.GetDataTable();
                dt.TableName = "TaxiList";
                doc.MailMerge.ExecuteWithRegions(dt);
            }

            using (var ms = new MemoryStream())
            {
                doc.Save(ms, SaveFormat.Pdf);
                var parentFolder = _attachLib.RootFolder.CreateSubFolders(new[]
                {
                    DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                    DateTime.Now.Month.ToString(CultureInfo.InvariantCulture),
                    _request.Title
                });

                var uplFolder =
                    parentFolder.CreateSubFolders(new[]
                    {(parentFolder.ItemCount + 1).ToString(CultureInfo.InvariantCulture)});
                var fn = Path.HasExtension(tmplItem.File.Name)
                    ? Path.ChangeExtension(tmplItem.File.Name, "pdf")
                    : tmplItem.File.Name + ".pdf";
                var templatedDoc = uplFolder.Files.Add(fn, ms);
                uplFolder.Update();

                templatedDoc.Item["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(_request.ID, _request.Title);
                templatedDoc.Item.Update();

                return new DocumentMetaData
                {
                    DocumentId = templatedDoc.Item.ID,
                    DocumentUrl = templatedDoc.ServerRelativeUrl
                };
            }
        }

        #endregion
    }
}
