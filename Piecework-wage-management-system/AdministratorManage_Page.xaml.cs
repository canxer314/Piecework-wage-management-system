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
                MessageBox.Show("请先选择一个想要修改密码的管理员账号！");
                return;
            }
            if (gridAdministrator.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法同时修改多个管理员的密码！");
                return;
            }
            Administrator admin = db.QueryAdministratorByName((gridAdministrator.SelectedItem as Administrator).Name).Single();
            ModifyAdminPasswordWindow modifyPwdWnd = new ModifyAdminPasswordWindow(admin,db);
            modifyPwdWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyPwdWnd.ShowDialog();
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdministrator.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择要删除的管理员账号！");
                return;
            }
            if (gridAdministrator.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法一次删除多个管理员账号！");
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
