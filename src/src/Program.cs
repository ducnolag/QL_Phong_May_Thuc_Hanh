using System;
using System.Windows.Forms;
using src.Helpers;

namespace src
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Test database connection and seed initial data
            string dbError;
            if (DatabaseHelper.TestConnection(out dbError))
            {
                DatabaseHelper.SeedInitialData();
            }
            else
            {
                MessageBox.Show(
                    "Không thể kết nối đến cơ sở dữ liệu!\n\n" + dbError +
                    "\n\nVui lòng kiểm tra SQL Server đang chạy và connection string đúng.",
                    "Lỗi Kết Nối CSDL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            Application.Run(new Forms.LoginForm());
        }
    }
}
