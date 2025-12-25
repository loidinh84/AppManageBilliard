using AppManageBida.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppManageBilliard.DAL
{
    public class AccountDAL
    {
        private static AccountDAL instance;

        public static AccountDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAL();
                }
                return AccountDAL.instance;
            }
            private set { AccountDAL.instance = value; }
        }
        private AccountDAL() { }

        public bool Login(string userName, string passWord)
        {
            string query = "SELECT * FROM Account WHERE UserName = @userName AND PassWord = @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });

            return result.Rows.Count > 0;
        }
    }
}
