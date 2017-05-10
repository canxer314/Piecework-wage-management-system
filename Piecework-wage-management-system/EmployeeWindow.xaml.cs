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
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// EmployeeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public Employee LoginedEmployee { set; get; }
        private ReckonByPiece_Page rPage { set; get; }
        private ShowReckonPayroll_Page sPage { set; get; }
        public EmployeeWindow(Employee employee)
        {
            LoginedEmployee = employee;
            rPage = new ReckonByPiece_Page(LoginedEmployee);
            sPage = new ShowReckonPayroll_Page();
            InitializeComponent();
        }

        private void btn_Reckon_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Content = rPage;
        }

        private void btn_ShowPayroll_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Content = sPage;
        }

        private void btn_LoginOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = new MainWindow();
            mainWnd.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
