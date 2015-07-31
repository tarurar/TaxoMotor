using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.ServiceModel;
using WebClientGIBDD;
using WebClientGIBDD.ModerationJournalQueryService;
using WebClientGIBDD.SpecialTransportDataReceiverService;

namespace WebClientGIBDD
{
    public class WebServiceClient
    {
        /// <summary>
        /// Клиент для сервиса ГИБДД
        /// </summary>
        /// <param name="applicationName">адреса от кого и кому, выполняемое действие;</param>
        /// <param name="specialTransportURL">- адрес сервиса отправки сведений из реестра разрешений в регистр спецтранспорта</param>
        /// <param name="moderationJournalURL">- адрес сервиса получения сведений о ТС, исключенных из регистра специального транспорта</param>
        /// <param name="dbConnectionString">- параметры подключения к БД (DB host, DB name, логин, пароль)</param>
        public WebServiceClient(string applicationName, string specialTransportURL, string moderationJournalURL, string dbConnectionString)
        {
            ApplicationName = applicationName;
            SpecialTransportURL = specialTransportURL;
            ModerationJournalURL = moderationJournalURL;
            DBConnectionString = dbConnectionString;
            //на случай отсутствия необходимой библиотеки EF.SqlServer.dll 
            //http://stackoverflow.com/questions/14033193/entity-framework-provider-type-could-not-be-loaded
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public string DBConnectionString { get; private set; }
        public string ModerationJournalURL { get; private set; }
        public string SpecialTransportURL { get; private set; }
        public string ApplicationName { get; private set; }

        public void putDataPackages()
        {
            using (var srv = new SpecialTransportDataReceiverServiceClient(new BasicHttpBinding(), new EndpointAddress(SpecialTransportURL)))
            {
                Guid? recordId = null;
                srv.ChannelFactory.Endpoint.Behaviors.Add(new SoapServiceBehavior((sender, writeData) =>
                    {
                        WriteToAppLog(ApplicationName, writeData.Direction, writeData.Message, recordId);
                    }));

                using (var tmData = new TmDataModelContainer(DBConnectionString))
                {
                    var licenseids = tmData.ExecuteStoreQuery<int>(
                        @"      create table #tt(id uniqueidentifier,licenseid int, recordid decimal(18,0))
                                 
                                insert into #tt(id, licenseid, recordId)
                                select NEWID(), l1.Id, l1.RootParent
                                from License l1
                                    left join SpecialVehiclesRegister svr on l1.id = svr.License_id
                                    left join license l2 on l2.Parent = l1.id and l2.Status<4
                                where svr.License_id is null and l1.status < 4 and l2.id is null;

                                insert into [SpecialVehiclesRegister](ID, [License_id], [RecordId])
                                select id,licenseid,recordid from #tt
                                
                                select licenseid
                                from #tt

                                drop table #tt
                                ").ToList();

                    
                    var minDate = new DateTime(2011, 9, 1);

                    var result = new List<DataPackageInfoType>();

                    int count = licenseids.Count;
                    for (int i = 0; i < count; i += 1000)
                    {
                        var nextIds = licenseids.Take(1000).ToList();
                        nextIds.ForEach(x=>licenseids.Remove(x));
                        var newSvrs = (from tm in tmData.License
                                       join svr in tmData.SpecialVehiclesRegister on tm.Id equals svr.License_id
                                       where nextIds.Contains(tm.Id) && tm.Status < 4
                                       select new { License = tm, SpecialVehReg = svr}).ToList();
                        foreach (var newSvr in newSvrs)
                        {
                            recordId = Guid.NewGuid();
                            try
                            {
                                var package = GetDataPackage(newSvr.License, newSvr.SpecialVehReg, minDate, recordId);
                                newSvr.SpecialVehReg.PackageId = recordId.ToString();
                                result.AddRange(srv.putDataPackages(package));
                            }
                            catch (Exception ex)
                            {
                                newSvr.SpecialVehReg.Exception = ex.Message;
                            }
                        }

                        UpdateSpecialTransportRegister(result, newSvrs.Select(x => x.SpecialVehReg).ToList(), tmData);
                    }
                }
            }
        }


        private static DataPackagesType GetDataPackage(License l, SpecialVehiclesRegister svr, DateTime minDate, Guid? packageId)
        {
            return new DataPackagesType
                {
                    size = 1,
                    owner = l.TaxiColor == "Желтый" && l.TaxiNumberColor == "1"
                                ? "TAXOMOTOR_YLWTX"
                                : "TAXOMOTOR_LICTX",
                    DataPackage = new[]
                        {
                            new DataPackageType()
                                {
                                    PackageId = packageId.ToString(),
                                    PackageCreatedDate = DateTime.Now,
                                    Records = new RecordsType
                                        {
                                            size = 1,
                                            Record = new[]
                                                {
                                                    new RecordType
                                                        {
                                                            RecordId = svr.RecordId.ToString(),
                                                            CatalogNumber = l.RegNumber.PadLeft(5,'0'),
                                                            AgreementDate =
                                                                l.CreationDate < minDate ||
                                                                l.CreationDate == null
                                                                    ? minDate
                                                                    : l.CreationDate.Value,
                                                            PermitStartDate = l.CreationDate < minDate ||
                                                                              l.CreationDate == null
                                                                                  ? minDate
                                                                                  : l.CreationDate.Value,
                                                            PermitEndDate =
                                                                l.Status < 2 && !(l.DisableGibddSend ?? false)
                                                                    ? l.TillDate.Value
                                                                    : new[]
                                                                        {l.OutputDate.Value, DateTime.Now.AddDays(-1)}
                                                                          .Min(),
                                                            CategoryCode =
                                                                l.TaxiColor == "Желтый" && l.TaxiNumberColor == "1"
                                                                    ? 18
                                                                    : 17,
                                                            RestrictionAreaCode = 0,
                                                            CauseInclusion =
                                                                "Выгрузка из реестра разрешений АИС Таксомотор",
                                                            Organization = new OrganizationType
                                                                {
                                                                    OrganizationTitle = l.ShortName,
                                                                    OrganizationAddress = l.JuridicalAddress,
                                                                    OrganizationPhone = l.PhoneNumber
                                                                },
                                                            Vehicle = new VehicleType
                                                                {
                                                                    VehicleRegNumber = l.TaxiStateNumber,
                                                                    VehicleType1 = "Легковое такси",
                                                                    VehicleBrand = l.TaxiBrand,
                                                                    VehicleModel = l.TaxiModel
                                                                }

                                                        }
                                                }
                                        }
                                }
                        }
                };
        }

        private static void UpdateSpecialTransportRegister(List<DataPackageInfoType> result, List<SpecialVehiclesRegister> newSvrs, TmDataModelContainer tmData)
        {
            foreach (var res in result)
            {
                var recordId = res.PackageId;
                var svr = newSvrs.FirstOrDefault(x => x.PackageId == recordId);
                if (svr == null)
                    continue;

                svr.ProccessingStageCode = res.ProccessingStageCode.ToString();
                svr.ProcessingStageDate = res.ProcessingStageDate;
                svr.ProcessingStageInfo = res.ProcessingStageInfo;
                svr.PackageId = res.PackageId;
                if (res.RecordsStatuses != null && res.RecordsStatuses.RecordStatus != null)
                {
                    var rec = (svr.RecordId).ToString();
                    var stat = res.RecordsStatuses.RecordStatus.FirstOrDefault(x => x.RecordId == rec);
                    if (stat != null)
                    {
                        svr.RecordStatusCode = stat.RecordStatusCode.ToString();
                        svr.RecordStatusText = stat.RecordStatusText;
                    }
                }
            }
            tmData.SaveChanges();
        }

        public void getDataPackagesInfo()
        {
            // последовательно опрашивать статусы по всем записям таблицы SpecialVehiclesRegister для которых PackageId in not null и 
            // ProccessingStageCode не равен одному из следующих значений: NOTEXIST, LOADED, FAILED

            using (var srv = new SpecialTransportDataReceiverServiceClient(new BasicHttpBinding(), new EndpointAddress(SpecialTransportURL)))
            {
                Guid? packageId = null;
                srv.ChannelFactory.Endpoint.Behaviors.Add(new SoapServiceBehavior((sender, writeData) =>
                    {
                        WriteToAppLog(ApplicationName, writeData.Direction, writeData.Message, packageId);
                    }));

                using (var tmData = new TmDataModelContainer(DBConnectionString))
                {
                    var svRegister = from svr in tmData.SpecialVehiclesRegister
                                     join l in tmData.License on svr.License_id equals l.Id
                                     where
                                         svr.PackageId != null && svr.ProccessingStageCode != "LOADED" &&
                                         svr.ProccessingStageCode != "FAILED" && svr.ProccessingStageCode != "NOTEXIST"
                                     select new {Svr = svr, License = l};

                    var result = new List<DataPackageInfoType>();
                    foreach (var svr in svRegister)
                    {
                        packageId = new Guid(svr.Svr.PackageId);

                        bool error = false;
                        IEnumerable<DataPackageInfoType> res = null;
                        try
                        {
                            res = srv.getDataPackagesInfo(new DataPackagesInfoRequestType
                                {
                                    owner = svr.License.TaxiColor == "Желтый" && svr.License.TaxiNumberColor == "1"
                                                ? "TAXOMOTOR_YLWTX"
                                                : "TAXOMOTOR_LICTX",
                                    PackageId = new[] {svr.Svr.PackageId}
                                });
                        }
                        catch
                        {
                            error = true;
                        }
                        if (!error && res != null)
                            result.AddRange(res);
                    }

                    UpdateSpecialTransportRegister(result, svRegister.Select(x => x.Svr).ToList(), tmData);
                }
            }
        }

        /// <summary>
        /// получить сообщение, содержащее массив сведений об аннулированных разрешениях 
        /// </summary>
        public void getCancelledLicenses(DateTime? cancelStartDate = null, DateTime? cancelEndDate = null)
        {
            using (var mjClident = new ModerationJournalQueryServiceClient(new BasicHttpBinding(), new EndpointAddress(ModerationJournalURL)))
            {
                Guid? messageId = null;
                mjClident.ChannelFactory.Endpoint.Behaviors.Add(new SoapServiceBehavior((sender, writeData) =>
                {
                    WriteToAppLog(ApplicationName, writeData.Direction, writeData.Message, messageId);
                }));

                using (var tmData = new TmDataModelContainer(DBConnectionString))
                {
                    var allCancelled = new List<ResponseLicense>();
                    foreach (var own in new[] {"TAXOMOTOR_LICTX", "TAXOMOTOR_YLWTX"})
                    {
                        bool hasmorrecords = true;
                        int page = 1; // должен начинаться с 1
                        do
                        {
                            messageId = Guid.NewGuid();
                            ResponseLicense[] rl = null;
                            bool error = false;
                            try
                            {
                                string statusComment;
                                mjClident.getCancelledLicenses(new CancelledLicensesFilter()
                                    {
                                        owner = own,
                                        cancelStartDate = cancelStartDate ?? DateTime.Now.AddDays(-7),
                                        cancelStartDateSpecified = true,
                                        cancelEndDate = cancelEndDate ?? DateTime.Now,
                                        cancelEndDateSpecified = true,
                                    }, (page++).ToString(), out statusComment, out hasmorrecords, out rl);
                            }
                            catch(Exception ex)
                            {
                                var err = tmData.GibddServiceErrors.CreateObject();
                                err.ErrorDate = DateTime.Now;
                                err.Exception = ex.Message;
                                tmData.GibddServiceErrors.AddObject(err);
                                tmData.SaveChanges();

                                error = true;
                            }
                            if (!error && rl != null)
                                allCancelled.AddRange(rl);

                        } while (hasmorrecords);
                    }

                    foreach (var rl in allCancelled)
                    {
                        if (tmData.ExcludeVehicles.Any(x => x.licencePlateNumber == rl.licencePlateNumber && x.cancelDate == rl.cancelDate))
                            continue;

                        var exv = tmData.ExcludeVehicles.CreateObject();
                        exv.ID = Guid.NewGuid();
                        exv.owner = rl.owner;
                        exv.catalogNumber = rl.catalogNumber;
                        exv.licencePlateNumber = rl.licencePlateNumber;
                        exv.uploadDate = rl.uploadDate;
                        exv.cancelDate = rl.cancelDate;
                        exv.cancelComment = rl.cancelComment;
                        exv.licenseId = rl.licenseId;
                        tmData.ExcludeVehicles.AddObject(exv);
                    }
                    tmData.SaveChanges();
                }
            }
        }

        public enum Direction
        {
            Send,
            Receive
        }

        /// <summary>
        /// запись в таблицу AppLog
        /// </summary>
        public void WriteToAppLog(string applicationName, Direction direction, string message, Guid? messageId)
        {
            using (var tmData = new TmDataModelContainer(DBConnectionString))
            {
                tmData.AppLogInsert(
                    applicationName,
                    direction == Direction.Send ? "S" : "R",
                    message,
                    messageId);

            }
        }

        
    }


}
