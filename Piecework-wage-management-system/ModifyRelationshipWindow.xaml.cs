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
        private Relationship OriginRelationship { set; get; }
        private IEnumerable<Procedure> procedureIEnum { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        private DataAccessLayer Db { set; get; }
        public ModifyRelationshipWindow(Relationship originRelationship, ProcedureManage_Page pmPage)
        {
            OriginRelationship = originRelationship;
            PmPage = pmPage;
            Db = new DataAccessLayer();
            procedureIEnum = Db.QueryProcedureByProduct_Id(originRelationship.Product_Id);
            InitializeComponent();
            //Db.DeleteRelationshipByName(originRelationship.Procedure_Name);
            RestoreOriginRelationship();
        }

        private void RestoreOriginRelationship()
        {
            tb_Input.Text = OriginRelationship.InputProcedure;
            List<Procedure> tmpList = Db.QueryProcedureByProduct_Id(OriginRelationship.Product_Id).ToList();
            //Procedure pNull = new Procedure();
            //pNull.Id = 0;
            //pNull.Name = "无";
            //pNull.Product_Id = tmpList.ElementAt(0).Product_Id;
            //tmpList.Add(pNull);
            int i = -1;
            foreach (var p in tmpList)
            {
                i++;
                if (p.Name == OriginRelationship.OutputProcedure)
                    break;
            }
            cmb_Output.ItemsSource = tmpList;
            cmb_Output.SelectedIndex = i;
            txt_Ratio.Text = OriginRelationship.Input_Output_Ratio.ToString();
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginRelationship();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Relationship alteredRelationship = new Relationship();
            //if(tb_Input.Text == (cmb_Output.SelectedItem as Procedure).Name)
            //{
            //    SystemSounds.Beep.Play();
            //    MessageBox.Show("前置工序与后置工序不能相同！");
            //    return;
            //}
            try
            {
                alteredRelationship.Input_Output_Ratio = int.Parse(txt_Ratio.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("投入产出比必须为数字！");
                return;
            }
            alteredRelationship.InputProcedure = OriginRelationship.InputProcedure;
            alteredRelationship.OutputProcedure = (cmb_Output.SelectedItem as Procedure).Name;
            alteredRelationship.Product_Id = OriginRelationship.Product_Id;
            Db.UpdateRelationship(alteredRelationship);
            PmPage.FillGridView_Relationship();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
