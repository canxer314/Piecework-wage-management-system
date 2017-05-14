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
    /// AddRelationshipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddRelationshipWindow : Window
    {
        private Product SelectedProduct { set; get; }
        private DataAccessLayer Db { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        public AddRelationshipWindow(ProcedureManage_Page pmPage, Product product)
        {
            PmPage = pmPage;
            SelectedProduct = product;
            Db = new DataAccessLayer();
            InitializeComponent();
            BindingComboBoxItemSource();
        }
        private void BindingComboBoxItemSource()
        {
            IEnumerable<Procedure> inputList = Db.QueryProcedureNotInRelationshipByProductId(SelectedProduct.Id);
            cmb_Input.ItemsSource = inputList;
            IEnumerable<Procedure> outputList = Db.QueryProcedureByProduct_Id(SelectedProduct.Id);
            cmb_Output.ItemsSource = outputList;
            txt_Ratio.Text = "1";
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            BindingComboBoxItemSource();
            txt_Ratio = null;
        }

        private void btn_AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_Input.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a input procedure.");
                return;
            }
            if (cmb_Output.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a output procedure.");
                return;
            }
            if (String.IsNullOrEmpty(txt_Ratio.Text.Trim()) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Ratio must not be blank!");
                return;
            }
            if((cmb_Input.SelectedItem as Procedure).Name == (cmb_Output.SelectedItem as Procedure).Name)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Input procedure and Output procedure can not be same!");
                return;
            }
            Relationship r = new Relationship();
            r.Product_Id = SelectedProduct.Id;
            r.InputProcedure = (cmb_Input.SelectedItem as Procedure).Name;
            r.OutputProcedure = (cmb_Output.SelectionBoxItem as Procedure).Name;
            try
            {
                r.Input_Output_Ratio = int.Parse(txt_Ratio.Text);
            }
            catch {
                SystemSounds.Beep.Play();
                MessageBox.Show("Ratio must be numberic!");
                return;
            }
            Db.InsertRelationship(r);
            PmPage.FillGridView_Relationship();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
