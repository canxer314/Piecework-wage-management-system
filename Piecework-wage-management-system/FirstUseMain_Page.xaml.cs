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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// FirstUseMain_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FirstUseMain_Page : Page
    {
        public FirstUseMain_Page()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            if(pwbPassword.SecurePassword==pwbConfirm.SecurePassword)
            {
                Administrator administrator = new Administrator();
                administrator.Name = txtUsername.Text.ToString();
                administrator.Password = pwbConfirm.SecurePassword.ToString();
                administrator.Authority = 1;
                DataAccessLayer dataAccesslayer = new DataAccessLayer();
                dataAccesslayer.InsertAdministrator(administrator);
                Window firstUseWindow = (Window)this.Parent;
                firstUseWindow.Close();
            }
        }
    }
}
