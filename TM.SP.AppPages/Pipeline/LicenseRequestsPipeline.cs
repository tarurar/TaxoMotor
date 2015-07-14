using System;
using Microsoft.SharePoint;
using TM.SP.BCSModels.Taxi;
using TM.Utils;

namespace TM.SP.AppPages.Pipeline
{
    /// <summary>
    /// Конвейер поставки разрешений для автоматической отправки запросов по ним
    /// Инкапсулирована логика выборки таких разрешений
    /// Реализация предполагает, что нет опасности при последовательном и сильно 
    /// разделенном во времени вызове методов GetNext и PutBack для одного элемента, 
    /// поскольку даже в случае появления у разрешения новых наследников в этот 
    /// период времени, они (наследники) попадут в конвейер довольно скоро, что не
    /// рассматривается как ошибка
    /// </summary>
    public class LicenseRequestsPipeline: LicenseBasePipeline
    {
        private readonly int _days;

        public LicenseRequestsPipeline(SPWeb web)
        {
            var daysStr = Config.GetConfigValueOrDefault<string>(web, "LicenseAutoSender.CycleInDays");
            _days = Convert.ToInt32(daysStr);
        }
        public override License GetNext()
        {
            return LicenseHelper.GetLicenseRequestToSend(_days);
        }

        public override void PutBack(License element)
        {
            element.LastRequestSendDate = DateTime.Now.Date;
            LicenseHelper.UpdateLicense(element);
        }
    }
}
