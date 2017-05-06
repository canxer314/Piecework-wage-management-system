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
    /// AddValueWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddValueWindow : Window
    {
        private Procedure SelectedProcedure { set; get; }
        private DataAccessLayer Db { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        public AddValueWindow(ProcedureManage_Page pmPage, Procedure p)
        {
            PmPage = pmPage;
            SelectedProcedure = p;
            Db = new DataAccessLayer();
            InitializeComponent();
            BindingComboBoxItemSource();
        }
        private void BindingComboBoxItemSource()
        {
            IEnumerable<Procedure> ps = Db.QueryProcedureByAll();
            cmb_Procedure.ItemsSource = ps;
            int i = -1;
            foreach (var p in ps)
            {
                i++;
                if (p.Name == SelectedProcedure.Name)
                    break;
            }
            cmb_Procedure.SelectedIndex = i;
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            BindingComboBoxItemSource();
            txt_ValueName.Text = null;
            txt_ValueUnit.Text = null;
            txt_ValueUnit_Price.Text = null;
        }

        private void btn_AddValue_Click(object sender, RoutedEventArgs e)
        {
            foreach (Value v in Db.QueryValueByAll())
            {
                if (txt_ValueName.Text == v.Name)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Value with Name:" + txt_ValueName.Text);
                    return;
                }
            }
            if (txt_ValueName == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Value Name can not be blank!");
                return;
            }
            if (txt_ValueUnit == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Value Unit can not be blank!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_ValueUnit_Price.Text) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Value Price can not be blank!");
                return;
            }
            if (cmb_Procedure.SelectedItem == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Belong to what Procedure can not be blank!");
                return;
            }
            double price = 0;
            try
            {
                price = double.Parse(txt_ValueUnit_Price.Text);
            }
            catch(FormatException except)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must only input numberic in Price!");
                return;
            }
            Value value = new Value();
            value.Name = txt_ValueName.Text;
            value.Unit = txt_ValueUnit.Text;
            value.Unit_Price = price;
            value.Procedure_Id = (cmb_Procedure.SelectedItem as Procedure).Id;
            Db.InsertValue(value);
            PmPage.FillGridView_Value();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
