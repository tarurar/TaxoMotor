namespace WebClientGIBDD
{
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Core.Objects;

    public partial class TmDataModelContainer : ObjectContext
    {
        public TmDataModelContainer(string connectionString)
            : base((new EntityConnectionStringBuilder
                {
                    Provider = "System.Data.SqlClient",
                    ProviderConnectionString = connectionString,
                    Metadata =
                        @"res://*/TmDataModel.csdl|res://*/TmDataModel.ssdl|res://*/TmDataModel.msl"
                }).ToString(), "TmDataModelContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();

            //this.Configuration.ValidateOnSaveEnabled = validationMode.Equals(DbValidationMode.Enabled);
            //this.Configuration.LazyLoadingEnabled = lazyLoadingMode.Equals(DbLazyLoadingMode.Enabled);
            //this.Configuration.AutoDetectChangesEnabled = autoDetectMode.Equals(DbAutoDetectMode.Enabled);
        }
    }
}