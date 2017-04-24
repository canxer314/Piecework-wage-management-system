using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
            try
            {
                gridEmployees.ItemsSource = db.QueryEmployeeByAll();
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
            if (gridEmployees.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a employee in the table before you alter it.");
                return;
            }
            if (gridEmployees.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not alter multiple employee information.");
                return;
            }
            IEnumerable<Employee> employeeList;
            employeeList = db.QueryEmployeeByEID((gridEmployees.SelectedItem as Employee).Id);
            AlterEmployeeWindow alterEmployeeWindow = new AlterEmployeeWindow(employeeList.ElementAt(0), db, this);
            alterEmployeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alterEmployeeWindow.ShowDialog();
        }

        private void RemoveEmployee(object sender, RoutedEventArgs e)
        {
            if (gridEmployees.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select at least one employee in the table before you remove it.");
                return;
            }
            foreach (Employee item in gridEmployees.SelectedItems)
            {
                db.DeleteEmployeeById(item.Id);
            }
            FillListView();
        }

        private void RefreshList(object sender, RoutedEventArgs e)
        {
            FillListView();
            SystemSounds.Beep.Play();
        }
    }
}
