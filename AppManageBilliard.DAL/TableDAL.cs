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
    
    public class TableDAL
    {
        public static int TableWidth = 100;
        public static int TableHeight = 100;
        private static TableDAL instance;
        public static TableDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TableDAL();
                }
                return TableDAL.instance;
            }
            private set { TableDAL.instance = value; }
        }
        private TableDAL() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;

        }
        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }
        public bool InsertTable(string name)
        {
            string query = string.Format("EXEC USP_InsertTable @name = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateTable(int id, string name)
        {
            string query = string.Format("EXEC USP_UpdateTable @id = {0}, @name = N'{1}'", id, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE dbo.TableFood WHERE id = {0} AND status = N'Trống'", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public int GetTotalTable()
        {
            string query = "SELECT COUNT(id) FROM TableFood";

            return (int)DataProvider.Instance.ExecuteScalar(query);
        }
        public double GetPriceByTableID(int id)
        {
            string query = "SELECT c.price FROM dbo.TableFood AS t JOIN dbo.TableCategory AS c ON t.idTableCategory = c.id WHERE t.id = " + id;

            try
            {
                object result = DataProvider.Instance.ExecuteScalar(query);
                if (result != null) return Convert.ToDouble(result);
            }
            catch
            {
                return 0;
            }

            return 0;
        }
    }
}
