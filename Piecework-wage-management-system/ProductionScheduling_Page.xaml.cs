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
            try
            {
                gridTask.SelectedIndex = 0;
                gridProcedure.SelectedIndex = 0;
                gridPrice.SelectedIndex = 0;
                gridEmployee.SelectedIndex = 0;
            }
            catch { }
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
            if (gridPrice.SelectedItems.Count != 1)
                return;
            List<Assign> assignList = Db.QueryAssignByValuePrice((gridPrice.SelectedItem as ValuePrice)).ToList();
            List<Employee> employeeList = new List<Employee>();
            foreach (Assign a in assignList)
            {
                Employee e = Db.QueryEmployeeByEID(a.EmployeeId).Single();
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
                MessageBox.Show("一次只能更改一个生产任务信息！");
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
                    try
                    {
                        Db.DeleteValueById(item.Id);
                    }
                    catch
                    {
                        SystemSounds.Beep.Play();
                        MessageBox.Show("删除失败！");
                        FillGridTask();
                        return;
                    }
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
            FillEmployee();
        }

        private void gridProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridPrice.SelectedIndex = gridProcedure.SelectedIndex;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (gridPrice.SelectedItems.Count == 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择一个工序！");
                return;
            }
            AssignEmployeeWindow aeWnd = new AssignEmployeeWindow(this, gridPrice.SelectedItem as ValuePrice);
            aeWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aeWnd.ShowDialog();
        }

        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (gridEmployee.SelectedItems.Count == 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择想要取消分配的员工！");
                return;
            }
            foreach (Employee employee in gridEmployee.SelectedItems)
            {
                Value v = gridTask.SelectedItem as Value;
                Relationship r = gridProcedure.SelectedItem as Relationship;
                Procedure p = Db.QueryProcedureByName(r.InputProcedure).Single();
                ValuePrice vp = gridPrice.SelectedItem as ValuePrice;
                foreach (Employee eTmp in gridEmployee.SelectedItems)
                {
                    try
                    {
                        Db.DeleteAssignByValueIdAndProcedureIdAndEmployeeId(v.Id, p.Id, eTmp.Id);
                    }
                    catch
                    {
                        SystemSounds.Beep.Play();
                        MessageBox.Show("取消分配失败！");
                        FillEmployee();
                        return;
                    }
                }
            }
            FillEmployee();
        }
    }
}
