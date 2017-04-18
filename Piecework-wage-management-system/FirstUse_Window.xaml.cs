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
        public FirstUse_Window()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            if(pwbPassword.Password==pwbConfirm.Password)
            {
                Administrator administrator = new Administrator();
                administrator.Name = txtUsername.Text.ToString();
                administrator.Password = pwbConfirm.Password;
                administrator.Authority = 1;
                DataAccessLayer dataAccesslayer = new DataAccessLayer();
                dataAccesslayer.InsertAdministrator(administrator);
                this.Close();
            }   
        }
    }
}
