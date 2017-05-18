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
    /// InitializeDatabase_Page.xaml 的交互逻辑
    /// </summary>
    public partial class InitializeDatabase_Page : Page
    {
        private DataAccessLayer Db { set; get; }
        public InitializeDatabase_Page()
        {
            InitializeComponent();
            Db = new DataAccessLayer();
        }

        private void btnInit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtAdminUsername.Text) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("管理员名称不能为空！");
                return;
            }
            if (String.IsNullOrWhiteSpace(pwdAdminPassword.Password) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("管理员密码不能为空！");
                return;
            }
            IEnumerable<Administrator> adminList = Db.QueryAdministratorByAll();
            foreach (Administrator a in adminList)
            {
                if (a.Name == txtAdminUsername.Text && a.Password == pwdAdminPassword.Password)
                {
                    Db.DropDatabase();
                    Db.DataBaseInit();
                    System.Windows.Forms.Application.Restart();
                    Application.Current.Shutdown();
                    return;
                }
            }
            SystemSounds.Beep.Play();
            MessageBox.Show("管理员账号或密码错误！");
            return;
        }
    }
}
