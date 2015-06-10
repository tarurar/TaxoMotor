using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;

namespace WebClientGIBDD
{

    public partial class TmDataModelContainer : DbContext
    {
        public TmDataModelContainer(string connectionString)
            : base((new EntityConnectionStringBuilder
                {
                    Provider = "System.Data.SqlClient",
                    ProviderConnectionString = connectionString,
                    Metadata =
                        @"res://*/TmDataModel.csdl|res://*/TmDataModel.ssdl|res://*/TmDataModel.msl"
                }).ToString())
        {
            //this.Configuration.ValidateOnSaveEnabled = validationMode.Equals(DbValidationMode.Enabled);
            //this.Configuration.LazyLoadingEnabled = lazyLoadingMode.Equals(DbLazyLoadingMode.Enabled);
            //this.Configuration.AutoDetectChangesEnabled = autoDetectMode.Equals(DbAutoDetectMode.Enabled);
        }
    }
}