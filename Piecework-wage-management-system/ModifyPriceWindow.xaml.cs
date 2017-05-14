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
    /// ModifyPriceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyPriceWindow : Window
    {
        private DataAccessLayer Db { set; get; }
        private ProductionScheduling_Page PsPage { set; get; }
        private ValuePrice OriginPrice { set; get; }
        public ModifyPriceWindow(ProductionScheduling_Page psPage, ValuePrice selectedPrice)
        {
            InitializeComponent();
            PsPage = psPage;
            OriginPrice = selectedPrice;
            Db = new DataAccessLayer();
            RestoreOriginPrice();
        }

        private void RestoreOriginPrice()
        {
            txt_ProductName.Text = (Db.QueryValueById(OriginPrice.Value_Id).Single()).Product_Name;
            txt_ValueName.Text = Db.QueryValueById(OriginPrice.Value_Id).Single().Name;
            txt_ProcedureName.Text = Db.QueryProcedureById(OriginPrice.Procedure_Id).Single().Name;
            txt_Price.Text = OriginPrice.Unit_Price.ToString();
            txt_Unit.Text = OriginPrice.Unit;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            RestoreOriginPrice();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            ValuePrice modifiedPrice = new ValuePrice();
            try
            {
                modifiedPrice.Unit_Price = double.Parse(txt_Price.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("The Price must be numberic!");
                return;
            }
            modifiedPrice.Id = OriginPrice.Id;
            modifiedPrice.Procedure_Id = OriginPrice.Procedure_Id;
            modifiedPrice.Value_Id = OriginPrice.Value_Id;
            modifiedPrice.Unit = txt_Unit.Text;
            Db.UpdatePrice(modifiedPrice);
            PsPage.FillPrice();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
