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
    /// ModifyAdminPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyAdminPasswordWindow : Window
    {
        public Administrator admin { set; get; }
        public DataAccessLayer db { set; get; }
        public ModifyAdminPasswordWindow(Administrator admin,DataAccessLayer db)
        {
            this.admin = admin;
            this.db = db;
            InitializeComponent();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            if(pwd_OldPassword.Password != admin.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("旧密码不正确！");
                return;
            }else if(pwd_NewPassword.Password != pwd_Confirm.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("两次输入的密码不相同！");
            }else
            {
                db.UpdateAdministratorPasswordById(admin.Id, pwd_NewPassword.Password);
                this.Close();
            }
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
