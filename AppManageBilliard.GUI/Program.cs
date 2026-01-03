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
            Application.Run(new fLogin());

            if (AppManageBilliard.GUI.Properties.Settings.Default.IsRemember)
            {
                Account loginAccount = AccountDAL.Instance.GetAccountByUserName(Properties.Settings.Default.UserName);

                Application.Run(new fTableManager(loginAccount));
            }
            else
            {
                Application.Run(new fLogin());
            }
        }
    }
}
