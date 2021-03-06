﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using TM.Utils;
using Aspose.Words;
using AsposeLicense = Aspose.Words.License;
using RequestAccount = TM.SP.BCSModels.CoordinateV5.RequestAccount;
using CamlexNET.Impl.Helpers;
using System.Data;

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
        private RequestAccount _declarant;

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

        public SPListItem GetDenyReasonItem(int num)
        {
            SPListItem el;

            var fieldname = "Tm_DenyReasonLookup" + (num > 1 ? num.ToString() : "");
            if (_request.Fields.ContainsField(fieldname)) 
            {
                var lookupField = _request.Fields.GetFieldByInternalName(fieldname) as SPFieldLookup;
                return Utility.TryGetListItemFromLookupValue(_request[fieldname], lookupField, out el) 
                    ? el 
                    : null;
            } else return null;
        }

        public string GetDenyComment(int num)
        {
            var fieldname = "Tm_Comment" + (num > 1 ? num.ToString() : "");
            if (_request.Fields.ContainsField(fieldname))
            {
                return _request.TryGetValue<string>(fieldname);
            }
            else return null;
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

        public string ApplyDateText
        {
            get
            {
                return _request["Tm_ApplyDate"] != null
                    ? DateTime.Parse(_request["Tm_ApplyDate"].ToString()).ToString("dd.MM.yyyy")
                    : "Дата принятия в работу не указана";
            }
        }

        public string OperatorDepartment
        {
            get
            {
                var pManager  = new UserProfileManager(SPServiceContext.Current);
                var cuLogin   = SPContext.Current.Web.CurrentUser.LoginName;
                var cuProfile = pManager.GetUserProfile(cuLogin);
                
                if (cuProfile != null)
                {
                    var department = cuProfile[PropertyConstants.Department].Value;
                    return (string)department;
                }
                else return String.Empty;
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

            var declarantId = BCS.GetBCSFieldLookupId(_request, "Tm_RequestAccountBCSLookup");
            this._declarant = IncomeRequestHelper.ReadRequestAccountItem((int)declarantId);
        }

        private SPListItemCollection GetTaxiItemsInStatus(string statuses, List<Expression<Func<SPListItem, bool>>> additionalExprAnd)
        {
            var arrStatuses = statuses.Split(';');

            var statusConditions = new List<Expression<Func<SPListItem, bool>>>();
            foreach (string t in arrStatuses)
            {
                string token = t;
                statusConditions.Add(x => x["Tm_TaxiStatus"] == (DataTypes.Choice)t);
            }
            var statusExpr = ExpressionsHelper.CombineOr(statusConditions);

            var parentConditions = new List<Expression<Func<SPListItem, bool>>>
            {
                x =>
                    x["Tm_IncomeRequestLookup"] ==
                    (DataTypes.LookupId) _request.ID.ToString(CultureInfo.InvariantCulture)
            };
            var parentExpr = ExpressionsHelper.CombineOr(parentConditions);

            Expression<Func<SPListItem, bool>> additionalExpr = null;
            if (additionalExprAnd != null) {
                additionalExpr = ExpressionsHelper.CombineAnd(additionalExprAnd);
            }

            var expressions = new List<Expression<Func<SPListItem, bool>>> { parentExpr, statusExpr };
            if (additionalExpr != null)
            {
                expressions.Add(additionalExpr);
            }

            return _taxiList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });
        }

        private void AddExtraFields(DataTable dt)
        {
            #region DenyReasonPrintTitle
            dt.Columns.Add("DenyReasonPrintTitle", typeof(string));

            foreach(DataRow row in dt.Rows)
            {
                var taxiId = Convert.ToInt32(row["ID"]);
                if (taxiId > 0)
                {
                    var taxiItem = _taxiList.GetItemById(taxiId);
                    SPListItem denyReason;
                    Utility.TryGetListItemFromLookupValue(taxiItem["Tm_DenyReasonLookup"],
                        taxiItem.Fields.GetFieldByInternalName("Tm_DenyReasonLookup") as SPFieldLookup, out denyReason);
                    if (denyReason != null)
                    {
                        row["DenyReasonPrintTitle"] = denyReason["Tm_PrintTitle"];
                    }
                    else continue;

                }
                else continue;
            }
            # endregion
        }

        private DataTable GetDenyReasonDataTable()
        {
            var dt = new DataTable("DenyReasonList");

            dt.Columns.Add("ReasonText", typeof(string));
            dt.Columns.Add("Comment", typeof(string));

            #region filling with data
            for (int i = 1; i < 4; i++)
            {
                var reasonItem = GetDenyReasonItem(i);

                if (reasonItem != null)
                {
                    dt.Rows.Add(reasonItem["Tm_PrintTitle"], GetDenyComment(i));
                }
            }
            #endregion

            return dt;
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
                    taxiList = GetTaxiItemsInStatus("Решено положительно", null);
                    break;
                case 1:
                case 2:
                case 3:
                case 6:
                    // выводим только те ТС у которых действительно указана причина отказа
                    var addConditions = new List<Expression<Func<SPListItem, bool>>>() 
                    {
                        x => x["Tm_DenyReasonLookup"] != null
                    };

                    taxiList = GetTaxiItemsInStatus("Отказано;Решено отрицательно", addConditions);
                    break;
            }

            var scalarValueNames = new string[]
            {
                "DeclarantNamePE", "DeclarantNameJP", "CeoText", "CreationDate", "SingleNumber", "SubServiceName",
                "OperatorDepartment", "OperatorName", "ApplyDate", "InternalRegNumber"
            };

            bool isPrvtEntrprnr = _declarant.OrgFormCode.Equals(SendRequestEGRULPage.PrivateEntrepreneurCode);
            var scalarValues = new object[]
            {
                isPrvtEntrprnr ? _request["Tm_RequestAccountBCSLookup"] : String.Empty,
                isPrvtEntrprnr ? String.Empty : _request["Tm_RequestAccountBCSLookup"],
                isPrvtEntrprnr ? String.Empty : "Генеральному директору",
                RegistrationDateText,
                _request["Tm_SingleNumber"] ?? "",
                RequestedDocument != null ? RequestedDocument.Title : "",
                OperatorDepartment,
                _web.CurrentUser.Name,
                ApplyDateText,
                _request["Tm_InternalRegNumber"] ?? ""
            };

            var doc = new Document(tmplItem.File.OpenBinaryStream());

            doc.MailMerge.Execute(scalarValueNames, scalarValues);
            if (taxiList != null)
            {
                var dt = taxiList.GetDataTable() ?? new DataTable();
                if (dt.Rows.Count > 0)
                {
                    AddExtraFields(dt);
                }
                dt.TableName = "TaxiList";
                doc.MailMerge.ExecuteWithRegions(dt);
            }
            doc.MailMerge.ExecuteWithRegions(GetDenyReasonDataTable());

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
