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
            IEnumerable<Procedure> procedureIEnum = Db.QueryProcedureByProduct_Id(SelectedProduct.Id);
            //cmb_OutputProcedure.ItemsSource = procedureIEnum;
            List<int> sequenceList = new List<int>();
            int sequenceNumber = procedureIEnum.Count();
            for(int x = 1;x <= sequenceNumber;x++)
            {
                sequenceList.Add(x);
            }
            cmb_Sequence_Number.ItemsSource = sequenceList;

            IEnumerable<Procedure> tmpList = Db.QueryProcedureNotInRelationshipByProductId(SelectedProduct.Id);
            cmb_Procedure_Name.ItemsSource = tmpList;
            cmb_Procedure_Name.SelectedIndex = -1;
            cmb_Sequence_Number.SelectedIndex = -1;
            txt_Ratio.Text = "1";
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            BindingComboBoxItemSource();
            txt_Ratio = null;
        }

        private void btn_AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            foreach (Relationship relate in Db.QueryRelationshipByAll())
            {
                if ((cmb_Procedure_Name.SelectedItem as Procedure).Name == relate.Procedure_Name)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Relation with Procedure Name:" + (cmb_Procedure_Name.SelectedItem as Procedure).Name);
                    return;
                }
            }
            if (cmb_Procedure_Name.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a procedure.");
                return;
            }
            if (cmb_Sequence_Number.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a sequence number.");
                return;
            }
            if (String.IsNullOrEmpty(txt_Ratio.Text.Trim()) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Ratio must not be blank!");
                return;
            }
            Relationship r = new Relationship();
            r.Product_Id = SelectedProduct.Id;
            r.Procedure_Name = (cmb_Procedure_Name.SelectedItem as Procedure).Name;
            r.Sequence_Number = (int)cmb_Sequence_Number.SelectionBoxItem;
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
