﻿using System;
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
                gridProcedure.ItemsSource = db.QueryProcedureByAll();
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
                gridProduct.ItemsSource = db.QueryProductById(int.Parse(txtSearchProduct.Text));
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
            AddProcedureWindow aProcWnd = new AddProcedureWindow(this,tmpList.ElementAt(0));
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
                gridProcedure.ItemsSource = db.QueryProcedureById(int.Parse(txtSearchProcedure.Text));
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please choose a search category!");
            }
        }

        private void AddValue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifyValue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveValue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchValue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddRelationship_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifyRelationship_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveRelationship_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchRelationship_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
