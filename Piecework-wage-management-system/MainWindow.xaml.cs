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
            dataAccessLayer.DataBaseInit();
            if (dataAccessLayer.QueryAdministratorByAll().Count() == 0)
            {
                FirstUse_Window firstUseWnd = new FirstUse_Window();
                firstUseWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                firstUseWnd.ShowDialog();
            }

        }

        private void btnAdministratorLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLoginWindow adminLoginWindow = new AdminLoginWindow();
            adminLoginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            adminLoginWindow.ShowDialog();
        }

        private void btnReckonByThePiece_Click(object sender, RoutedEventArgs e)
        {
            ReckonByThePieceWindow reckonByThePieceWindow = new ReckonByThePieceWindow();
            reckonByThePieceWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            reckonByThePieceWindow.Show();
            this.Close();
        }

        private void btnTrackRecord_Click(object sender, RoutedEventArgs e)
        {
            TrackRecordWindow trackRecordWindow = new TrackRecordWindow();
            trackRecordWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            trackRecordWindow.Show();
            this.Close();
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
        }
    }
}
