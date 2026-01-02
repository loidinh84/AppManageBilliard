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
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                var tables = TableBUS.Instance.LoadTableList();
                icTableList.ItemsSource = new ObservableCollection<AppManageBilliard.DTO.Table>(tables);
            }
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var table = button.Tag as AppManageBilliard.DTO.Table;

            if (table != null)
            {
                MessageBox.Show($"Bạn đã chọn {table.Name} - Trạng thái: {table.Status}");
            }
        }
    }
}