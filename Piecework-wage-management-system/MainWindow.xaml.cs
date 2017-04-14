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
            InitializeComponent();
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            dataAccessLayer.DataBaseInit();
        }

        private void btnAdministratorLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLoginWindow adminLoginWindow = new AdminLoginWindow();
            adminLoginWindow.Show();
        }

        private void btnReckonByThePiece_Click(object sender, RoutedEventArgs e)
        {
            ReckonByThePieceWindow reckonByThePieceWindow = new ReckonByThePieceWindow();
            reckonByThePieceWindow.Show();
            this.Close();
        }

        private void btnTrackRecord_Click(object sender, RoutedEventArgs e)
        {
            TrackRecordWindow trackRecordWindow = new TrackRecordWindow();
            trackRecordWindow = new TrackRecordWindow();
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (AdminLoginWindow.IsLogin == true)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
        }
    }
}
