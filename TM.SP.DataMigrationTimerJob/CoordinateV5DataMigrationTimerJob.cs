using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TM.SP.DataMigrationTimerJob
{
    public class CoordinateV5DataMigrationTimerJob : SPJobDefinition
    {
        private static readonly string jobTitle = "TaxoMotor CoordinateV5 Data Migration Timer Job";

        public CoordinateV5DataMigrationTimerJob() : base() {}

        public CoordinateV5DataMigrationTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = jobTitle;
        }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPWebApplication webapp): base(jobName, webapp, null, SPJobLockType.ContentDatabase)
        {
            this.Title = jobTitle;
        }

        public override void Execute(Guid targetInstanceId)
        {
            throw new NotImplementedException("Логика обработчика миграции данных еще не написана");
        }


    }
}
