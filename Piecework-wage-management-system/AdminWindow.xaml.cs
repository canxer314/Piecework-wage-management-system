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
    /// AdminWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdministratorManage_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("AdministratorManage_Page.xaml", UriKind.Relative);
        }

        private void btnWorkshopManage_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("WorkShopManage_Page.xaml", UriKind.Relative);
        }

        private void btnJobManage_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("JobManage_Page.xaml", UriKind.Relative);
        }

        private void btnWorkerManage_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("WorkerManage_Page.xaml", UriKind.Relative);
        }

        private void btnProcedureManage_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("ProcedureManage_Page.xaml", UriKind.Relative);
        }

        private void btnProductionScheduling_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("ProductionScheduling_Page.xaml", UriKind.Relative);
        }

        private void btnPrintPayroll_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("PrintPayroll_Page.xaml", UriKind.Relative);
        }

        private void btnInitDatabase_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("InitializeDatabase_Page.xaml", UriKind.Relative);
        }

        private void btnLoginOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = new MainWindow();
            mainWnd.Show();
            this.Close();
        }

        private void btnProductionState_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Source = new Uri("ProductionState_Page.xaml", UriKind.Relative);
        }
    }
}
