using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.DatabaseModel
{
    public static class SqlErrorCode
    {
        public static int DuplicateKey = 2627;
    }

    public static class SqlErrorTemplate
    {
        public static string DuplicateKey =
            "Violation of (?<ConstraintType>.*) constraint '(?<ConstraintName>.*)'. Cannot insert duplicate key in object '(?<ObjectName>.*)'. The duplicate key value is \\((?<DuplicateValue>.*)\\).";
    }
}
