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
    /// AlterEmployeeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlterEmployeeWindow : Window
    {
        public Employee OriginEmployee { set; get; }
        public WorkerManage_Page wmPage { set; get; }
        public DataAccessLayer db { set; get; }
        public AlterEmployeeWindow(Employee originEmployee, DataAccessLayer db, WorkerManage_Page wmPage)
        {
            this.db = db;
            this.wmPage = wmPage;
            OriginEmployee = originEmployee;
            InitializeComponent();
            RestoreOriginEmployee();
        }

        private void RestoreOriginEmployee()
        {
            txt_EmployeeName.Text = OriginEmployee.Name;
            cmb_Gender.Text = OriginEmployee.Gender;
            txt_EmployeeId.Text = OriginEmployee.Id.ToString();

            cmb_Workshop.ItemsSource = db.QueryWorkshopByAll();
            IEnumerable<Workshop> ws = db.QueryWorkshopByAll();
            int i = -1;
            foreach (var w in ws)
            {
                i++;
                if (w.Name == OriginEmployee.Workshop)
                    break;
            }
            cmb_Workshop.SelectedIndex = i;

            cmb_Job.ItemsSource = db.QueryJobByAll();
            IEnumerable<Job> jobIE = db.QueryJobByAll();
            int j = -1;
            foreach (var jTmp in jobIE)
            {
                j++;
                if (jTmp.Name == OriginEmployee.Job)
                    break;
            }
            cmb_Job.SelectedIndex = j;
            txt_Telephone.Text = OriginEmployee.Telephone;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginEmployee();
        }

        private void btn_AlterEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee alteredEmployee = new Employee();
            if (txt_EmployeeId.Text != OriginEmployee.Id.ToString())
                if (db.QueryEmployeeByEID(int.Parse(txt_EmployeeId.Text)).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists employee with ID:" + txt_EmployeeId.Text);
                    return;
                }
            alteredEmployee.Name = txt_EmployeeName.Text;
            alteredEmployee.Id = int.Parse(txt_EmployeeId.Text);
            alteredEmployee.Gender = cmb_Gender.SelectionBoxItem.ToString();
            alteredEmployee.Workshop = (cmb_Workshop.SelectedItem as Workshop).Name;
            alteredEmployee.Job = (cmb_Job.SelectedItem as Job).Name;
            alteredEmployee.Telephone = txt_Telephone.Text;
            alteredEmployee.Password = OriginEmployee.Password;
            db.DeleteEmployeeById(OriginEmployee.Id);
            db.InsertEmployee(alteredEmployee);
            wmPage.FillListView();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txt_Telephone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_AlterEmployee_Click(sender, e);
        }

        private void txt_EmployeeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_AlterEmployee_Click(sender, e);
        }

        private void txt_EmployeeId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_AlterEmployee_Click(sender, e);
        }

        private void btn_ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            db.UpdateEmployeePasswordById(OriginEmployee.Id, OriginEmployee.Id.ToString());
            SystemSounds.Beep.Play();
            MessageBox.Show("The password of " + OriginEmployee.Name + " has been reset to its default value.");
            this.Close();
        }
    }
}
