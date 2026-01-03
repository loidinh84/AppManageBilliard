using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppManageBilliard.UserControls
{
    public partial class ucBill : UserControl, INotifyPropertyChanged
    {
        // Class model cho món ăn
        public class BillItem : INotifyPropertyChanged
        {
            private int _quantity;
            private decimal _price;

            public string Name { get; set; } = "";
            public int Quantity
            {
                get => _quantity;
                set { _quantity = value; OnPropertyChanged(nameof(Quantity)); OnPropertyChanged(nameof(TotalFormatted)); }
            }
            public decimal Price
            {
                get => _price;
                set { _price = value; OnPropertyChanged(nameof(Price)); OnPropertyChanged(nameof(PriceFormatted)); OnPropertyChanged(nameof(TotalFormatted)); }
            }

            public decimal Total => Quantity * Price;
            public string PriceFormatted => Price.ToString("N0", new CultureInfo("vi-VN")) + " đ";
            public string TotalFormatted => Total.ToString("N0", new CultureInfo("vi-VN")) + " đ";

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Danh sách món ăn - Liên kết trực tiếp với ItemsSource của ListView trong XAML
        public ObservableCollection<BillItem> BillItems { get; set; } = new ObservableCollection<BillItem>();

        private DateTime? _startTime;
        private DispatcherTimer _timer;
        private decimal _ratePerHour = 0; // Giá tiền mỗi giờ của bàn
        private decimal _discountPercent = 0;

        public ucBill()
        {
            InitializeComponent();
            this.DataContext = this;

            // Cài đặt Timer để cập nhật tiền giờ mỗi giây
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        // 1. HÀM MỞ BÀN (Gọi khi chọn bàn từ danh sách bàn)
        public void OpenTable(string tableName, DateTime startTime, decimal ratePerHour)
        {
            txtTableTitle.Text = tableName;
            _startTime = startTime;
            _ratePerHour = ratePerHour;
            txtStartTime.Text = startTime.ToString("HH:mm:ss");

            BillItems.Clear(); // Reset danh sách món
            txtFoodCount.Text = "0";

            _timer.Start();
            UpdateTotal();
        }

        // 2. HÀM GỌI MÓN (Hàm quan trọng bạn cần gọi từ Menu)
        public void AddFoodItem(string foodName, decimal price, int quantity = 1)
        {
            // Kiểm tra xem món này đã có trong danh sách chưa
            var existingItem = BillItems.FirstOrDefault(i => i.Name == foodName);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // Nếu có rồi thì tăng số lượng
            }
            else
            {
                // Nếu chưa có thì thêm món mới vào (Tự động hiện lên ListView nhờ ObservableCollection)
                BillItems.Add(new BillItem { Name = foodName, Price = price, Quantity = quantity });
            }

            // Cập nhật số lượng tổng món hiển thị trên giao diện
            txtFoodCount.Text = BillItems.Sum(i => i.Quantity).ToString();
            UpdateTotal();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_startTime.HasValue)
            {
                var elapsed = DateTime.Now - _startTime.Value;
                txtPlayTime.Text = $"{(int)elapsed.TotalHours}h {elapsed.Minutes}p {elapsed.Seconds}s";
                UpdateTotal();
            }
        }

        // 3. HÀM TÍNH TỔNG TIỀN (Giờ + Món - Giảm giá)
        private void UpdateTotal()
        {
            // Tính tiền món
            decimal totalFood = BillItems.Sum(i => i.Total);

            // Tính tiền giờ
            decimal hourCost = 0;
            if (_startTime.HasValue)
            {
                var elapsed = DateTime.Now - _startTime.Value;
                decimal totalHours = (decimal)elapsed.TotalHours;

                // Logic: Tính tiền giờ thực tế dựa trên giá tiền/giờ
                hourCost = totalHours * _ratePerHour;
            }

            // Tính giảm giá và tổng cộng
            decimal subTotal = totalFood + hourCost;
            decimal discountValue = subTotal * (_discountPercent / 100);
            decimal grandTotal = subTotal - discountValue;

            // Hiển thị lên màn hình
            txtGrandTotal.Text = grandTotal.ToString("N0", new CultureInfo("vi-VN")) + " đ";
        }

        // Xử lý khi chọn giảm giá trong ComboBox
        private void cbDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDiscount.SelectedItem is ComboBoxItem item)
            {
                string content = item.Content.ToString();
                // Lấy số từ chuỗi "Giảm giá: 10%"
                string numericPart = new string(content.Where(char.IsDigit).ToArray());
                if (decimal.TryParse(numericPart, out decimal val))
                    _discountPercent = val;
                else
                    _discountPercent = 0;
            }
            UpdateTotal();
        }

        // 4. CÁC NÚT HÀNH ĐỘNG
        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            if (BillItems.Count == 0 && !_startTime.HasValue) return;

            MessageBox.Show($"Thanh toán thành công!\nTổng cộng: {txtGrandTotal.Text}", "Thông báo");
            ResetBill();
        }

        private void btnTransferTable_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng chuyển bàn đang được phát triển", "Thông báo");
        }

        private void btnCancelTable_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy bàn này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ResetBill();
            }
        }

        private void ResetBill()
        {
            _timer.Stop();
            _startTime = null;
            BillItems.Clear();
            txtTableTitle.Text = "Bàn";
            txtStartTime.Text = "00:00:00";
            txtPlayTime.Text = "0h 0p";
            txtFoodCount.Text = "0";
            txtGrandTotal.Text = "0 đ";
            cbDiscount.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}