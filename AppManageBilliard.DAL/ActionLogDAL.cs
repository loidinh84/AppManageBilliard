using AppManageBilliard.DAL;
using System;
using System.Data;

namespace AppManageBilliard.DAL
{
    public class ActionLogDAL
    {
        private static ActionLogDAL instance;
        public static ActionLogDAL Instance
        {
            get { if (instance == null) instance = new ActionLogDAL(); return instance; }
            private set { instance = value; }
        }

        private ActionLogDAL() { }

        public void InsertActionLog(string staffName, string actionType, string details)
        {
            string query = "INSERT INTO ActionLog (StaffName, ActionType, Details, ActionTime) VALUES ( @staffName , @actionType , @details , GETDATE() )";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { staffName, actionType, details });
        }
    }
}