using AppManageBilliard.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppManageBilliard.BUS
{
    public class BillBUS
    {
        private static BillBUS instance;
        public static BillBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillBUS();
                }
                return BillBUS.instance;
            }
            private set { BillBUS.instance = value; }
        }
        private BillBUS() { }
        public int GetUncheckBillID(int id)
        {
            return BillDAL.Instance.GetUncheckBillIDByTableID(id);
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            BillDAL.Instance.InsertBillInfo(idBill, idFood, count);
        }
        public void InsertBill(int idTable)
        {
            BillDAL.Instance.InsertBill(idTable);
        }
        public void CheckOut(int id, int discount)
        {
            BillDAL.Instance.CheckOut(id, discount);
        }
    }
}
