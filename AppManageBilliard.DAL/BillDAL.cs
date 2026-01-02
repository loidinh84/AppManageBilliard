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
        public void DeleteBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_DeleteBill @idBill", new object[] { id });
        }
        public int GetCountBillInfo(int idBill)
        {
            string query = "SELECT COUNT(*) FROM dbo.BillInfo WHERE idBill = " + idBill;

            try
            {
                return (int)DataProvider.Instance.ExecuteScalar(query);
            }
            catch
            {
                return 0;
            }
        }
        public DateTime GetDateCheckIn(int idBill)
        {
            string query = "SELECT DateCheckIn FROM dbo.Bill WHERE id = " + idBill;
            try
            {
                return (DateTime)DataProvider.Instance.ExecuteScalar(query);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        public double GetFoodTotalPrice(int idBill)
        {
            string query = "SELECT SUM(f.price * bi.count) FROM dbo.BillInfo AS bi " +
                   "JOIN dbo.Food AS f ON bi.idFood = f.id " +
                   "JOIN dbo.FoodCategory AS fc ON f.idCategory = fc.id " +
                   "WHERE bi.idBill = " + idBill + " AND fc.name != N'Loại Bàn'"; // <--- Khác Loại Bàn
            try
            {
                object result = DataProvider.Instance.ExecuteScalar(query);
                if (result == DBNull.Value) return 0;
                return Convert.ToDouble(result);
            }
            catch { return 0; }
        }
        public double GetTablePriceFromBill(int idBill)
        {
            string query = "SELECT f.price FROM dbo.BillInfo bi " +
                           "JOIN dbo.Food f ON bi.idFood = f.id " +
                           "JOIN dbo.FoodCategory fc ON f.idCategory = fc.id " +
                           "WHERE bi.idBill = " + idBill + " AND fc.name = N'Loại Bàn'";

            try
            {
                object result = DataProvider.Instance.ExecuteScalar(query);
                if (result == null) return 0; // Nếu chưa chọn loại bàn thì giá = 0
                return Convert.ToDouble(result);
            }
            catch { return 0; }
        }
        public Bill GetBillByID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE id = " + id);
            foreach (DataRow item in data.Rows)
            {
                return new Bill(item);
            }
            return null;
        }
    }
}
