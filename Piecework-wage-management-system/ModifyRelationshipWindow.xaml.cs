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
    /// ModifyRelationshipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyRelationshipWindow : Window
    {
        private bool IsModified { set; get; }
        private Relationship OriginRelationship { set; get; }
        private IEnumerable<Procedure> procedureIEnum { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        private DataAccessLayer Db { set; get; }
        public ModifyRelationshipWindow(Relationship originRelationship, ProcedureManage_Page pmPage)
        {
            IsModified = false;
            OriginRelationship = originRelationship;
            PmPage = pmPage;
            Db = new DataAccessLayer();
            procedureIEnum = Db.QueryProcedureByProduct_Id(originRelationship.Product_Id);
            InitializeComponent();
            Db.DeleteRelationshipByInputProcedure(originRelationship.InputProcedure);
            RestoreOriginRelationship();
        }

        private void RestoreOriginRelationship()
        {
            cmb_OutputProcedure.ItemsSource = procedureIEnum;
            int i = -1;
            foreach (var p in procedureIEnum)
            {
                i++;
                if (p.Name == OriginRelationship.OutputProcedure)
                    break;
            }
            cmb_OutputProcedure.SelectedIndex = i;
            List<Procedure> tmpList = Db.QueryProcedureNotInRelationshipByProductId(OriginRelationship.Product_Id).ToList();
            //i = 0;
            //foreach (var p in tmpList)
            //{
            //    if (p.Name != OriginRelationship.InputProcedure)
            //        i++;
            //    else
            //    {
            //        tmpList.RemoveAt(i);
            //        break;
            //    }
            //}
            cmb_InputProcedure.ItemsSource = tmpList;
            i = -1;
            foreach (var p in tmpList)
            {
                    i++;
                if (p.Name == OriginRelationship.InputProcedure)
                    break;
            }
            cmb_InputProcedure.SelectedIndex = i;
            IsModified = false;
        }
        private void BindingComboBoxItemSourceDynamic()
        {
            //List<Procedure> tmpList = Db.QueryProcedureNotInRelationshipByProductId(OriginRelationship.Product_Id).ToList();
            //int i = 0;
            //foreach (var p in tmpList)
            //{
            //    if (p.Id != (cmb_OutputProcedure.SelectedItem as Procedure).Id)
            //        i++;
            //    else
            //    {
            //        tmpList.RemoveAt(i);
            //        break;
            //    }
            //}
            //cmb_InputProcedure.ItemsSource = tmpList;
            cmb_InputProcedure.SelectedIndex = -1;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginRelationship();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Relationship alteredRelationship = new Relationship();
            alteredRelationship.Scale = int.Parse(txt_Scale.Text);
            alteredRelationship.InputProcedure = (cmb_InputProcedure.SelectedItem as Procedure).Name;
            alteredRelationship.OutputProcedure = (cmb_OutputProcedure.SelectedItem as Procedure).Name;
            alteredRelationship.Product_Id = OriginRelationship.Product_Id;
            Db.InsertRelationship(alteredRelationship);
            PmPage.FillGridView_Relationship();
            IsModified = true;
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            IsModified = false;
            this.Close();
        }

        private void cmb_OutputProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingComboBoxItemSourceDynamic();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (IsModified == false)
                Db.InsertRelationship(OriginRelationship);
        }
    }
}
