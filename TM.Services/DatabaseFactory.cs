using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TM.Services
{

    public sealed class DatabaseFactorySectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string) base["Name"]; }
        }

        [ConfigurationProperty("ConnectionStringName")]
        public string ConnectionStringName
        {
            get { return (string) base["ConnectionStringName"]; }
        }

        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Connection string {0} was not found in web.config. ExDetails: {1}", ConnectionStringName, ex.Message));
                }
            }
        }
    }

    public sealed class DatabaseFactory
    {
        private static readonly string SectionName = "databaseFactory.config";

        private static DatabaseFactorySectionHandler sectionHandler =
            (DatabaseFactorySectionHandler)ConfigurationManager.GetSection(SectionName);
        private DatabaseFactory() { }

        public static SqlConnection CreateConnection()
        {
            if (sectionHandler == null)
            {
                throw new Exception(String.Format("There is no {0} section in web.config", SectionName));
            }

            return new SqlConnection(sectionHandler.ConnectionString);
        }
    }
}