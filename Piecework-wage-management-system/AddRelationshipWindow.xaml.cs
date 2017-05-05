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
        private Procedure SelectedProcedure { set; get; }
        private IEnumerable<Procedure> procedureIEnum { set; get; }
        private List<Procedure> tmpList { set; get; }
        private DataAccessLayer Db { set; get; }
        private ProcedureManage_Page PmPage { set; get; }
        public AddRelationshipWindow(ProcedureManage_Page pmPage, Procedure procedure)
        {
            PmPage = pmPage;
            SelectedProcedure = procedure;
            Db = new DataAccessLayer();
            procedureIEnum = Db.QueryProcedureByProduct_Id(procedure.Product_Id);
            tmpList = Db.QueryProcedureNotInRelationshipByProductId(SelectedProcedure.Product_Id).ToList();
            InitializeComponent();
            BindingComboBoxItemSource();
        }
        private void BindingComboBoxItemSource()
        {
            cmb_OutputProcedure.ItemsSource = procedureIEnum;
            int i = -1;
            foreach (var p in procedureIEnum)
            {
                i++;
                if (p.Id == SelectedProcedure.Id)
                    break;
            }
            cmb_OutputProcedure.SelectedIndex = i;
            i = 0;
            foreach (var p in tmpList)
            {
                if (p.Id != SelectedProcedure.Id)
                    i++;
                else
                {
                    tmpList.RemoveAt(i);
                    break;
                }
            }
            cmb_InputProcedure.ItemsSource = tmpList;
        }
        private void BindingComboBoxItemSourceDynamic()
        {
            int i = 0;
            foreach (var p in tmpList)
            {
                if (p.Id != (cmb_OutputProcedure.SelectedItem as Procedure).Id)
                    i++;
                else
                {
                    tmpList.RemoveAt(i);
                    break;
                }
            }
            cmb_InputProcedure.ItemsSource = tmpList;
            cmb_InputProcedure.SelectedIndex = -1;
        }
        private void btn_Clean_Click(object sender, RoutedEventArgs e)
        {
            BindingComboBoxItemSource();
            txt_Scale.Text = null;
        }

        private void btn_AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            foreach (Relationship relate in Db.QueryRelationshipByAll())
            {
                if ((cmb_InputProcedure.SelectedItem as Procedure).Name == relate.InputProcedure)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Already exists Relationship with inputProcedure:" + (cmb_InputProcedure.SelectedItem as Procedure).Name);
                    return;
                }
            }
            if (cmb_InputProcedure.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a input procedure.");
                return;
            }
            if (cmb_OutputProcedure.SelectedIndex == -1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must select a output procedure.");
                return;
            }
            if (String.IsNullOrEmpty(txt_Scale.Text.Trim()) == true)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Scale must not be blank!");
                return;
            }
            Relationship r = new Relationship();
            r.InputProcedure = (cmb_InputProcedure.SelectedItem as Procedure).Name;
            r.OutputProcedure = (cmb_OutputProcedure.SelectedItem as Procedure).Name;
            try
            {
                r.Scale = int.Parse(txt_Scale.Text);
            }
            catch { }
            r.Product_Id = SelectedProcedure.Product_Id;
            Db.InsertRelationship(r);
            PmPage.FillGridView_Relationship();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmb_OutputProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingComboBoxItemSourceDynamic();
        }
    }
}
