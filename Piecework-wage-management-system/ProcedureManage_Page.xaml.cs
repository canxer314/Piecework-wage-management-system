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
    /// ProcedureManage_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProcedureManage_Page : Page
    {
        public DataAccessLayer db { set; get; }
        public ProcedureManage_Page()
        {
            db = new DataAccessLayer();
            InitializeComponent();
            FillGridView_Product();
        }

        public void FillGridView_Product()
        {
            try
            {
                gridProduct.ItemsSource = db.QueryProductByAll();
            }
            catch
            {

            }
        }
        public void FillGridView_Procedure()
        {
            try
            {
                if (gridProduct.SelectedItems.Count != 1)
                {
                    //gridProcedure.ItemsSource = db.QueryProcedureByAll();
                    return;
                }
                IEnumerable<Product> tmpList;
                tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
                gridProcedure.ItemsSource = db.QueryProcedureByProduct_Id(tmpList.ElementAt(0).Id);
            }
            catch
            {

            }
        }
        public void FillGridView_Relationship()
        {
            try
            {
                if (gridProduct.SelectedItems.Count != 1)
                {
                    gridProcedureRelationship.ItemsSource = null;
                    return;
                }
                IEnumerable<Product> tmpList;
                tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByProduct_Id(tmpList.ElementAt(0).Id);
            }
            catch
            {

            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow aProtWnd = new AddProductWindow(this);
            aProtWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aProtWnd.ShowDialog();
        }

        private void ModifyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择产品！");
                return;
            }
            if (gridProduct.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法一次更改多个产品信息！");
                return;
            }
            IEnumerable<Product> tmpList;
            tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
            if (db.QueryProcedureByProduct_Id(tmpList.ElementAt(0).Id).Count() != 0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法更改已分配工序的产品信息！");
                return;
            }
            ModifyProductWindow modifyProductWnd = new ModifyProductWindow(tmpList.ElementAt(0), this);
            modifyProductWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyProductWnd.ShowDialog();
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择产品！");
                return;
            }
            foreach (Product item in gridProduct.SelectedItems)
            {
                try
                {
                    db.DeleteProductById(item.Id);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("删除失败！");
                    FillGridView_Product();
                    return;
                }
            }
            FillGridView_Product();
        }

        private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearchProduct.Text.Trim()) == true)
            {
                gridProduct.ItemsSource = db.QueryProductByAll();
                return;
            }
            if (rbtnProductName.IsChecked == true)
            {
                gridProduct.ItemsSource = db.QueryProductByName(txtSearchProduct.Text);
            }
            else if (rbtnProductId.IsChecked == true)
            {
                int i = 0;
                try
                {
                    i = int.Parse(txtSearchProduct.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("当您选择以产品编号为搜索条件时请在搜索栏里输入数字！");
                    return;
                }
                gridProduct.ItemsSource = db.QueryProductById(i);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择搜索条件！");
            }
        }

        private void AddProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count != 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择一项产品！");
                return;
            }
            IEnumerable<Product> tmpList;
            tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
            AddProcedureWindow aProcWnd = new AddProcedureWindow(this, tmpList.ElementAt(0));
            aProcWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aProcWnd.ShowDialog();
        }

        private void ModifyProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedure.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择工序！");
                return;
            }
            if (gridProcedure.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法一次更改多个工序信息！");
                return;
            }
            Procedure p = db.QueryProcedureById((gridProcedure.SelectedItem as Procedure).Id).Single();
            ModifyProcedureWindow modifyProcedureWnd = new ModifyProcedureWindow(p, this);
            modifyProcedureWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyProcedureWnd.ShowDialog();
        }

        private void RemoveProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedure.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择想要删除的工序！");
                return;
            }
            foreach (Procedure item in gridProcedure.SelectedItems)
            {
                try
                {
                    db.DeleteProcedureById(item.Id);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("删除失败！");
                    FillGridView_Procedure();
                    return;
                }
            }
            FillGridView_Procedure();
        }

        private void btnSearchProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearchProcedure.Text.Trim()) == true)
            {
                gridProcedure.ItemsSource = db.QueryProcedureByAll();
                return;
            }
            if (rbtnProcedureName.IsChecked == true)
            {
                gridProcedure.ItemsSource = db.QueryProcedureByName(txtSearchProcedure.Text);
            }
            else if (rbtnProcedureId.IsChecked == true)
            {
                int i = 0;
                try
                {
                    i = int.Parse(txtSearchProcedure.Text);
                }
                catch (FormatException ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("当您选择以工序编号为搜索条件时请在搜索栏里输入数字！");
                    return;
                }
                gridProcedure.ItemsSource = db.QueryProcedureById(i);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择搜索条件！");
            }
        }

        private void AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count != 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择一个产品！");
                return;
            }
            Product p = db.QueryProductById((gridProduct.SelectedItem as Product).Id).Single();
            AddRelationshipWindow aRwnd = new AddRelationshipWindow(this, p);
            aRwnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aRwnd.ShowDialog();
        }

        private void ModifyRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedureRelationship.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选选择一个工序关系！");
                return;
            }
            if (gridProcedureRelationship.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("无法一次修改多个工序关系！");
                return;
            }
            Relationship r = db.QueryRelationshipByInput((gridProcedureRelationship.SelectedItem as Relationship).InputProcedure).Single();
            ModifyRelationshipWindow modifyRelationshipWnd = new ModifyRelationshipWindow(r, this);
            modifyRelationshipWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyRelationshipWnd.ShowDialog();

        }

        private void RemoveRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedureRelationship.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请先选择想要删除的工序关系！");
                return;
            }
            foreach (Relationship item in gridProcedureRelationship.SelectedItems)
            {
                try
                {
                    db.DeleteRelationshipByName(item.InputProcedure);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("删除失败！");
                    return;
                }
            }
            FillGridView_Relationship();
        }

        private void btnSearchRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearchRelationship.Text.Trim()) == true)
            {
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByAll();
                return;
            }
            if (rbtnInput.IsChecked == true)
            {
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByInput(txtSearchRelationship.Text);
            }
            else if (rbtnOutput.IsChecked == true)
            {
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByOutput(txtSearchRelationship.Text);
            }
            else if (rbtnRatio.IsChecked == true)
            {
                int query = 0;
                try
                {
                    query = int.Parse(txtSearchRelationship.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("当您选择以投入产出比为搜索条件时请在搜索栏里输入数字！");
                    return;
                }
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByRatio(query);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("请选择搜索条件！");
            }
        }

        private void gridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridView_Procedure();
            FillGridView_Relationship();
        }

        private void txtSearchRelationship_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
