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
    /// AddJobWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddJobWindow : Window
    {
        private JobManage_Page jmPage { set; get; }
        public AddJobWindow(JobManage_Page jmPage)
        {
            this.jmPage = jmPage;
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer db = new DataAccessLayer();
            if (txt_JobName.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入工种名称！");
                return;
            }
            else if (db.QueryJobByName(txt_JobName.Text).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("已存在工种名称：" + txt_JobName + "!");
                return;
            }
            else if (txt_JobId.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入工种编号！");
                return;
            }
            else if (db.QueryJobById(int.Parse(txt_JobId.Text)).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("已存在工种编号：" + txt_JobId.Text + "!");
                return;
            }
            else
            {
                Job job = new Job();
                job.Name = txt_JobName.Text;
                try
                {
                    job.Id = int.Parse(txt_JobId.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("工种编号必须为数字!");
                    return;
                }
                db.InsertJob(job);
                jmPage.FillListView();
                this.Close();
            }
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
