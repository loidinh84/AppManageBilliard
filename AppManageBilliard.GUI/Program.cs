using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppManageBilliard.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.IsRemember)
            {
                string user = Properties.Settings.Default.UserName;
                Account loginAccount = AccountDAL.Instance.GetAccountByUserName(user);

                Application.Run(new fTableManager(loginAccount));
            }
            else
            {
                Application.Run(new fLogin());
            }
        }
    }
}
