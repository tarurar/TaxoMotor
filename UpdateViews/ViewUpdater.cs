using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;

namespace ViewUpdater
{
    public class ViewUpdater
    {
        string DBHost;
        string DBName;
        string Login;
        string Password;

        public ViewUpdater(string dbHost, string dbName, string login, string pass)
        {
            DBHost = dbHost;
            DBName = dbName;
            Login = login;
            Password = pass;
        }

        public void UpdateViews()
        {
            using (var sqlCon = new SqlConnection(@"Data Source=" + DBHost.Trim() + ";Initial Catalog=WSS_Content;Persist Security Info=True;User ID=" + Login.Trim() + ";Password=" + Password.Trim()))
            {
                int i = 0;
                String cmd1 = @"SELECT * FROM [WSS_Content].[dbo].[AllLists] where tp_Title not in (N'Состояние перевода', N'Шаблоны форм')";
                DataSet ds = new DataSet("DS");

                sqlCon.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd1, sqlCon);
                da.FillSchema(ds, SchemaType.Source, "cmd1");
                da.Fill(ds, "cmd1");
                if (ds.Tables["cmd1"].Rows.Count > 0)
                {
                    i = ds.Tables["cmd1"].Rows.Count - 1;
                    while (i >= 0)
                    {
                        var row = ds.Tables["cmd1"].Rows[i];
                        CreateView(row["tp_ID"].ToString().Trim(), GetTranslit(row["tp_Title"].ToString().Trim()), sqlCon);
                        i = i - 1;
                    }
                }
                sqlCon.Close();
            }
        }

        private void CreateView(String listId, String viewName, SqlConnection con)
        {
            String s1 = "";
            String s2 = "";
            String cmd = @"SELECT  [tp_Fields]
                                    FROM [WSS_Content].[dbo].[AllLists]
                                    where tp_ID='" + listId.Trim() + "'";

            var zzzz1 = new SqlCommand(cmd, con);
            byte[] x = zzzz1.ExecuteScalar() as byte[];

            //if (List_id == "ee8d45cf-b510-4ba9-9bb6-2005595bd6bf")
            //    MessageBox.Show(getXmlFromTpFields(x).Trim());

            string sel = @"
                        declare @strFiledsSql nvarchar(max); 
                        select @strFiledsSql=''; 
                        declare @xmlFields xml; 
                        select @xmlFields = '" + getXmlFromTpFields(x).Trim().Substring(getXmlFromTpFields(x).Trim().IndexOf("<")) + @"'; 
                        select @strFiledsSql=@strFiledsSql+'ud.'+nref.value('@ColName', 'varchar(30)')  +' as [' + nref.value('@Name', 'varchar(255)') + '], ' +char(10)+char(9)
                                from @xmlFields.nodes('FieldRef,Field') as R(nref)
                                where nref.value('@ColName', 'varchar(30)') is not null;
                        select @strFiledsSql; ";

            zzzz1.CommandText = sel;
            string s = zzzz1.ExecuteScalar() as string;

            // обрезаем последнюю запятую
            s = s.Trim().Substring(0, s.Trim().Length - 1);

            s1 = @"if exists (select * from dbo.sysobjects  where id = object_id(N'dbo.View_" + viewName + @"') and  OBJECTPROPERTY(id, N'IsView') = 1)
              drop view View_" + viewName;
            s2 = @" CREATE VIEW View_" + viewName + @" AS 
                        select ud.tp_id as id1, " + s.Trim() + @" 
                        from WSS_Content.dbo.AllUserData UD
                            inner join (select max(tp_version) as tp_version, tp_id from WSS_Content.dbo.AllUserData where tp_ListId= '" + listId.Trim() + @"' group by tp_id) as vers1 on ud.tp_id=vers1.tp_id and ud.tp_version=vers1.tp_version
                            where UD.tp_ListId= '" + listId.Trim() + "'";

            //SqlConnection SQLCon2 = new SqlConnection(@"Data Source=127.0.0.1\sharepoint;Initial Catalog=TM.Data;Persist Security Info=True;User ID=Reporter;Password=111222");
            using (var SQLCon2 = new SqlConnection(@"Data Source=" + DBHost.Trim() + ";Initial Catalog=" + DBName.Trim() + ";Persist Security Info=True;User ID=" + Login.Trim() + ";Password=" + Password.Trim()))
            {
                SqlCommand myCommand1 = new SqlCommand(s1, SQLCon2);
                SqlCommand myCommand2 = new SqlCommand(s2, SQLCon2);

                myCommand1.Connection.Open();
                myCommand1.ExecuteNonQuery();
                myCommand2.ExecuteNonQuery();
                myCommand1.Connection.Close();
                SQLCon2.Close();
            }
        }

