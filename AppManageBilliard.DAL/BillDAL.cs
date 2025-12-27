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
    public class BillDAL
    {   
        private static BillDAL instance;
        public static BillDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillDAL();
                }
                return BillDAL.instance;
            }
            private set { BillDAL.instance = value; }
        }
        private BillDAL() { }
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + id + " AND status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void InsertBill(int idTable)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", new object[] { idTable });
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void CheckOut(int id, int discount)
        {
            string query = "EXEC USP_CheckOut @idBill , @discount";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, discount });
        }
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "EXEC USP_CheckOut @idBill , @discount , @totalPrice";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, discount, totalPrice });
        }
    }
}
