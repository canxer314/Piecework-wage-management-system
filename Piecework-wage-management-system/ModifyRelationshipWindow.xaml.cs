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
            Db.DeleteRelationshipByName(originRelationship.Procedure_Name);
            RestoreOriginRelationship();
        }

        private void RestoreOriginRelationship()
        {
            List<int> sequenceList = new List<int>();
            int sequenceNumber = procedureIEnum.Count();
            for(int x = 1;x <= sequenceNumber;x++)
            {
                sequenceList.Add(x);
            }
            cmb_Sequence_Number.ItemsSource = sequenceList;
            int i = -1;
            foreach (var p in sequenceList)
            {
                i++;
                if (p == OriginRelationship.Sequence_Number)
                    break;
            }
            cmb_Sequence_Number.SelectedIndex = i;
            List<Procedure> tmpList = Db.QueryProcedureNotInRelationshipByProductId(OriginRelationship.Product_Id).ToList();
            cmb_Procedure_Name.ItemsSource = tmpList;
            i = -1;
            foreach (var p in tmpList)
            {
                i++;
                if (p.Name == OriginRelationship.Procedure_Name)
                    break;
            }
            cmb_Procedure_Name.SelectedIndex = i;
            txt_Ratio.Text = OriginRelationship.Input_Output_Ratio.ToString();
            IsModified = false;
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginRelationship();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            Relationship alteredRelationship = new Relationship();
            try
            {
                alteredRelationship.Input_Output_Ratio = int.Parse(txt_Ratio.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Input output ratio must be numberic!");
                return;
            }
            alteredRelationship.Procedure_Name = (cmb_Procedure_Name.SelectedItem as Procedure).Name;
            alteredRelationship.Sequence_Number = (int)cmb_Sequence_Number.SelectedItem;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (IsModified == false)
                Db.InsertRelationship(OriginRelationship);
        }
    }
}
