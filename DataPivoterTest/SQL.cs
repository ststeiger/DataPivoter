using System;

namespace DataPivoterTest
{
    public class SQL
    {
        public SQL()
        {
        }


        public static string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.IntegratedSecurity = true;

            return csb.ConnectionString;
        }
    }
}

