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
    /// WorkShopManage_Page.xaml 的交互逻辑
    /// </summary>
    public partial class WorkShopManage_Page : Page
    {
        public DataAccessLayer db { set; get; }
        public WorkShopManage_Page()
        {
            db = new DataAccessLayer();
            InitializeComponent();
            FillListView();
        }

        public void FillListView()
        {
            try
            {
                gridWorkshop.ItemsSource = db.QueryWorkshopByAll();
            }
            catch
            {

            }
        }
        private void AddWorkshop_Click(object sender, RoutedEventArgs e)
        {
            AddWorkshopWindow addWSwnd = new AddWorkshopWindow(this);
            addWSwnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWSwnd.ShowDialog();
        }

        private void ModifyWorkshop_Click(object sender, RoutedEventArgs e)
        {
            if (gridWorkshop.SelectedItems.Count != 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择一个车间！");
                return;
            }
            IEnumerable<Workshop> wsList;
            wsList = db.QueryWorkshopById((gridWorkshop.SelectedItem as Workshop).Id);
            ModifyWorkshopWindow modifyWorkshopWnd = new ModifyWorkshopWindow(wsList.ElementAt(0), this);
            modifyWorkshopWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyWorkshopWnd.ShowDialog();
        }

        private void RemoveWorkshop_Click(object sender, RoutedEventArgs e)
        {
            if (gridWorkshop.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择想要删除的车间！");
                return;
            }
            foreach (Workshop item in gridWorkshop.SelectedItems)
            {
                try
                {
                    db.DeleteWorkshopById(item.Id);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("删除失败！");
                    FillListView();
                    return;
                }
            }
            FillListView();
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            FillListView();
            SystemSounds.Beep.Play();
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveWorker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchWorkshop_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearchWorkshop.Text.Trim()) == true)
            {
                gridWorkshop.ItemsSource = db.QueryWorkshopByAll();
                return;
            }
            if (rbtnWorkshopName.IsChecked == true)
            {
                gridWorkshop.ItemsSource = db.QueryWorkshopByName(txtSearchWorkshop.Text);
            }
            else if (rbtnWorkshopId.IsChecked == true)
            {
                gridWorkshop.ItemsSource = db.QueryWorkshopById(int.Parse(txtSearchWorkshop.Text));
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择搜索条件！");
            }
        }

        private void btnSearchWorker_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
