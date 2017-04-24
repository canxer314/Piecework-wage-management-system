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
        }

        private void btn_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            foreach (Employee empl in db.QueryEmployeeByAll())
            {
                if(txt_EmployeeId.Text == empl.Id.ToString())
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists employee having eID:" + txt_EmployeeId.Text);
                    return;
                }
            }
            if(txt_EmployeeName == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Name can not be blank!");
                return;
            }
            if(txt_EmployeeId == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Id can not be blank!");
                return;
            }
            if(txt_Department == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Department can not be blank!");
                return;
            }
            if(txt_Workshop == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Workshop can not be blank!");
                return;
            }
            if(txt_Job == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Job can not be blank!");
                return;
            }
            if(txt_Telephone == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Employee Telephone can not be blank!");
                return;
            }
            Employee employee = new Employee();
            employee.Name = txt_EmployeeName.Text;
            employee.PyAbbr = (new BusinessLogicLayer()).PinyinAbbreviationConvert(txt_EmployeeName.Text);
            try
            {
            employee.Id = int.Parse(txt_EmployeeId.Text);
            }
            catch { }
            employee.Department = txt_Department.Text;
            employee.Password = txt_EmployeeId.Text;
            employee.Telephone = txt_Telephone.Text;
            employee.Workshop = txt_Workshop.Text;
            employee.Job = txt_Job.Text;
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
            txt_Department.Text = null;
            txt_EmployeeId.Text = null;
            txt_EmployeeName.Text = null;
            txt_Job.Text = null;
            txt_Telephone.Text = null;
            txt_Workshop.Text = null;
            txt_EmployeeName.Focus();
        }

        private void txt_Telephone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_AddEmployee_Click(sender,e);
        }
    }
}
