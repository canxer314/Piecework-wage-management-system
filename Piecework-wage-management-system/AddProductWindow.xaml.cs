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
    /// AddProductWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private ProcedureManage_Page pmPage { set; get; }
        public AddProductWindow(ProcedureManage_Page pmPage)
        {
            this.pmPage = pmPage;
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer db = new DataAccessLayer();
            if (txt_ProductName.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入产品名称！");
                return;
            }
            else if (db.QueryProductByName(txt_ProductName.Text).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("已存在产品名称：" + txt_ProductName + "!");
                return;
            }
            else if (txt_ProductId.Text.Trim() == String.Empty)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请输入产品编号！");
                return;
            }
            else if (db.QueryProductById(int.Parse(txt_ProductId.Text)).Count() > 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("已存在产品编号：" + txt_ProductId.Text + "!");
                return;
            }
            else
            {
                Product p = new Product();
                p.Name = txt_ProductName.Text;
                try
                {
                    p.Id = int.Parse(txt_ProductId.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("产品编号必须为数字！");
                    return;
                }
                db.InsertProduct(p);
                pmPage.FillGridView_Product();
                this.Close();
            }
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
