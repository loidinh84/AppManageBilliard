using AppManageBida.DAL;
using AppManageBilliard.DTO;
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
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("EXEC USP_InsertAccount @userName = N'{0}', @displayName = N'{1}', @type = {2}", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("EXEC USP_UpdateAccount @userName = N'{0}', @displayName = N'{1}', @type = {2}", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string name)
        {
            string query = string.Format("EXEC USP_DeleteAccount @userName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string name)
        {
            string query = string.Format("EXEC USP_ResetPassword @userName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccountProfile(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccountProfile @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, pass, newPass });
            return result > 0;
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Account WHERE UserName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }
        public int GetTotalAccount()
        {
            string query = "SELECT COUNT(UserName) FROM Account";
            return (int)DataProvider.Instance.ExecuteScalar(query);
        }
    }

}
