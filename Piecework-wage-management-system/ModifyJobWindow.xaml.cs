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
    /// ModifyJobWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyJobWindow : Window
    {
        private Job OriginJob { set; get; }
        private JobManage_Page jmPage { set; get; }
        private DataAccessLayer db { set; get; }
        public ModifyJobWindow(Job job, JobManage_Page jmPage)
        {
            this.jmPage = jmPage;
            OriginJob = job;
            db = new DataAccessLayer();
            InitializeComponent();
            RestoreOriginJob();
        }
        private void RestoreOriginJob()
        {
            txt_JobName.Text = OriginJob.Name;
            txt_JobId.Text = OriginJob.Id.ToString();
        }
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Job modifiedJob = new Job();
            if (txt_JobName.Text != OriginJob.Name)
                if (db.QueryJobByName(txt_JobName.Text).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Job with name: " + txt_JobName.Text + "!");
                    return;
                }
            if (txt_JobId.Text != OriginJob.Id.ToString())
                if (db.QueryJobById(int.Parse(txt_JobId.Text)).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Job with ID: " + txt_JobId.Text + "!");
                    return;
                }
            modifiedJob.Name = txt_JobName.Text;
            try
            {
                modifiedJob.Id = int.Parse(txt_JobId.Text);
            }
            catch
            {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Job Id must be numberic!");
                    return;
            }
            db.DeleteJobById(OriginJob.Id);
            db.InsertJob(modifiedJob);
            jmPage.FillListView();
            this.Close();
        }

        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginJob();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
