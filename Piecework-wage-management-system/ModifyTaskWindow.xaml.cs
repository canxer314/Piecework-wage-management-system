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
    /// ModifyTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyTaskWindow : Window
    {
        private Value OriginTask { set; get; }
        private DataAccessLayer Db { set; get; }
        private ProductionScheduling_Page PsPage { set; get; }
        public ModifyTaskWindow(Value selectedTask, ProductionScheduling_Page page)
        {
            OriginTask = selectedTask;
            PsPage = page;
            Db = new DataAccessLayer();
            InitializeComponent();
            RestoreOriginTask();
        }

        private void RestoreOriginTask()
        {
            txt_Product.Text = OriginTask.Product_Name;
            txt_TaskNum.Text = OriginTask.TaskNum.ToString();
            txt_Value.Text = OriginTask.Name;
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            RestoreOriginTask();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Value modifiedTask = new Value();
            int n = 0;
            try
            {
                n = int.Parse(txt_TaskNum.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Task number must be numberic!");
                return;
            }
            if (n != OriginTask.TaskNum)
                if (Db.QueryValueByTaskNum(n).Count() != 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Task with task number:" + n);
                    return;
                }
            modifiedTask.TaskNum = n;
            if (txt_Value.Text != OriginTask.Name)
            {
                if (Db.QueryValueByNameAndProductId(txt_Value.Text, OriginTask.Product_Id).Count() != 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Task with value:" + txt_Value.Text);
                    return;
                }
            }
            modifiedTask.Name = txt_Value.Text;
            modifiedTask.Id = OriginTask.Id;
            modifiedTask.Product_Id = OriginTask.Product_Id;
            modifiedTask.Product_Name = OriginTask.Product_Name;
            Db.UpdateValue(modifiedTask);
            PsPage.FillGridTask();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
