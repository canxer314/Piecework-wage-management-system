using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            if (dataAccessLayer.DataBaseInit() == false)
            {
                MessageBox.Show("Database connection failed. Make sure the setting is correct.");
                this.Close();
            }
            else if (dataAccessLayer.QueryAdministratorByAll().Count() == 0)
            {
                //MessageBox.Show(dataAccessLayer.QueryAdministratorByAll().Count().ToString());
                FirstUse_Window firstUseWnd = new FirstUse_Window();
                firstUseWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                firstUseWnd.ShowDialog();
            }
        }

        private void btnAdministratorLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWnd = new AdminWindow();
            adminWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            adminWnd.Show();
            this.Close();
            //AdminLoginWindow adminLoginWindow = new AdminLoginWindow();
            //adminLoginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //adminLoginWindow.ShowDialog();
        }

        private void btnReckonByThePiece_Click(object sender, RoutedEventArgs e)
        {
            EmployeeLoginWindow employLoginWnd = new EmployeeLoginWindow(this);
            employLoginWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            employLoginWnd.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (AdminLoginWindow.IsLogin == true)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                adminWindow.Show();
                this.Close();
            }
            if (FirstUse_Window.noAdminAccount == true)
                this.Close();
        }
    }
}