        private static string getXmlFromTpFields(byte[] tpFields)
        {
            using (var memoryStream = new MemoryStream(tpFields))
            {
                // ignore the first 14 bytes; I'm not sure why but it works!
                for (var index = 0; index <= 13; index++)
                    memoryStream.ReadByte();

                var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);

                using (var destination = new MemoryStream())
                {
                    deflateStream.CopyTo(destination);

                    var streamReader = new StreamReader(destination);
                    destination.Position = 0;
                    return streamReader.ReadToEnd();
                }
            }
        }


        private static string GetTranslit(string input_str)
        {
            string str_tr = input_str.Trim();

            str_tr = str_tr.Replace("а", "a");
            str_tr = str_tr.Replace("б", "b");
            str_tr = str_tr.Replace("в", "v");
            str_tr = str_tr.Replace("г", "g");
            str_tr = str_tr.Replace("д", "d");
            str_tr = str_tr.Replace("е", "e");
            str_tr = str_tr.Replace("ё", "yo");
            str_tr = str_tr.Replace("ж", "zh");
            str_tr = str_tr.Replace("з", "z");
            str_tr = str_tr.Replace("и", "i");
            str_tr = str_tr.Replace("й", "j");
            str_tr = str_tr.Replace("к", "k");
            str_tr = str_tr.Replace("л", "l");
            str_tr = str_tr.Replace("м", "m");
            str_tr = str_tr.Replace("н", "n");
            str_tr = str_tr.Replace("о", "o");
            str_tr = str_tr.Replace("п", "p");
            str_tr = str_tr.Replace("р", "r");
            str_tr = str_tr.Replace("с", "s");
            str_tr = str_tr.Replace("т", "t");
            str_tr = str_tr.Replace("у", "u");
            str_tr = str_tr.Replace("ф", "f");
            str_tr = str_tr.Replace("х", "h");
            str_tr = str_tr.Replace("ц", "c");
            str_tr = str_tr.Replace("ч", "ch");
            str_tr = str_tr.Replace("ш", "sh");
            str_tr = str_tr.Replace("щ", "sch");
            str_tr = str_tr.Replace("ъ", "j");
            str_tr = str_tr.Replace("ы", "i");
            str_tr = str_tr.Replace("ь", "j");
            str_tr = str_tr.Replace("э", "e");
            str_tr = str_tr.Replace("ю", "yu");
            str_tr = str_tr.Replace("я", "ya");
            str_tr = str_tr.Replace("А", "A");
            str_tr = str_tr.Replace("Б", "B");
            str_tr = str_tr.Replace("В", "V");
            str_tr = str_tr.Replace("Г", "G");
            str_tr = str_tr.Replace("Д", "D");
            str_tr = str_tr.Replace("Е", "E");
            str_tr = str_tr.Replace("Ё", "Yo");
            str_tr = str_tr.Replace("Ж", "Zh");
            str_tr = str_tr.Replace("З", "Z");
            str_tr = str_tr.Replace("И", "I");
            str_tr = str_tr.Replace("Й", "J");
            str_tr = str_tr.Replace("К", "K");
            str_tr = str_tr.Replace("Л", "L");
            str_tr = str_tr.Replace("М", "M");
            str_tr = str_tr.Replace("Н", "N");
            str_tr = str_tr.Replace("О", "O");
            str_tr = str_tr.Replace("П", "P");
            str_tr = str_tr.Replace("Р", "R");
            str_tr = str_tr.Replace("С", "S");
            str_tr = str_tr.Replace("Т", "T");
            str_tr = str_tr.Replace("У", "U");
            str_tr = str_tr.Replace("Ф", "F");
            str_tr = str_tr.Replace("Х", "H");
            str_tr = str_tr.Replace("Ц", "C");
            str_tr = str_tr.Replace("Ч", "Ch");
            str_tr = str_tr.Replace("Ш", "Sh");
            str_tr = str_tr.Replace("Щ", "Sch");
            str_tr = str_tr.Replace("Ъ", "J");
            str_tr = str_tr.Replace("Ы", "I");
            str_tr = str_tr.Replace("Ь", "J");
            str_tr = str_tr.Replace("Э", "E");
            str_tr = str_tr.Replace("Ю", "Yu");
            str_tr = str_tr.Replace("Я", "Ya");
            str_tr = str_tr.Replace(" ", "_");
            str_tr = str_tr.Replace("-", "_");
            str_tr = str_tr.Replace("(", "");
            str_tr = str_tr.Replace(")", "");
            str_tr = str_tr.Replace(".", "");
            str_tr = str_tr.Replace(",", "");
            str_tr = str_tr.Replace(";", "");
            str_tr = str_tr.Replace("+", "");
            str_tr = str_tr.Replace("%", "");
            str_tr = str_tr.Replace(":", "");

            return str_tr;
        }

    }
}
