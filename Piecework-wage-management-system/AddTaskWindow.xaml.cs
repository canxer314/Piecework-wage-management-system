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
    /// AddTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private ProductionScheduling_Page PsPage { set; get; }
        private DataAccessLayer Db { set; get; }
        public AddTaskWindow(ProductionScheduling_Page psPage)
        {
            PsPage = psPage;
            Db = new DataAccessLayer();
            InitializeComponent();
            BindingComboBoxItemsSource();
        }

        private void BindingComboBoxItemsSource()
        {
            cmb_Product.ItemsSource = Db.QueryProductByAll();
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            cmb_Product.SelectedIndex = -1;
            txt_TaskNum.Text = string.Empty;
            txt_Value.Text = string.Empty;
        }

        private void btn_AddTask_Click(object sender, RoutedEventArgs e)
        {
            Value v = new Value();
            if (cmb_Product.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("产品编号不能为空！");
                return;
            }
            else
            {
                v.Product_Id = (cmb_Product.SelectedItem as Product).Id;
                v.Product_Name = (cmb_Product.SelectedItem as Product).Name;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(txt_TaskNum.Text) == true)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("任务编号不能为空！");
                    return;
                }
                else
                {
                    v.TaskNum = int.Parse(txt_TaskNum.Text);
                }
            }
            catch (FormatException fe)
            {
                txt_TaskNum.Text = string.Empty;
                SystemSounds.Beep.Play();
                MessageBox.Show("任务编号必须为数字！");
                return;
            }
            //if (Db.QueryValueByName(txt_Value.Text).Count() == 0)
            // QueryValueByNameNotInProduct
            if (Db.QueryValueByNameAndProductId(txt_Value.Text, v.Product_Id).Count() == 0)
            {
                v.Name = txt_Value.Text;
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("该产品已存在感值：" + txt_Value.Text);
                txt_Value.Text = string.Empty;
                return;
            }
            v.TaskDate = DateTime.Now;
            Db.InsertValue(v);
            IEnumerable<Relationship> relateList = Db.QueryRelationshipByProduct_Id(v.Product_Id);
            foreach(Relationship item in relateList)
            {
                ValuePrice price = new ValuePrice();
                price.Procedure_Id = Db.QueryProcedureByName(item.InputProcedure).Single().Id;
                price.Value_Id = Db.QueryValueByTaskNum(v.TaskNum).Single().Id;
                Db.InsertValuePrice(price);
            }
            PsPage.FillGridTask();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
