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
    /// ModifyEmployeePassword.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyEmployeePassword : Window
    {
        private Employee LoginedEmployee { set; get; }
        public ModifyEmployeePassword(Employee employee)
        {
            LoginedEmployee = employee;
            InitializeComponent();
            txtName.SetBinding(TextBox.TextProperty, new Binding("Name") { Source = LoginedEmployee });
            txtName.SetBinding(TextBox.TextProperty, new Binding("Id") { Source = LoginedEmployee });
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(pwdOldPassword.Password) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入旧密码！");
                return;
            }
            if(pwdOldPassword.Password != LoginedEmployee.Password.ToString())
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("旧密码不正确！");
                return;
            }
            if(String.IsNullOrEmpty(pwdNewPassword.Password) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入新密码！");
                return;
            }
            if(String.IsNullOrEmpty(pwdConfirm.Password) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入密码确认！");
                return;
            }
            if(pwdNewPassword.Password == pwdOldPassword.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("新旧密码不能相同！");
                return;
            }
            if(pwdNewPassword.Password != pwdConfirm.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("两次输入的密码不相同！");
                return;
            }
            LoginedEmployee.Password = pwdConfirm.Password;
            DataAccessLayer dal = new DataAccessLayer();
            dal.UpdateEmployeePassword(LoginedEmployee);
            SystemSounds.Beep.Play();
            MessageBox.Show("密码更改成功！");
            this.Close();
        }
    }
}
