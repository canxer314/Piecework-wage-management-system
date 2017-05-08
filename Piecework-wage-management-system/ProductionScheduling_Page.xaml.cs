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
    /// ProductionScheduling_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionScheduling_Page : Page
    {
        private DataAccessLayer Db { set; get; }
        public ProductionScheduling_Page()
        {
            Db = new DataAccessLayer();
            InitializeComponent();
            FillGridTask();
        }
        public void FillGridTask()
        {
            gridTask.ItemsSource = Db.QueryValueByAll();
        }
        public void FillGridProcedure()
        {
            try
            {
                gridProcedure.ItemsSource = Db.QueryRelationshipByProduct_Id((gridTask.SelectedItem as Value).Product_Id);
            }
            catch { }
        }

        public void FillPrice()
        {
            if (gridTask.SelectedItems.Count != 1)
                return;
            int id = (gridTask.SelectedItem as Value).Id;
            gridPrice.ItemsSource = Db.QueryValuePriceByValueId(id);
        }

        public void FillEmployee()
        {
            List<Assign> assignList = Db.QueryAssignByValueId((gridPrice.SelectedItem as ValuePrice).Value_Id).ToList();
            List<Employee> employeeList = new List<Employee>();
            foreach (Assign a in assignList)
            {
                Employee e = Db.QueryEmployeeByEID(a.EmployeeId) as Employee;
                employeeList.Add(e);
            }
            gridEmployee.ItemsSource = employeeList;
        }

        private void gridTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridProcedure();
            FillPrice();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWnd = new AddTaskWindow(this);
            addTaskWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addTaskWnd.ShowDialog();
        }

        private void ModifyTask_Click(object sender, RoutedEventArgs e)
        {
            if (gridTask.SelectedItems.Count != 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You can only modify one task at one time!");
                return;
            }
            ModifyTaskWindow mtWnd = new ModifyTaskWindow(gridTask.SelectedItem as Value, this);
            mtWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mtWnd.ShowDialog();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (gridTask.SelectedItems.Count != 0)
            {
                foreach (Value item in gridTask.SelectedItems)
                {
                    Db.DeleteValueById(item.Id);
                }
                FillGridTask();
            }
        }

        private void ModifyPrice_Click(object sender, RoutedEventArgs e)
        {
            if (gridPrice.SelectedItems.Count != 1)
            {
                return;
            }
            else
            {
                ModifyPriceWindow mpWnd = new ModifyPriceWindow(this, gridPrice.SelectedItem as ValuePrice);
                mpWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mpWnd.ShowDialog();
            }
        }

        private void gridPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridProcedure.SelectedIndex = gridPrice.SelectedIndex;
        }

        private void gridProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridPrice.SelectedIndex = gridProcedure.SelectedIndex;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AssignEmployeeWindow aeWnd = new AssignEmployeeWindow(this,gridPrice.SelectedItem as ValuePrice);
            aeWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aeWnd.ShowDialog();
        }

        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            foreach(Employee employee in gridEmployee.SelectedItems)
            {
            }
        }
    }
}
