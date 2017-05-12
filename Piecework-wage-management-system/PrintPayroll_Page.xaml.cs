using System;
using System.Collections.Generic;
using System.Linq;
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
    /// PrintPayroll_Page.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPayroll_Page : Page
    {
        private DataAccessLayer Db { set; get; }
        private Employee SelectedEmployee { set; get; }
        public PrintPayroll_Page()
        {
            InitializeComponent();
            Db = new DataAccessLayer();
            FillListEmployee();
        }

        private void grid_Employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid_Employee.SelectedItems.Count != 1)
                return;
            SelectedEmployee = grid_Employee.SelectedItem as Employee;
            lst_Months.Items.Clear();
            FillListBox_Months();
        }

        private void lst_Months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Months.SelectedItems.Count != 1)
                return;
            FillListView();
        }

        private void lv_Task_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateDayWage();
        }

        private void FillListBox_Months()
        {
            List<Value> valueList = new List<Value>();
            List<Assign> assginList = Db.QueryAssignByEmployeeId(SelectedEmployee.Id).ToList();
            foreach (Assign a in assginList)
            {
                ValuePrice price = Db.QueryValuePriceById(a.Price_Id).Single();
                valueList.Add(Db.QueryValueById(price.Value_Id).Single());
            }
            if (valueList.Count == 0)
                return;
            DateTime beginTime = valueList.ElementAt(0).TaskDate;
            DateTime endTime = valueList.ElementAt(0).TaskDate;
            foreach (Value v in valueList)
            {
                if (v.TaskDate < beginTime)
                {
                    beginTime = v.TaskDate;
                }
                endTime = v.TaskDate;
            }
            int months = (endTime.Year - beginTime.Year) * 12 + (endTime.Month - beginTime.Month);
            for(int i = 0; i<months + 1;i++)
            {
                lst_Months.Items.Add(beginTime.AddMonths(i).ToString("yyyy/MM"));
            }
        }

        private void FillListEmployee()
        {
            List<Employee> eList = Db.QueryEmployeeByAll().ToList();
            grid_Employee.ItemsSource = eList;
        }

        private void FillListView()
        {
            string dateString = lst_Months.SelectedItem as string;
            dateString += "/01";
            DateTime pickedDate = new DateTime();
            try
            {
                pickedDate = DateTime.Parse(dateString);
            }
            catch
            {
                MessageBox.Show("Wrong date :" + dateString);
                return;
            }
            List<Value> valueList = new List<Value>();
            List<Assign> assginList = Db.QueryAssignByEmployeeId(SelectedEmployee.Id).ToList();
            List<wage> wageList = new List<wage>();
            foreach (Assign a in assginList)
            {
                ValuePrice p = Db.QueryValuePriceById(a.Price_Id).Single();
                Value v = Db.QueryValueById(p.Value_Id).Single();
                if(v.TaskDate > pickedDate && v.TaskDate <pickedDate.AddMonths(1))
                {
                    wage w = new wage();
                    w.Count = Db.QueryReckonByAssignId(a.Id).Single().Count;
                    w.Date = v.TaskDate;
                    w.Price = p.Unit_Price;
                    w.Procedure = Db.QueryProcedureById(p.Procedure_Id).Single().Name;
                    w.Product = Db.QueryProductById(v.Product_Id).Single().Name;
                    w.Unit = p.Unit;
                    w.Value = v.Name;
                    w.Wage = w.Price * w.Count;
                    wageList.Add(w);
                }
            }
            lv_Task.ItemsSource = wageList;
            CalculateMonthWage(wageList);
        }

        private void CalculateDayWage()
        {
            if (lv_Task.SelectedItems.Count != 1)
                return;
            wage w = lv_Task.SelectedItem as wage;
            double dayWage = 0;
            for(int i = 0;i < lv_Task.Items.Count; i++)
            {
                wage wt = lv_Task.Items.GetItemAt(i) as wage;
                if (w.Date == wt.Date)
                    dayWage += wt.Wage;
            }
            tb_DayWage.Text = dayWage.ToString();
        }

        private void CalculateMonthWage(List<wage> wageList)
        {
            if (wageList.Count == 0)
                return;
            double monthWage = 0;
            foreach(wage w in wageList)
            {
                monthWage += w.Wage;
            }
            tb_MonthWage.Text = monthWage.ToString();
        }

    }
}
