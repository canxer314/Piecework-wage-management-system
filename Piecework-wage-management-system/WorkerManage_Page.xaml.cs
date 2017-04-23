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
    /// WorkerManage_Page.xaml 的交互逻辑
    /// </summary>
    public partial class WorkerManage_Page : Page
    {
        public DataAccessLayer db { set; get; }
        public WorkerManage_Page()
        {
            db = new DataAccessLayer();
            InitializeComponent();
            FillListView();
        }

        public void FillListView()
        {
            List<Employee> list = db.QueryEmployeeByAll().ToList();
            if (list == null)
            {
                MessageBox.Show("No Employee in the Database");
            }
            else
            {
                employee_ListView.ItemsSource = db.QueryEmployeeByAll();
                MessageBox.Show(list.ElementAt(0).Name.ToString());
            }
            try
            {
                //employee_ListView.ItemsSource = list;
                tb_show.Text = list.ElementAt(0).Name;
                ///employee_ListView.ItemsSource = db.QueryEmployeeByAll();
            }
            catch
            {
            }
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmplWnd = new AddEmployeeWindow(db, this);
            addEmplWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addEmplWnd.ShowDialog();
        }

        private void AlterEmployee(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveEmployee(object sender, RoutedEventArgs e)
        {

        }
    }
}
