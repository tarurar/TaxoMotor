using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.Ratings.Reports
{
    public class SqlReport : BaseReport
    {
        private string _connectionString;
        public SqlReport(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
