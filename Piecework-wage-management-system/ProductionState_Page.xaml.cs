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
    /// ProceduceRecord_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionState_Page : Page
    {
        private DataAccessLayer Db { set; get; }
        public ProductionState_Page()
        {
            InitializeComponent();
            Db = new DataAccessLayer();
            FillListMonth();
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
                MessageBox.Show("Wrong date :" + dateString);
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
                MessageBox.Show("Wrong date :" + dateString);
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
            foreach(ValuePrice vp in vpList)
            {
                List<Assign> aList = Db.QueryAssignByValuePrice(vp).ToList();
                foreach(Assign a in aList)
                {
                    ProcedureState ps = new ProcedureState();
                    ps.Count = Db.QueryReckonByAssignId(a.Id).Single().Count;
                    ps.EmployeeId = a.EmployeeId;
                    ps.EmployeeName = Db.QueryEmployeeByEID(a.EmployeeId).Single().Name;
                    ps.ProcedureName = Db.QueryProcedureById(vp.Procedure_Id).Single().Name;
                    ps.Ratio = Db.QueryRelationshipByInput(ps.ProcedureName).Single().Input_Output_Ratio;
                    ps.State = "";
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
            psList = psList;
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
    }
}
