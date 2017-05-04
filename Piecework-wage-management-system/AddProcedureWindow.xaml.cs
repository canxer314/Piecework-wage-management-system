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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class AddProcedureWindow : Window
    {
        private Product SelectedProduct { set; get; }
        private DataAccessLayer db { set; get; }
        private ProcedureManage_Page pmPage { set; get; }

        public AddProcedureWindow(ProcedureManage_Page pmPage, Product p)
        {
            this.pmPage = pmPage;
            SelectedProduct = p;
            db = new DataAccessLayer();
            InitializeComponent();
            BindingComboBoxItemSource();
        }

        private void BindingComboBoxItemSource()
        {
            IEnumerable<Product> ps = db.QueryProductByAll();
            cmb_Product.ItemsSource = ps;
            int i = -1;
            foreach (var p in ps)
            {
                i++;
                if (p.Name == SelectedProduct.Name)
                    break;
            }
            cmb_Product.SelectedIndex = i;
        }

        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            BindingComboBoxItemSource();
            txt_ProcedureId.Text = null;
            txt_ProcedureName.Text = null;
        }

        private void btn_AddProcedure_Click(object sender, RoutedEventArgs e)
        {
            foreach (Procedure proc in db.QueryProcedureByAll())
            {
                if (txt_ProcedureId.Text == proc.Id.ToString())
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Procedure with ID:" + txt_ProcedureId.Text);
                    return;
                }
            }
            if (txt_ProcedureName == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Procedure Name can not be blank!");
                return;
            }
            if (txt_ProcedureId == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Procedure Id can not be blank!");
                return;
            }
            if (cmb_Product.SelectedItem == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Belong to what Product can not be blank!");
                return;
            }
            Procedure p = new Procedure();
            p.Name = txt_ProcedureName.Text;
            try
            {
                p.Id = int.Parse(txt_ProcedureId.Text);
            }
            catch { }
            p.Product_Id = (cmb_Product.SelectedItem as Product).Id;
            db.InsertProcedure(p);
            pmPage.FillGridView_Procedure();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
