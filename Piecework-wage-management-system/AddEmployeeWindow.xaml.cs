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
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// AddEmployeeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public DataAccessLayer db { set; get; }
        public WorkerManage_Page wmPage { set; get; }
        public AddEmployeeWindow(DataAccessLayer db, WorkerManage_Page wmPage)
        {
            this.db = db;
            this.wmPage = wmPage;
            InitializeComponent();
            BindingComboBoxItemsSource();
        }

        private void BindingComboBoxItemsSource()
        {
            cmb_Workshop.ItemsSource = db.QueryWorkshopByAll();
            cmb_Job.ItemsSource = db.QueryJobByAll();
        }
        private void btn_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            foreach (Employee empl in db.QueryEmployeeByAll())
            {
                if (txt_EmployeeId.Text == empl.Id.ToString())
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("已存在员工号:" + txt_EmployeeId.Text);
                    return;
                }
            }
            if (txt_EmployeeName == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("员工姓名不能为空！");
                return;
            }
            if (txt_EmployeeId == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("员工号不能为空！");
                return;
            }
            if (cmb_Gender.SelectedItem == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("员工性别不能为空！");
                return;
            }
            if (cmb_Workshop.SelectedItem == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("工作车间不能为空！");
                return;
            }
            if (cmb_Job.SelectedItem == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("工种不能为空！");
                return;
            }
            if (txt_Telephone == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("手机号码不能为空！");
                return;
            }
            Employee employee = new Employee();
            employee.Name = txt_EmployeeName.Text;
            try
            {
                employee.Id = int.Parse(txt_EmployeeId.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("员工号必须为数字！");
                return;
            }
            employee.Gender = cmb_Gender.SelectionBoxItem.ToString();
            employee.Workshop = (cmb_Workshop.SelectedItem as Workshop).Name;
            employee.Job = (cmb_Job.SelectedItem as Job).Name;
            employee.Password = txt_EmployeeId.Text;
            employee.Telephone = txt_Telephone.Text;
            db.InsertEmployee(employee);
            wmPage.FillListView();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            cmb_Gender.SelectedItem = null;
            txt_EmployeeId.Text = null;
            txt_EmployeeName.Text = null;
            cmb_Job.SelectedItem = null;
            txt_Telephone.Text = null;
            cmb_Workshop.SelectedItem = null;
            txt_EmployeeName.Focus();
        }

        private void txt_Telephone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_AddEmployee_Click(sender, e);
        }
    }
}
