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
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// FirstUse_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FirstUse_Window : Window
    {
        public static bool noAdminAccount { get; set; }

        public FirstUse_Window()
        {
            noAdminAccount = true;
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            if(txtUsername.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入用户名！");
                pwbPassword.Password = null;
                pwbConfirm.Password = null;
                return;
            }else if(pwbConfirm.Password.Trim() == String.Empty || pwbPassword.Password.Trim() == String.Empty)
            {
                MessageBox.Show("请输入密码！");
                pwbPassword.Password = null;
                pwbConfirm.Password = null;
                return;
            }
            else if(pwbPassword.Password == pwbConfirm.Password)
            {
                Administrator administrator = new Administrator();
                administrator.Name = txtUsername.Text.ToString();
                administrator.Password = pwbConfirm.Password;
                DataAccessLayer dataAccesslayer = new DataAccessLayer();
                dataAccesslayer.InsertAdministrator(administrator);
                noAdminAccount = false;
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("两次密码不一致！");
                pwbPassword.Password = null;
                pwbConfirm.Password = null;
                return;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pwbConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSignup_Click(sender, e);
        }
    }
}
