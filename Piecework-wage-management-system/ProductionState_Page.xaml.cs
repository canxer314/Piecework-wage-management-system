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
    /// ProceduceRecord_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionState_Page : System.Windows.Controls.Page
    {
        private DataAccessLayer Db { set; get; }
        public ProductionState_Page()
        {
            InitializeComponent();
            Db = new DataAccessLayer();
            FillListMonth();
            try
            {
                lst_Months.SelectedIndex = 0;
                lst_Days.SelectedIndex = 0;
                gridTask.SelectedIndex = 0;
                gridProcedure.SelectedIndex = 0;
            }
            catch { }
        }

        private void FillListMonth()
        {
            List<Value> valueList = Db.QueryValueByAll().ToList();
            if (valueList.Count == 0)
                return;
            DateTime beginTime = valueList.ElementAt(0).TaskDate;
            DateTime endTime = beginTime;
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
        private void FillListDays()
        {
            if (lst_Months.SelectedItems.Count != 1)
                return;
            lst_Days.Items.Clear();
            string dateString = lst_Months.SelectedItem.ToString();
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
            List<Value> valueList = Db.QueryValueByAll().ToList();
            if (valueList.Count == 0)
                return;
            foreach (Value v in valueList)
            {
                if (v.TaskDate > pickedDate && v.TaskDate < pickedDate.AddMonths(1))
                {
                    lst_Days.Items.Add(v.TaskDate.ToString("dd"));
                }
            }
        }

        private void FillGridTask()
        {
            if (lst_Days.SelectedItems.Count != 1)
                return;
            string dateString = lst_Months.SelectedItem.ToString() + "/" + lst_Days.SelectedItem.ToString();
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
            List<Value> valueList = Db.QueryValueByAll().ToList();
            if (valueList.Count == 0)
                return;
            List<Value> disValueList = new List<Value>();
            foreach (Value v in valueList)
            {
                if (v.TaskDate.ToString("yyyy/MM/dd") == pickedDate.ToString("yyyy/MM/dd"))
                    disValueList.Add(v);
            }
            gridTask.ItemsSource = disValueList;
        }

        private void FillGridProcedureState()
        {
            if (gridTask.SelectedItems.Count != 1)
                return;
            Value pickedTask = gridTask.SelectedItem as Value;
            List<ProcedureState> psList = new List<ProcedureState>();
            List<ValuePrice> vpList = Db.QueryValuePriceByValueId(pickedTask.Id).ToList();
            if (vpList.Count == 0)
                return;
            foreach (ValuePrice vp in vpList)
            {
                List<Assign> aList = Db.QueryAssignByValuePrice(vp).ToList();
                foreach (Assign a in aList)
                {
                    ProcedureState ps = new ProcedureState();
                    ps.Count = Db.QueryReckonByAssignId(a.Id).Single().Count;
                    ps.EmployeeId = a.EmployeeId;
                    ps.EmployeeName = Db.QueryEmployeeByEID(a.EmployeeId).Single().Name;
                    ps.ProcedureName = Db.QueryProcedureById(vp.Procedure_Id).Single().Name;
                    ps.Ratio = Db.QueryRelationshipByInput(ps.ProcedureName).Single().Input_Output_Ratio;
                    ps.State = "正常";
                    ps.ProcedureBehind = Db.QueryRelationshipByInput(ps.ProcedureName).Single().OutputProcedure;
                    psList.Add(ps);
                }
            }
            //process the state
            processState(ref psList);
            gridProcedure.ItemsSource = psList;
        }

        private void processState(ref List<ProcedureState> psList)
        {
            if (psList.Count == 0)
                return;
            foreach (ProcedureState ps in psList)
            {
                int outputCount = 0;
                int maxInputCount = -1;
                foreach (ProcedureState ps1 in psList)
                {
                    if (ps1.ProcedureName == ps.ProcedureName)
                        outputCount += ps1.Count;
                }
                foreach (ProcedureState ps2 in psList)
                {
                    //ps2 is the procedure in front of ps1
                    int inputCount = 0;
                    foreach (ProcedureState ps3 in psList)
                    {
                        //ps3 has the same procedure name with ps2
                        if (ps3.ProcedureName == ps2.ProcedureName)
                            inputCount += ps3.Count;
                    }
                    if (ps2.ProcedureBehind == ps.ProcedureName)
                    {
                        if (inputCount / ps2.Ratio < maxInputCount || maxInputCount < 0)
                        {
                            maxInputCount = inputCount / ps2.Ratio;
                        }
                    }
                }
                if (outputCount > maxInputCount && maxInputCount >= 0)
                {
                    ps.State = "超标！";
                }
            }
        }

        private void lst_Months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillListDays();
        }

        private void gridTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridProcedureState();
        }

        private void lst_Days_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridTask();
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Months.SelectedItems.Count != 1 ||
                lst_Days.SelectedItems.Count != 1 ||
                gridTask.SelectedItems.Count != 1)
                return;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Task", typeof(int));
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("ProcedureName", typeof(string));
            dt.Columns.Add("Ratio", typeof(int));
            dt.Columns.Add("EmployeeName", typeof(string));
            dt.Columns.Add("EmployeeId", typeof(int));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("ProcedureBehind", typeof(string));

            string dateString = lst_Months.SelectedItem.ToString() + "/" + lst_Days.SelectedItem.ToString();
            int task = (gridTask.SelectedItem as Value).TaskNum;
            string product = (gridTask.SelectedItem as Value).Product_Name;
            string value = (gridTask.SelectedItem as Value).Name;
            foreach (ProcedureState ps in gridProcedure.Items)
            {
                DataRow row = dt.NewRow();
                row["Date"] = dateString;
                row["Task"] = task;
                row["Product"] = product;
                row["Value"] = value;
                row["ProcedureName"] = ps.ProcedureName;
                row["Ratio"] = ps.Ratio;
                row["EmployeeName"] = ps.EmployeeName;
                row["EmployeeId"] = ps.EmployeeId;
                row["Count"] = ps.Count;
                row["State"] = ps.State;
                row["ProcedureBehind"] = ps.ProcedureBehind;
                dt.Rows.Add(row);
            }


            //创建Excel  

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);    //创建工作簿（WorkBook：即Excel文件主体本身）  
            Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];   //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出  

            //excelWS.Cells.NumberFormat = "@";     //  如果数据中存在数字类型 可以让它变文本格式显示  
            //将数据导入到工作表的单元格  

            excelWS.Cells[1, 1] = "日期";
            excelWS.Cells[1, 2] = "任务编号";
            excelWS.Cells[1, 3] = "产品名称";
            excelWS.Cells[1, 4] = "感值";
            excelWS.Cells[1, 5] = "工序名称";
            excelWS.Cells[1, 6] = "投入产出比";
            excelWS.Cells[1, 7] = "员工姓名";
            excelWS.Cells[1, 8] = "员工编号";
            excelWS.Cells[1, 9] = "计数";
            excelWS.Cells[1, 10] = "状态";
            excelWS.Cells[1, 11] = "后置工序";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();   //Excel单元格第一个从索引1开始  
                }
            }

            string savePath = "D:\\生产状况_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".xlsx";
            excelWB.SaveAs(savePath);  //将其进行保存到指定的路径  
            excelWB.Close();

            SystemSounds.Beep.Play();
            MessageBox.Show("已保存为Excel文件：" + savePath);
            excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程  
        }
    }
}
