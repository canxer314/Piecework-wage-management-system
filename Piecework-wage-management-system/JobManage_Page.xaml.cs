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
    /// JobManage_Page.xaml 的交互逻辑
    /// </summary>
    public partial class JobManage_Page : Page
    {
        public DataAccessLayer db { set; get; }
        public JobManage_Page()
        {
            db = new DataAccessLayer();
            InitializeComponent();
            FillListView();
        }

        public void FillListView()
        {
            gridJob.ItemsSource = db.QueryJobByAll();
        }
        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJobWindow addJobWnd = new AddJobWindow(this);
            addJobWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addJobWnd.ShowDialog();
        }

        private void ModifyJob_Click(object sender, RoutedEventArgs e)
        {
            if (gridJob.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择工种！");
                return;
            }
            if (gridJob.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法同时修改多个工种！");
                return;
            }
            IEnumerable<Job> jobList;
            jobList = db.QueryJobById((gridJob.SelectedItem as Job).Id);
            ModifyJobWindow modifyJobWnd = new ModifyJobWindow(jobList.ElementAt(0),this);
            modifyJobWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyJobWnd.ShowDialog();
        }

        private void RemoveJob_Click(object sender, RoutedEventArgs e)
        {
            if (gridJob.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择工种！");
                return;
            }
            foreach (Job item in gridJob.SelectedItems)
            {
                try
                {
                    db.DeleteJobById(item.Id);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("删除失败！");
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

        private void btnSearchJob_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearchJob.Text.Trim()) == true)
            {
                gridJob.ItemsSource = db.QueryJobByAll();
                return;
            }
            if(rbtnJobName.IsChecked == true)
            {
                gridJob.ItemsSource = db.QueryJobByName(txtSearchJob.Text);
            }else if(rbtnJobId.IsChecked == true)
            {
                gridJob.ItemsSource = db.QueryJobById(int.Parse(txtSearchJob.Text));
            }else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择搜索方式！");
            }
        }
    }
}
