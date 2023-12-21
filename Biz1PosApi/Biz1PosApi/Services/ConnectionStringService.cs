using Biz1PosApi.Models;

namespace Biz1PosApi.Services
{
    public class ConnectionStringService
    {
        public string getConnString(int companyid)
        {
            string conn = "";
            switch (companyid)
            {
                case 3:
                    conn = "myconn";
                    break;
                case 4:
                    conn = "myconn";
                    break;
                case 5:
                    conn = "myconn";
                    break;
                default:
                    conn = "myconn";
                    break;
            }

            return conn;
        }
    }
}
