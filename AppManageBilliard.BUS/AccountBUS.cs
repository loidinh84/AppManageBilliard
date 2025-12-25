using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;

namespace AppManageBilliard.BUS
{
    public class AccountBUS
    {
        private static AccountBUS instance;
        public static AccountBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountBUS();
                }
                return AccountBUS.instance;
            }
            private set { AccountBUS.instance = value; }
        }
        private AccountBUS() { }

        public bool Login(string userName, string passWord)
        {
            return AccountDAL.Instance.Login(userName, passWord);
        }
    }
}
