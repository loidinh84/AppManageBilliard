using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppManageBilliard.DAL;

namespace AppManageBilliard.BUS
{
    public class TableBUS
    {
        private static TableBUS instance;
        public static TableBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TableBUS();
                }
                return TableBUS.instance;
            }
            private set { TableBUS.instance = value; }
        }
        private TableBUS() { }

        public List<Table> LoadTableList()
        {
            return TableDAL.Instance.LoadTableList();

        }
        public void SwitchTable(int id1, int id2)
        {
            TableDAL.Instance.SwitchTable(id1, id2);
        }
    }
}
