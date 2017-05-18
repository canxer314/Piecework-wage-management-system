using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// PrintPayroll_Page.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPayroll_Page : System.Windows.Controls.Page
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
            for (int i = 0; i < months + 1; i++)
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
                MessageBox.Show("错误日期:" + dateString);
                return;
            }
            List<Value> valueList = new List<Value>();
            List<Assign> assginList = Db.QueryAssignByEmployeeId(SelectedEmployee.Id).ToList();
            List<wage> wageList = new List<wage>();
            foreach (Assign a in assginList)
            {
                ValuePrice p = Db.QueryValuePriceById(a.Price_Id).Single();
                Value v = Db.QueryValueById(p.Value_Id).Single();
                if (v.TaskDate > pickedDate && v.TaskDate < pickedDate.AddMonths(1))
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
            for (int i = 0; i < lv_Task.Items.Count; i++)
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
            foreach (wage w in wageList)
            {
                monthWage += w.Wage;
            }
            tb_MonthWage.Text = monthWage.ToString();
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Months.SelectedItems.Count != 1 ||
                grid_Employee.SelectedItems.Count != 1)
                return;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Procedure", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Price", typeof(double));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Wage", typeof(double));

            foreach (wage w in lv_Task.Items)
            {
                DataRow row = dt.NewRow();
                row["Date"] = w.Date.ToString("yyyy/MM/dd");
                row["Product"] = w.Product;
                row["Value"] = w.Value;
                row["Procedure"] = w.Procedure;
                row["Unit"] = w.Unit;
                row["Price"] = w.Price;
                row["Count"] = w.Count;
                row["Wage"] = w.Wage;
                dt.Rows.Add(row);
            }

            //创建Excel  

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);    //创建工作簿（WorkBook：即Excel文件主体本身）  
            Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];   //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出  

            //excelWS.Cells.NumberFormat = "@";     //  如果数据中存在数字类型 可以让它变文本格式显示  
            //将数据导入到工作表的单元格  

            excelWS.Cells[1, 1] = "日期";
            excelWS.Cells[1, 2] = "产品名称";
            excelWS.Cells[1, 3] = "感值";
            excelWS.Cells[1, 4] = "工序名称";
            excelWS.Cells[1, 5] = "计件单位";
            excelWS.Cells[1, 6] = "工序单价";
            excelWS.Cells[1, 7] = "计数";
            excelWS.Cells[1, 8] = "薪水";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();   //Excel单元格第一个从索引1开始  
                }
            }
            excelWS.Cells[dt.Rows.Count + 2, dt.Columns.Count - 1] = "月工资";
            excelWS.Cells[dt.Rows.Count + 2, dt.Columns.Count] = tb_MonthWage.Text;

            string savePath = "D:\\" + (grid_Employee.SelectedItem as Employee).Name + "_工资单_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".xlsx";
            excelWB.SaveAs(savePath);  //将其进行保存到指定的路径  
            excelWB.Close();

            SystemSounds.Beep.Play();
            MessageBox.Show("已保存为Excel文件：" + savePath);
            excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程  
        }
    }
}
