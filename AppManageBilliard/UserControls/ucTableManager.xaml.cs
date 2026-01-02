using AppManageBilliard.BUS;
using AppManageBilliard.DTO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AppManageBilliard.UserControls
{
    public partial class ucTableManager : UserControl
    {
        public ucTableManager()
        {
            InitializeComponent();
            LoadTableList();
        }

        void LoadTableList()
        {
            var mockData = new ObservableCollection<AppManageBilliard.DTO.Table>();
            mockData.Add(new AppManageBilliard.DTO.Table { Name = "Bàn 01", Status = "Trống" });

            icTableList.ItemsSource = mockData;
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var table = button.Tag as AppManageBilliard.DTO.Table;

            if (table != null)
            {
                // Bạn chỉ cần xử lý logic chọn bàn tại đây
                // Ví dụ: Đổi màu viền bàn đang chọn (mình sẽ hỗ trợ nếu bạn cần)
            }
        }
    }
}