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
    /// AddWorkshopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddWorkshopWindow : Window
    {
        private WorkShopManage_Page wsmPage { set; get; }
        public AddWorkshopWindow(WorkShopManage_Page wsmPage)
        {
            this.wsmPage = wsmPage;
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer db = new DataAccessLayer();
            if (txt_WorkshopName.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please input the workshop name!");
                return;
            } else if (db.QueryWorkshopByName(txt_WorkshopName.Text).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Already exists workshop named " + txt_WorkshopName + "!");
                return;
            } else if (txt_WorkshopId.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please input the workname id!");
                return;
            }
            else if (db.QueryWorkshopById(int.Parse(txt_WorkshopId.Text)).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Already exists workshop with id: " + txt_WorkshopId.Text + "!");
                return;
            }else
            {
                Workshop ws = new Workshop();
                ws.Name = txt_WorkshopName.Text;
                try
                {
                    ws.Id = int.Parse(txt_WorkshopId.Text);
                }
                catch
                {
                SystemSounds.Beep.Play();
                MessageBox.Show("Workshop Id must be numberic!");
                return;
                }
                db.InsertWorkshop(ws);
                wsmPage.FillListView();
                this.Close();
                return;
            }
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
