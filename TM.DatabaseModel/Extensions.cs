using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TM.DatabaseModel
{
    public static class IncomingRequestXMLExtensions
    {
        public static bool hasDuplicateMessageError(this IncomingRequestXML obj, SqlException ex)
        {
            Regex re = new Regex(SqlErrorTemplate.DuplicateKey);
            foreach (SqlError err in ex.Errors)
            {
                MatchCollection matchList = re.Matches(err.Message);
                foreach (Match match in matchList)
                {
                    if (match.Groups["ConstraintName"].ToString() == "IncomingRequestXML_UniqueMessageId")
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
