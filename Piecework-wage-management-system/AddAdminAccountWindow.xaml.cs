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
    /// AddAdminAccountWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddAdminAccountWindow : Window
    {
        private AdministratorManage_Page adminManagePage { set; get; }
        public AddAdminAccountWindow(AdministratorManage_Page adminManagePage )
        {
            this.adminManagePage = adminManagePage;
            InitializeComponent();
        }

        private void btn_Signup_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer dataAccesslayer = new DataAccessLayer();
            if(txt_Username.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入用户名！");
                pwd_Password.Password = null;
                pwd_Confirm.Password = null;
                return;
            }else if(dataAccesslayer.QueryAdministratorByName(txt_Username.Text).Count()>0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("已存在管理员：" + txt_Username + "！");
                return;
            }else if(pwd_Password.Password.Trim() == String.Empty || pwd_Confirm.Password.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入密码！");
                pwd_Password.Password = null;
                pwd_Confirm.Password = null;
                return;
            }
            else if(pwd_Password.Password == pwd_Confirm.Password)
            {
                Administrator administrator = new Administrator();
                administrator.Name = txt_Username.Text.ToString();
                administrator.Password = pwd_Confirm.Password;
                dataAccesslayer.InsertAdministrator(administrator);
                adminManagePage.FillDataGrid();
                this.Close();
                return;
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("两次输入的密码不一致！");
                pwd_Password.Password = null;
                pwd_Confirm.Password = null;
                return;
            }
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
