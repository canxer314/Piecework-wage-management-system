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
        //public void FillGridView_Value()
        //{
        //    try
        //    {
        //        if (gridProcedure.SelectedItems.Count != 1)
        //        {
        //            //gridValue.ItemsSource = db.QueryValueByAll();
        //            return;
        //        }
        //        IEnumerable<Procedure> tmpList;
        //        tmpList = db.QueryProcedureById((gridProcedure.SelectedItem as Procedure).Id);
        //        gridValue.ItemsSource = db.QueryValueByProcedureId(tmpList.ElementAt(0).Id);
        //    }
        //    catch
        //    {

        //    }
        //}
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
                MessageBox.Show("You must first select a Product in the table before you modify it.");
                return;
            }
            if (gridProduct.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify multiple Product information at one time.");
                return;
            }
            IEnumerable<Product> tmpList;
            tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
            if(db.QueryProcedureByProduct_Id(tmpList.ElementAt(0).Id).Count()!=0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify Product which have procedures under it.");
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
                MessageBox.Show("You must first select at least one Product in the table before you remove it.");
                return;
            }
            foreach (Product item in gridProduct.SelectedItems)
            {
                db.DeleteProductById(item.Id);
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
                    MessageBox.Show("You must only input numberic when search by id!");
                    return;
                }
                    gridProduct.ItemsSource = db.QueryProductById(i);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please choose a search category!");
            }
        }

        private void AddProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a Product before you add Procedure on it.");
                return;
            }
            if (gridProduct.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not add Procedure on multiple Product at one time.");
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
                MessageBox.Show("You must first select a Procedure in the table before you modify it.");
                return;
            }
            if (gridProcedure.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify multiple Procedure information at one time.");
                return;
            }
            IEnumerable<Procedure> tmpList;
            tmpList = db.QueryProcedureById((gridProcedure.SelectedItem as Procedure).Id);
            if(db.QueryRelationshipByName(tmpList.ElementAt(0).Name).Count()!=0)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify Procedure which have relations under it.");
                return;
            }
            ModifyProcedureWindow modifyProcedureWnd = new ModifyProcedureWindow(tmpList.ElementAt(0), this);
            modifyProcedureWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyProcedureWnd.ShowDialog();
        }

        private void RemoveProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedure.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select at least one Procedure in the table before you remove it.");
                return;
            }
            foreach (Procedure item in gridProcedure.SelectedItems)
            {
                db.DeleteProcedureById(item.Id);
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
                catch(FormatException ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("You must only input numberic when search by id!");
                    return;
                }
                    gridProcedure.ItemsSource = db.QueryProcedureById(i);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please choose a search category!");
            }
        }

        //private void AddValue_Click(object sender, RoutedEventArgs e)
        //{
        //    if (gridProcedure.SelectedItems.Count < 1)
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("You must first select a Procedure before you add Value on it.");
        //        return;
        //    }
        //    if (gridProcedure.SelectedItems.Count > 1)
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("Can not add Value on multiple Procedure at one time.");
        //        return;
        //    }
        //    IEnumerable<Procedure> tmpList;
        //    tmpList = db.QueryProcedureById((gridProcedure.SelectedItem as Procedure).Id);
        //    AddValueWindow aVWnd = new AddValueWindow(this, tmpList.ElementAt(0));
        //    aVWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    aVWnd.ShowDialog();
        //}

        //private void ModifyValue_Click(object sender, RoutedEventArgs e)
        //{
        //    if (gridValue.SelectedItems.Count < 1)
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("You must first select a Value in the table before you modify it.");
        //        return;
        //    }
        //    if (gridValue.SelectedItems.Count > 1)
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("Can not modify multiple Value information at one time.");
        //        return;
        //    }
        //    IEnumerable<Value> tmpList;
        //    tmpList = db.QueryValueByName((gridValue.SelectedItem as Value).Name);
        //    IEnumerable<Product> tmpList2;
        //    tmpList2 = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
        //    ModifyValueWindow modifyValueWnd = new ModifyValueWindow(tmpList.ElementAt(0),tmpList2.ElementAt(0), this);
        //    modifyValueWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    modifyValueWnd.ShowDialog();
        //}

        //private void RemoveValue_Click(object sender, RoutedEventArgs e)
        //{
        //    if (gridValue.SelectedItems.Count < 1)
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("You must first select at least one Value in the table before you remove it.");
        //        return;
        //    }
        //    foreach (Value item in gridValue.SelectedItems)
        //    {
        //        db.DeleteValueByName(item.Name);
        //    }
        //    FillGridView_Value();
        //}

        //private void btnSearchValue_Click(object sender, RoutedEventArgs e)
        //{
        //    if (String.IsNullOrEmpty(txtSearchValue.Text.Trim()) == true)
        //    {
        //        gridValue.ItemsSource = db.QueryValueByAll();
        //        return;
        //    }
        //    if (rbtnValueName.IsChecked == true)
        //    {
        //        gridValue.ItemsSource = db.QueryValueByName(txtSearchValue.Text);
        //    }
        //    else if (rbtnValueUnit.IsChecked == true)
        //    {
        //        gridValue.ItemsSource = db.QueryValueByUnit(txtSearchValue.Text);
        //    }
        //    else if (rbtnValuePrice.IsChecked == true)
        //    {
        //        double queryValue = 0;
        //        try
        //        {
        //            queryValue = double.Parse(txtSearchValue.Text);
        //        }
        //        catch(FormatException ex)
        //        {
        //            SystemSounds.Beep.Play();
        //            MessageBox.Show("You must only input numberic when search by Price!");
        //            return;
        //        }
        //            gridValue.ItemsSource = db.QueryValueByUnitPrice(queryValue);
        //    }
        //    else
        //    {
        //        SystemSounds.Beep.Play();
        //        MessageBox.Show("Please choose a search category!");
        //    }
        //}

        private void AddRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProduct.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a Product before you add Relation on it.");
                return;
            }
            if (gridProduct.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not add Relation on multiple Product at one time.");
                return;
            }
            IEnumerable<Product> tmpList = db.QueryProductById((gridProduct.SelectedItem as Product).Id);
            AddRelationshipWindow aRwnd = new AddRelationshipWindow(this, tmpList.ElementAt(0));
            aRwnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            aRwnd.ShowDialog();
        }

        private void ModifyRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedureRelationship.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select a Relationship in the table before you modify it.");
                return;
            }
            if (gridProcedureRelationship.SelectedItems.Count > 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Can not modify multiple Relationship information at one time.");
                return;
            }
            IEnumerable<Relationship> tmpList;
            tmpList = db.QueryRelationshipByName((gridProcedureRelationship.SelectedItem as Relationship).Procedure_Name);
            ModifyRelationshipWindow modifyRelationshipWnd = new ModifyRelationshipWindow(tmpList.ElementAt(0),this);
            modifyRelationshipWnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            modifyRelationshipWnd.ShowDialog();

        }

        private void RemoveRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcedureRelationship.SelectedItems.Count < 1)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("You must first select at least one Relationship in the table before you remove it.");
                return;
            }
            foreach (Relationship item in gridProcedureRelationship.SelectedItems)
            {
                db.DeleteRelationshipByName(item.Procedure_Name);
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
            if (rbtnSequence.IsChecked == true)
            {
                int i = 0;
                try
                {
                    i = int.Parse(txtSearchRelationship.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("You must only input numberic when search by Sequence!");
                    return;
                }
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipBySequence_Number(i);
            }
            else if (rbtnProcedure.IsChecked == true)
            {
                gridProcedureRelationship.ItemsSource = db.QueryRelationshipByName(txtSearchRelationship.Text);
            }
            else if (rbtnRatio.IsChecked == true)
            {
                int query  = 0;
                try
                {
                    query = int.Parse(txtSearchRelationship.Text);
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("You must only input numberic when search by Ratio!");
                    return;
                }
                    gridProcedureRelationship.ItemsSource = db.QueryRelationshipByRatio(query);
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please choose a search category!");
            }
        }

        private void gridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGridView_Procedure();
            FillGridView_Relationship();
        }

        //private void gridProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    FillGridView_Value();
        //}

        private void txtSearchRelationship_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
