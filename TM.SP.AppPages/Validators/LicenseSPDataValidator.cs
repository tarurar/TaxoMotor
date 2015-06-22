using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TM.SP.BCSModels.Taxi;
using TM.Utils;
using System.Xml.Serialization;
using TM.Utils.Attributes;
using System.Xml.Linq;

namespace TM.SP.AppPages.Validators
{
    /// <summary>
    /// Сравнение данных разрешения в подписанном xml и в списке SharePoint
    /// </summary>
    public class LicenseSPDataValidator: Validator
    {
        #region [fields]
        SPListItem spLicense;
        License sqlLicense;
        #endregion

        #region [methods]
        public LicenseSPDataValidator(SPWeb web, int licenseId): base(web)
        {
            SPList spList = _web.GetListOrBreak("Lists/LicenseList");
            this.spLicense = spList.GetItemOrBreak(licenseId);
            this.sqlLicense = LicenseHelper.GetLicense(Convert.ToInt32(this.spLicense["Tm_LicenseExternalId"]));
        }

        public override bool Execute(params object[] paramsList)
        {
            var valid = false;
            
            var fields = LicenseHelper.GetLicenseFieldsToCompare();
            foreach (MemberInfo mi in fields)
            {
                string xmlValue = GetXmlValue(mi);
                object spValue = GetSPValue(mi);

                if (String.IsNullOrEmpty(xmlValue))
                {
                    valid = spValue == null;
                }
                else
                {
                    valid = spValue != null ? CompareValues<string, object>(xmlValue, spValue, mi.GetFieldType()) : false;
                }

                if (!valid) break;
            }

            return valid;
        }

        private string GetXmlValue(MemberInfo mi)
        {
            var xmlStr = LicenseHelper.GetLicenseSavedXml(this.sqlLicense);
            var xmlDoc = XDocument.Parse(xmlStr);
            var xmlTag = mi.GetCustomAttribute<XmlElementAttribute>().ElementName;
            var elem = xmlDoc.Descendants().Where(n => n.Name.LocalName == xmlTag).FirstOrDefault();
            var value = elem != null ? elem.Value : null;

            return ApplyExclusionsXmlData(value, xmlTag);
        }

        private object GetSPValue(MemberInfo mi)
        {
            var fieldName = mi.GetCustomAttribute<SharepointFieldAttribute>().Name;
            var value = this.spLicense[fieldName];

            return ApplyExclusionsSPData(value, fieldName);
        }

        private object ApplyExclusionsSPData(object sourceValue, string fieldName)
        {
            var fixedValue = sourceValue;
            if (fieldName == "Tm_LicenseStatus")
            {
                var status = sourceValue.ToString();
                switch (status)
                {
                    case "Переоформлено":
                    case "Первичное": 
                        fixedValue = 0;
                        break;
                    case "Выдан дубликат":
                        fixedValue = 1;
                        break;
                    case "Приостановлено":
                        fixedValue = 2;
                        break;
                    case "Аннулировано":
                        fixedValue = 3;
                        break;
                    default:
                        fixedValue = -1;
                        break;
                }
            }

            return fixedValue;
        }

        private string ApplyExclusionsXmlData(string sourceValue, string tagName)
        {
            return sourceValue;
        }

        private bool CompareValues<T1, T2>(T1 v1, T2 v2, Type type)
        {
            var identical = false;

            if (type == typeof(string))
            {
                identical = v1.ToString() == v2.ToString();
            } else if (type == typeof(bool))
            {
                var op1 = bool.Parse(v1.ToString());
                var op2 = bool.Parse(v2.ToString());
                identical = op1 == op2;
            } else if (type == typeof(int))
            {
                var op1 = Int32.Parse(v1.ToString());
                var op2 = Int32.Parse(v2.ToString());
                identical = op1 == op2;
            } else if (type == typeof(DateTime))
            {
                var op1 = DateTime.Parse(v1.ToString()).Date;
                var op2 = DateTime.Parse(v2.ToString()).Date;
                identical = op1 == op2;
            }
            else
            {
                throw new NotImplementedException(String.Format("Для типа {0} не предусмотрена операция сравнения", type.ToString()));
            }

            return identical;
        }
        #endregion
    }
}
