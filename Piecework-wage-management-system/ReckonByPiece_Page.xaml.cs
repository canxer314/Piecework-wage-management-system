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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// ReckonByPiece_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ReckonByPiece_Page : Page
    {
        private Employee LoginedEmployee { set; get; }
        private List<P_Price> priceList { set; get; }
        private DataAccessLayer Db { set; get; }
        public ReckonByPiece_Page(Employee employee)
        {
            Db = new DataAccessLayer();
            LoginedEmployee = employee;
            InitializeComponent();
            FillGridTask();
        }

        private void FillGridTask()
        {
            List<Assign> assignList = Db.QueryAssignByEmployeeId(LoginedEmployee.Id).ToList();
            List<Value> valueList = new List<Value>();
            bool isExist = false;
            foreach (Assign a in assignList)
            {
                isExist = false;
                int id = Db.QueryValuePriceById(a.Price_Id).Single().Value_Id;
                foreach (Value v in valueList)
                {
                    if (id == v.Id)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (isExist == false)
                    valueList.Add(Db.QueryValueById(id).Single());
            }
            gridTask.ItemsSource = valueList;
        }

        private void FillGridPrice()
        {
            if (gridTask.SelectedItems.Count != 1)
                return;
            Value v = gridTask.SelectedItem as Value;
            List<ValuePrice> vpList = Db.QueryValuePriceByValueId(v.Id).ToList();
            List<Assign> assignList = Db.QueryAssignByEmployeeId(LoginedEmployee.Id).ToList();
            priceList = new List<P_Price>();
            foreach (Assign a in assignList)
            {
                foreach (ValuePrice vp in vpList)
                {
                    if (a.Price_Id == vp.Id)
                    {
                        P_Price p = new P_Price();
                        p.AssignId = a.Id;
                        p.Price = vp.Unit_Price;
                        p.PriceId = vp.Id;
                        p.ProcedureName = Db.QueryProcedureById(vp.Procedure_Id).Single().Name;
                        p.Unit = vp.Unit;
                        p.IsSubmited = false;
                        priceList.Add(p);
                    }
                }
            }
            gridPrice.ItemsSource = priceList;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (gridPrice.SelectedItems.Count != 1)
                return;
            P_Price p = gridPrice.SelectedItem as P_Price;
            Reckon r = Db.QueryReckonByAssignId(p.AssignId).Single();
            try
            {
                r.Count = int.Parse(txt_Count.Text);
                if (r.Count < 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("计件数量不能小于零！");
                    return;
                }
                Db.UpdateReckonCount(r);
                p.IsSubmited = true;
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("提交失败！");
                return;
            }
        }

        private void gridTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridPrice();
        }

        private void gridPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridPrice.SelectedItems.Count != 1)
                return;
            P_Price p = gridPrice.SelectedItem as P_Price;
            Reckon r = Db.QueryReckonByAssignId(p.AssignId).Single();
            txt_Count.Text = r.Count.ToString();
        }
    }
}
