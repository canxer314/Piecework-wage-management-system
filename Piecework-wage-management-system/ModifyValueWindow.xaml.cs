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
    /// ModifyValueWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyValueWindow : Window
    {
        private Value OriginValue { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        private DataAccessLayer Db { set; get; }
        public ModifyValueWindow(Value originValue, ProcedureManage_Page pmPage)
        {
            OriginValue = originValue;
            PmPage = pmPage;
            Db = new DataAccessLayer();
            InitializeComponent();
            RestoreOriginValue();
        }

        private void RestoreOriginValue()
        {
            txt_ValueName.Text = OriginValue.Name;
            txt_ValueUnit.Text = OriginValue.Unit;
            txt_ValueUnitPrice.Text = OriginValue.Unit_Price.ToString();
            IEnumerable<Procedure> procedureList = Db.QueryProcedureById(OriginValue.Procedure_Id);
            IEnumerable<Product> productList = Db.QueryProductById(procedureList.ElementAt(0).Product_Id);
            IEnumerable<Procedure> procedure = Db.QueryProcedureByProduct_Id(productList.ElementAt(0).Id);
            cmb_Procedure.ItemsSource = procedure;
            int i = -1;
            foreach (var p in procedure)
            {
                i++;
                if (p.Id == OriginValue.Procedure_Id)
                    break;
            }
            cmb_Procedure.SelectedIndex = i;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginValue();
        }

        private void btn_ModifyValue_Click(object sender, RoutedEventArgs e)
        {
            Value alteredValue = new Value();
            if (txt_ValueName.Text != OriginValue.Name)
                if (Db.QueryValueByName(txt_ValueName.Text).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Value with name:" + txt_ValueName.Text);
                    return;
                }
            if (String.IsNullOrEmpty(txt_ValueName.Text.Trim()) == true ||
                String.IsNullOrEmpty(txt_ValueUnit.Text.Trim()) == true ||
                    String.IsNullOrEmpty(txt_ValueUnitPrice.Text.Trim()) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("There must no be blank in any textbox.");
                return;
            }
            alteredValue.Name = txt_ValueName.Text;
            alteredValue.Unit = txt_ValueUnit.Text;
            alteredValue.Unit_Price = int.Parse(txt_ValueUnitPrice.Text);
            alteredValue.Procedure_Id = (cmb_Procedure.SelectedItem as Procedure).Id;
            Db.DeleteValueByName(OriginValue.Name);
            Db.InsertValue(alteredValue);
            PmPage.FillGridView_Value();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
