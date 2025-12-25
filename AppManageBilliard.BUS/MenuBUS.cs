using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppManageBilliard.BUS
{
    public class MenuBUS
    {
        private static MenuBUS instance;
        public static MenuBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuBUS();
                }
                return MenuBUS.instance;
            }
            private set { MenuBUS.instance = value; }
        }
        private MenuBUS() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            return MenuDAL.Instance.GetListMenuByTable(id);
        }
    }
}
