using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AppManageBilliard.BUS; 
using AppManageBilliard.DTO;
using AppManageBilliard.DAL;

namespace AppManageBilliard.UserControls
{
    public partial class ucMenu : UserControl
    {
        // Sự kiện tự định nghĩa: Khi chọn món thì báo ra ngoài (cho MainWindow biết)
        public event EventHandler<Food> OnFoodSelected;

        public ucMenu()
        {
            InitializeComponent();
            LoadCategories(); // Load danh mục ngay khi mở
        }

        // 1. Tải danh sách Danh mục
        void LoadCategories()
        {
            pnlCategories.Children.Clear();
            List<Category> listCategory = CategoryDAL.Instance.GetListCategory(); // Có thể đổi thành CategoryBUS nếu bạn đã viết BUS

            // Thêm nút "Tất cả" đầu tiên
            Button btnAll = CreateCategoryButton(new Category(-1, "Tất cả"));
            btnAll.Tag = "Selected"; // Đánh dấu đang chọn
            btnAll.Click += Category_Click;
            pnlCategories.Children.Add(btnAll);

            // Thêm các danh mục từ DB
            foreach (Category item in listCategory)
            {
                Button btn = CreateCategoryButton(item);
                btn.Click += Category_Click;
                pnlCategories.Children.Add(btn);
            }

            // Mặc định load tất cả món
            LoadFoodList(-1);
        }

        // Hàm phụ tạo nút Danh mục cho gọn code
        Button CreateCategoryButton(Category cat)
        {
            Button btn = new Button();
            btn.Content = cat.Name;
            btn.DataContext = cat; // Lưu object Category vào DataContext để lấy ID sau này
            btn.Style = (Style)this.Resources["CategoryButtonStyle"];
            return btn;
        }

        // Sự kiện khi bấm chọn Danh mục
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            Button btnClicked = sender as Button;
            Category cat = btnClicked.DataContext as Category;

            // Đổi màu nút đang chọn (Visual Effect)
            foreach (Button btn in pnlCategories.Children)
            {
                btn.Tag = ""; // Reset màu
                if (btn == btnClicked) btn.Tag = "Selected"; // Highlight nút này
            }

            // Load món ăn theo ID danh mục
            LoadFoodList(cat.ID);
        }

        // 2. Tải danh sách Món ăn
        void LoadFoodList(int categoryID)
        {
            wpFoodList.Children.Clear();
            List<Food> listFood;

            if (categoryID == -1) // -1 quy ước là lấy tất cả
            {
                listFood = FoodBUS.Instance.GetListFood(); // Bạn cần đảm bảo FoodBUS có hàm GetListFood() (lấy all)
                // Nếu FoodBUS chưa có hàm lấy All, bạn dùng tạm FoodDAL.Instance.GetListFood()
            }
            else
            {
                listFood = FoodBUS.Instance.GetFoodByCategoryID(categoryID);
            }

            foreach (Food item in listFood)
            {
                Button btn = new Button();

                // Binding dữ liệu vào Button (để XAML hiển thị Name và Price)
                btn.DataContext = item;

                // Áp dụng Style thẻ món ăn đã viết ở XAML
                btn.Style = (Style)this.Resources["FoodButtonStyle"];

                // Sự kiện khi bấm vào món -> Gọi món
                btn.Click += Food_Click;

                wpFoodList.Children.Add(btn);
            }
        }

        // Sự kiện khi bấm vào một Món ăn
        private void Food_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Food selectedFood = btn.DataContext as Food;

            // Bắn tín hiệu ra ngoài MainWindow
            OnFoodSelected?.Invoke(this, selectedFood);
        }

        // 3. Tìm kiếm món ăn
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();

            // Cách đơn giản: Duyệt qua các nút đang hiển thị để ẩn/hiện
            foreach (Button btn in wpFoodList.Children)
            {
                Food food = btn.DataContext as Food;
                if (food.Name.ToLower().Contains(keyword))
                {
                    btn.Visibility = Visibility.Visible;
                }
                else
                {
                    btn.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}