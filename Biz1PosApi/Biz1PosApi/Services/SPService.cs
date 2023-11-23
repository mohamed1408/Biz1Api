using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Biz1PosApi.Services
{
    public class SPService
    {
        private SqlConnection sqlCon;
        private IConfiguration Configuration;
        
        public SPService(string conn, IConfiguration _configuration)
        {
            Configuration = _configuration;
            sqlCon = new SqlConnection(Configuration.GetConnectionString(conn));
        }
        public DataSet exec(string sp, Dictionary<string,dynamic> paramList)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(sp, sqlCon);
            cmd.CommandType = CommandType.Text;

            foreach (var param in paramList)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }
            
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(cmd);
            sqlAdp.Fill(ds);
            sqlCon.Close();
            return ds;
        }
    }
}
