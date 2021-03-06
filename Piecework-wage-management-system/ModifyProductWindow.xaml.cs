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
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// ModifyProductWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyProductWindow : Window
    {
        private Product OriginProduct { set; get; }
        private ProcedureManage_Page pmPage { set; get; }
        public ModifyProductWindow(Product p, ProcedureManage_Page pmPage)
        {
            this.pmPage = pmPage;
            OriginProduct = p;
            InitializeComponent();
            RestoreOriginProduct();
        }

        private void RestoreOriginProduct()
        {
            txt_ProductName.Text = OriginProduct.Name;
            txt_ProductId.Text = OriginProduct.Id.ToString();
        }
        private void btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            RestoreOriginProduct();
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer db = new DataAccessLayer();
            Product modifiedProduct = new Product();
            if (txt_ProductName.Text != OriginProduct.Name)
                if (db.QueryProductByName(txt_ProductName.Text).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("已存在产品名称： " + txt_ProductName.Text + "!");
                    return;
                }
            if (txt_ProductId.Text != OriginProduct.Id.ToString())
                if (db.QueryProductById(int.Parse(txt_ProductId.Text)).Count() > 0)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("已存在产品编号：" + txt_ProductId.Text + "!");
                    return;
                }
            modifiedProduct.Name = txt_ProductName.Text;
            try
            {
                modifiedProduct.Id = int.Parse(txt_ProductId.Text);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("产品编号必须为数字！");
                return;
            }
            try
            {

                db.DeleteProductById(OriginProduct.Id);
            }
            catch
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("更改失败！");
                return;
            }
            db.InsertProduct(modifiedProduct);
            pmPage.FillGridView_Product();
            this.Close();
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
