using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
namespace TM.SP.BCSModels.CoordinateV5
{
    public partial class FileEntityService
    {
        public static Stream ReadFileItemContent(int Id_Auto)
        {
            using (SqlConnection conn = getSqlConnection())
            using (SqlCommand cm = conn.CreateCommand())
            {
                cm.CommandText = @"SELECT [FileContent] FROM [File] WHERE [Id_Auto] = @Id_Auto";
                cm.Parameters.AddWithValue("@Id_Auto", Id_Auto);
                conn.Open();

                var content = cm.ExecuteScalar() as Byte[];
                if (content == null)
                {
                    throw new Exception(String.Format("There is no binary content for record with Id={0} in table {1}",
                        Id_Auto, "[File]"));
                }

                return new MemoryStream(content);
            }
        }
    }
}
