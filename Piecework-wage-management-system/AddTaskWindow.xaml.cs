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
            txt_TaskNum = null;
            txt_Value = null;
        }

        private void btn_AddProcedure_Click(object sender, RoutedEventArgs e)
        {
            Value v = new Value();
            if (cmb_Product.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a product!");
                return;
            }
            else
            {
                v.Product_Id = (cmb_Product.SelectedItem as Product).Id;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(txt_TaskNum.Text) == true)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("The Task Number must not be blank!");
                    return;
                }
                else
                {
                    v.TaskNum = int.Parse(txt_TaskNum.Text);
                }
            }
            catch (FormatException fe)
            {
                txt_TaskNum = null;
                SystemSounds.Beep.Play();
                MessageBox.Show("Task number must be numberic!");
                return;
            }
            //if (Db.QueryValueByName(txt_Value.Text).Count() == 0)
            // QueryValueByNameNotInProduct
            {
                v.Name = txt_Value.Text;
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Task number must be numberic!");
                return;
            }
            Db.InsertValue(v);
            PsPage.FillGridTask();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
