using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using TM.Utils;
using TM.SP.Ratings.Cache;
using TM.SP.Ratings.Helpers;

namespace TM.SP.Ratings.Timers
{
    public interface IRatingReport
    {
        string GetName();
        string GetTitle();
        Guid GetGuid();
    }
    public class RatingBaseJobDefinition : SPJobDefinition, IRatingReport
    {
        #region [interfaces]

        public virtual string GetName()
        {
            throw new NotImplementedException();
        }
        public virtual string GetTitle()
        {
            throw new NotImplementedException();
        }
        public virtual Guid GetGuid()
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region [resource strings]

        private static readonly string FeatureId = "{5ba9a0a0-323d-4997-8105-4f27fae6e732}";
        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        #endregion

        #region [methods]

        protected static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }

        public RatingBaseJobDefinition() : base() {}

        public RatingBaseJobDefinition(string jobName, string rsJobTitle, SPService service)
            : base(jobName, service, null, SPJobLockType.None)
        {
            Title = rsJobTitle;
        }

        public RatingBaseJobDefinition(string jobName, string rsJobTitle, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = rsJobTitle;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                if (webApp != null)
                {
                    foreach (SPSite siteCollection in webApp.Sites)
                    {
                        SPWeb web = siteCollection.RootWeb;

                        if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                            web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                        {
                            DataTable data = WebExecuteJob(web);
                            ICacher cacher = new Cacher();
                            cacher.Dump(data, GetGuid(), SqlHelper.GetConnectionString(web));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(GetFeatureLocalizedResource("GeneralErrorFmt"), Title, ex.Message));
            }
        }

        protected virtual DataTable WebExecuteJob(SPWeb web)
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            var webApp = Parent as SPWebApplication;
            if (webApp != null)
            {
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;

                    if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                        web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                    {
                        DoRegister(web);
                    }
                }
            }
        }

        protected virtual void DoRegister(SPWeb web)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(SqlHelper.GetConnectionString(web));
                conn.Open();

                SqlCommand insertCmd = new SqlCommand
                {
                    CommandText = @"SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
                                    BEGIN TRANSACTION
                                        DECLARE @id AS INT
                                        SELECT @id = [Id] FROM [dbo].[Report] WHERE [Guid] = @Guid
                                        IF @id IS NULL
                                        BEGIN
		                                    INSERT INTO [dbo].[Report] ([Title], [Guid], [Maintained]) VALUES (@Title, @Guid, @Maintained)
		                                    SELECT @id = SCOPE_IDENTITY()
                                        END
	                                    ELSE
	                                    BEGIN
		                                    UPDATE [dbo].[Report] SET [Maintained] = @Maintained WHERE [Id] = @id
	                                    END
                                        SELECT @id
                                    COMMIT TRANSACTION",
                    Connection = conn
                };

                insertCmd.Parameters.AddWithValue("@Title", GetTitle());
                insertCmd.Parameters.AddWithValue("@Guid", GetGuid());
                insertCmd.Parameters.AddWithValue("@Maintained", 1);

                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Dispose();
            }
        }

        public virtual void UnRegister()
        {
            var webApp = Parent as SPWebApplication;
            if (webApp != null)
            {
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;

                    if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                        web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                    {
                        DoUnRegister(web);
                    }
                }
            }
        }

        protected virtual void DoUnRegister(SPWeb web)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(SqlHelper.GetConnectionString(web));
                conn.Open();

                SqlCommand insertCmd = new SqlCommand
                {
                    CommandText = @"SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
                                    BEGIN TRANSACTION
                                        DECLARE @id AS INT
                                        SELECT @id = [Id] FROM [dbo].[Report] WHERE [Guid] = @Guid
                                        IF @id IS NOT NULL
                                        BEGIN
                                            UPDATE [dbo].[Report] SET [Maintained] = @Maintained WHERE [Id] = @id
                                        END
                                    COMMIT TRANSACTION",
                    Connection = conn
                };

                insertCmd.Parameters.AddWithValue("@Title", GetTitle());
                insertCmd.Parameters.AddWithValue("@Guid", GetGuid());
                insertCmd.Parameters.AddWithValue("@Maintained", 0);

                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Dispose();
            }
        }

        #endregion
    }
}
