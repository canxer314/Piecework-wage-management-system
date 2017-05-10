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
    /// AssignEmployeeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AssignEmployeeWindow : Window
    {
        private DataAccessLayer Db { set; get; }
        private ProductionScheduling_Page PsPage { set; get; }
        private ValuePrice SelectedPrice { set; get; }
        public AssignEmployeeWindow(ProductionScheduling_Page psPage, ValuePrice selectedPrice)
        {
            PsPage = psPage;
            SelectedPrice = selectedPrice;
            Db = new DataAccessLayer();
            InitializeComponent();
            BindingComboBoxSource();
        }

        private void BindingComboBoxSource()
        {
            txt_Procedure.Text = (Db.QueryProcedureById(SelectedPrice.Procedure_Id)).Single().Name;
            Value value = Db.QueryValueById(SelectedPrice.Value_Id).Single();
            txt_Product.Text = (Db.QueryProductById(value.Product_Id)).Single().Name;
            txt_Value.Text = (Db.QueryValueById(SelectedPrice.Value_Id)).Single().Name;
            txt_SearchBox.Text = string.Empty;
            cmb_Employee.ItemsSource = Db.QueryEmployeeByAll();
        }

        private void btn_AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            Assign a = new Assign();
            a.EmployeeId = (cmb_Employee.SelectedItem as Employee).Id;
            a.Price_Id = SelectedPrice.Id;
            if (Db.QueryAssignWhetherExsit(a).Count() > 0)
            {
                SystemSounds.Beep.Play();
                string str = @"Already assign this employee to the procedure!
Please choose another one!";
                MessageBox.Show(str);
                return;
            }
            Db.InsertAssign(a);
            Assign a0 = Db.QueryAssignWhetherExsit(a).Single();
            Reckon r = new Reckon();
            r.Assign_Id = a0.Id;
            r.Count = 0;
            Db.InsertReckon(r);
            PsPage.FillEmployee();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_SearchByEmployeeId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(txt_SearchBox.Text);
                int i = 0;
                for (i = 0; i < cmb_Employee.Items.Count; i++)
                {
                    if ((cmb_Employee.Items.GetItemAt(i) as Employee).Id == id)
                    {
                        cmb_Employee.SelectedIndex = i;
                        return;
                    }
                }
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not found Employee with Id:" + id + "!");
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please input a numberic!");
            }
        }

        private void btn_SearchByEmployeeName_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < cmb_Employee.Items.Count; i++)
            {
                if ((cmb_Employee.Items.GetItemAt(i) as Employee).Name == txt_SearchBox.Text)
                {
                    cmb_Employee.SelectedIndex = i;
                    return;
                }
            }
            SystemSounds.Beep.Play();
            MessageBox.Show("Can not found Employee with name:" + txt_SearchBox.Text + "!");
            return;
        }

    }
}
