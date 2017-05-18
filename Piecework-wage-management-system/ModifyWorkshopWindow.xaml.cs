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
    /// ModifyWorkshopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyWorkshopWindow : Window
    {
        private Workshop OriginWorkshop { set; get; }
        private WorkShopManage_Page wsmPage { set; get; }
        private DataAccessLayer db { set; get; }
        public ModifyWorkshopWindow(Workshop ws, WorkShopManage_Page wsmPage)
        {
            this.wsmPage = wsmPage;
            OriginWorkshop = ws;
            db = new DataAccessLayer();
            InitializeComponent();
            RestoreOriginWorkshop();
        }

        private void RestoreOriginWorkshop()
        {
            txt_WorkshopName.Text = OriginWorkshop.Name;
            txt_WorkshopId.Text = OriginWorkshop.Id.ToString();
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginWorkshop();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Workshop modifiedWorkshop = new Workshop();
            if (txt_WorkshopName.Text != OriginWorkshop.Name)
                if (db.QueryWorkshopByName(txt_WorkshopName.Text).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("已存在车间名称：" + txt_WorkshopName.Text + "!");
                    return;
                }
            if (txt_WorkshopId.Text != OriginWorkshop.Id.ToString())
                if (db.QueryWorkshopById(int.Parse(txt_WorkshopId.Text)).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("已存在车间编号：" + txt_WorkshopId.Text + "!");
                    return;
                }
            modifiedWorkshop.Name = txt_WorkshopName.Text;
            try
            {
                modifiedWorkshop.Id = int.Parse(txt_WorkshopId.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("车间编号必须为数字！");
                return;
            }
            try
            {
                db.DeleteWorkshopById(OriginWorkshop.Id);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("更改失败！");
                return;
            }
            db.InsertWorkshop(modifiedWorkshop);
            wsmPage.FillListView();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
