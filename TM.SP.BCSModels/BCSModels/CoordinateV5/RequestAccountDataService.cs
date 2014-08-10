using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

using TM.SP.BCSModels.CoordinateV5;

namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class RequestAccountDataService : CoordinateV5Service
    {
        public void GetItemsByIdList(string RequestIdList, out RequestAccountData returnParameter)
        {
            SqlConnection thisConn = null;

            thisConn = getSqlConnection();
            thisConn.Open();

            RequestAccountData res = new RequestAccountData()
            {
                DeclarantAccountFullName = "1",
                DeclarantAccountId = "1",
                Id = 1,
                Title = RequestIdList
            };

            returnParameter = res;
        }
    }
}
