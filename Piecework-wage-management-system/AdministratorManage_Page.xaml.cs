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
    /// AdministratorManage_Page.xaml 的交互逻辑
    /// </summary>
    public partial class AdministratorManage_Page : Page
    {
        public DataAccessLayer db { set; get; }
        public AdministratorManage_Page()
        {
            db = new DataAccessLayer();
            InitializeComponent();
            FillDataGrid();
        }
        
        public void FillDataGrid()
        {
            try
            {
                gridAdministrator.ItemsSource = db.QueryAdministratorByAll();
            }
            catch
            {

            }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            AddAdminAccountWindow addAdminWnd = new AddAdminAccountWindow(this);
            addAdminWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addAdminWnd.ShowDialog();
        }

        private void ModifyPassword_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdministrator.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a account in the table before you modify its password.");
                return;
            }
            if (gridAdministrator.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify multiple account password at one time.");
                return;
            }
            IEnumerable<Administrator> adminList;
            adminList = db.QueryAdministratorByName((gridAdministrator.SelectedItem as Administrator).Name);
            ModifyAdminPasswordWindow modifyPwdWnd = new ModifyAdminPasswordWindow(adminList.ElementAt(0),db);
            modifyPwdWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyPwdWnd.ShowDialog();
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdministrator.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a admin account in the table before you delete it.");
                return;
            }
            if (gridAdministrator.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not delete multiple admin account at one time.");
                return;
            }
            foreach (Administrator item in gridAdministrator.SelectedItems)
            {
                db.DeleteAdministratorById(item.Id);
            }
            FillDataGrid();
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
            SystemSounds.Beep.Play();
        }
    }
}
