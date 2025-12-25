using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppManageBilliard.BUS
{
    public class FoodBUS
    {
        private static FoodBUS instance;
        public static FoodBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FoodBUS();
                }
                return FoodBUS.instance;
            }
            private set { FoodBUS.instance = value; }
        }
        private FoodBUS() { }
        public List<Food> GetFoodByCategoryID(int id)
        {
            return FoodDAL.Instance.GetFoodByCategoryID(id);
        }
    }
}
