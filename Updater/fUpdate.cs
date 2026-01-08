using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;


namespace Updater
{
    public partial class fUpdate : Form
    {
        public fUpdate()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KillProcess("AppManageBilliard.GUI");
            string downloadUrl = "https://github.com/loidinh84/AppManageBilliard-Updates/releases/download/v1.0.1/Update.zip";
            string zipFile = Path.Combine(Application.StartupPath, "Update.zip");
            string extractPath = Application.StartupPath;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, zipFile);
                }
                using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string fullPath = Path.Combine(extractPath, entry.FullName);

                        string directory = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                        if (!string.IsNullOrEmpty(entry.Name))
                            entry.ExtractToFile(fullPath, true);
                    }
                }
                if (File.Exists(zipFile)) File.Delete(zipFile);

                MessageBox.Show("Cập nhật thành công!", "Thông báo");

                Process.Start("AppManageBilliard.GUI.exe");
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }
        private void KillProcess(string processName)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    process.Kill();
                    process.WaitForExit(3000);
                }
            }
            catch { /* Bỏ qua nếu không tìm thấy hoặc không có quyền */ }
        }
    }
}
