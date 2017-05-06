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
    /// ModifyProcedureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyProcedureWindow : Window
    {
        private Procedure OriginProcedure { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        private DataAccessLayer Db { set; get; }
        public ModifyProcedureWindow(Procedure originProcedure, ProcedureManage_Page pmPage)
        {
            OriginProcedure = originProcedure;
            PmPage = pmPage;
            Db = new DataAccessLayer();
            InitializeComponent();
            RestoreOriginProcedure();
        }

        private void RestoreOriginProcedure()
        {
            txt_ProcedureName.Text = OriginProcedure.Name;
            txt_ProcedureId.Text = OriginProcedure.Id.ToString();
            IEnumerable<Product> product = Db.QueryProductByAll();
            cmb_Product.ItemsSource = product;
            int i = -1;
            foreach(var p in product)
            {
                i++;
                if (p.Id == OriginProcedure.Product_Id)
                    break;
            }
            cmb_Product.SelectedIndex = i;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginProcedure();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_ModifyProcedure_Click(object sender, RoutedEventArgs e)
        {
            Procedure alteredProcedure = new Procedure();
            if (txt_ProcedureId.Text != OriginProcedure.Id.ToString())
                if (Db.QueryProcedureById(int.Parse(txt_ProcedureId.Text)).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Procedure with ID:" + txt_ProcedureId.Text);
                    return;
                }
            if (txt_ProcedureName.Text != OriginProcedure.Name)
                if (Db.QueryProcedureByName(txt_ProcedureName.Text).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Procedure with name:" + txt_ProcedureName.Text);
                    return;
                }
            alteredProcedure.Name = txt_ProcedureName.Text;
            try
            {
            alteredProcedure.Id = int.Parse(txt_ProcedureId.Text);
            }
            catch
            {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Procedure Id must be numberic!");
                    return;
            }
            alteredProcedure.Product_Id = (cmb_Product.SelectedItem as Product).Id;
            Db.DeleteProcedureById(OriginProcedure.Id);
            Db.InsertProcedure(alteredProcedure);
            PmPage.FillGridView_Procedure();
            this.Close();
        }
    }
}
