using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AppManageBilliard.UserControls
{
    public partial class ucBill : UserControl
    {
        // Model cho mỗi dòng trong hóa đơn
        public class BillItem
        {
            public string Name { get; set; } = "";
            public int Quantity { get; set; } = 1;
            public decimal Price { get; set; } = 0;

            public decimal Total => Quantity * Price;
            public string TotalPrice => Total.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
        }

        // Danh sách món - bind trực tiếp vào DataGrid
        public ObservableCollection<BillItem> BillItems { get; set; } = new ObservableCollection<BillItem>();

        // Các giá trị tính toán
        private decimal totalHour = 0;        // Tổng tiền giờ (từ bàn)
        private decimal discount = 0;          // Giảm giá nhập tay
        private decimal customerPaid = 0;     // Tiền khách đưa

        public ucBill()
        {
            InitializeComponent();

            // Bind DataContext để XAML truy cập BillItems và các TextBlock
            DataContext = this;

            // KHÔNG có dữ liệu mẫu nữa - bạn sẽ load từ SQL ở đây
            // LoadDataFromSQL(); // <--- Bạn sẽ gọi hàm này khi mở bill bàn nào đó
        }

        // TODO: Hàm bạn sẽ viết để load dữ liệu từ SQL khi mở bàn
        // private void LoadDataFromSQL(int tableId)
        // {
        //     // Ví dụ:
        //     txtTableInfo.Text = "Bàn 05 - Bàn lỗ VIP";
        //     txtStartTime.Text = "18:30";
        //     txtEndTime.Text = "21:45 (hiện tại)";
        //     txtPlayTime.Text = "3 giờ 15 phút";
        //     txtRate.Text = "80.000 đ/giờ";
        //     totalHour = 240000;
        //
        //     // Load món từ database vào BillItems
        //     BillItems.Clear();
        //     // BillItems.Add(... từ SQL);
        //
        //     UpdateAllCalculations();
        // }

        // Cập nhật tất cả tổng tiền
        private void UpdateAllCalculations()
        {
            decimal totalService = 0;
            foreach (var item in BillItems)
            {
                totalService += item.Total;
            }

            decimal grandTotal = totalHour + totalService - discount;
            decimal change = customerPaid - grandTotal;

            // Cập nhật lên giao diện
            txtHourTotal.Text = totalHour.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            txtServiceTotal.Text = totalService.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            txtGrandTotal.Text = grandTotal.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            txtChange.Text = change >= 0
                ? change.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ"
                : "0 đ";
        }

        // Khi người dùng nhập giảm giá
        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(txtDiscount.Text.Replace(".", "").Replace(",", ""), out decimal value))
                discount = value;
            else
                discount = 0;

            UpdateAllCalculations();
        }

        // Khi người dùng nhập tiền khách đưa
        private void txtCustomerPaid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(txtCustomerPaid.Text.Replace(".", "").Replace(",", ""), out decimal value))
                customerPaid = value;
            else
                customerPaid = 0;

            UpdateAllCalculations();
        }

        // Nút + Thêm món (bạn sẽ mở form chọn món từ menu SQL)
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở form chọn món từ database
            MessageBox.Show("Chức năng thêm món sẽ mở form chọn từ menu", "Thông báo");

            // Ví dụ tạm thêm 1 món để test
            // BillItems.Add(new BillItem { Name = "Nước suối", Quantity = 1, Price = 10000 });
            // UpdateAllCalculations();
        }

        // Nút Thanh toán & In hóa đơn
        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            if (customerPaid < (totalHour + GetServiceTotal() - discount))
            {
                MessageBox.Show("Tiền khách đưa chưa đủ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Lưu hóa đơn vào SQL, in bill, reset bàn
            MessageBox.Show(
                $"Thanh toán thành công!\nTổng tiền: {txtGrandTotal.Text}\nTiền thừa: {txtChange.Text}",
                "Hoàn tất",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Reset bill sau khi thanh toán (nếu cần)
            // ClearBill();
        }

        // Nút In bill tạm
        private void btnPrintTemp_Click(object sender, RoutedEventArgs e)
        {
            // TODO: In bill tạm (không lưu database)
            MessageBox.Show("Đã in bill tạm!", "In bill tạm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Nút Hủy bàn
        private void btnCancelTable_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn hủy bàn này? Dữ liệu sẽ mất.", "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: Hủy bàn trong database
                BillItems.Clear();
                totalHour = 0;
                discount = 0;
                customerPaid = 0;
                UpdateAllCalculations();

                MessageBox.Show("Đã hủy bàn thành công!", "Hủy bàn", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Hàm phụ tính tổng dịch vụ
        private decimal GetServiceTotal()
        {
            decimal total = 0;
            foreach (var item in BillItems)
                total += item.Total;
            return total;
        }

        // TODO: Bạn sẽ gọi hàm này khi nhận dữ liệu tiền giờ từ timer hoặc bàn
        public void SetHourAmount(decimal amount)
        {
            totalHour = amount;
            UpdateAllCalculations();
        }
    }
}