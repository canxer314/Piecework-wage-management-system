﻿using System;
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
using System.Windows.Shapes;

namespace Piecework_wage_management_system
{
    /// <summary>
    /// EmployeeLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeLoginWindow : Window
    {
        private Window fatherWnd { set; get; }
        public static bool IsLogin { get; set; }
        public EmployeeLoginWindow(Window wnd)
        {
            fatherWnd = wnd;
            IsLogin = false;
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //processing with the database
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            foreach (Employee employee in dataAccessLayer.QueryEmployeeByAll())
                if (txtID.Text.ToString() == employee.Id.ToString() && passwordBox.Password == employee.Password)
                {
                    IsLogin = true;
                    EmployeeWindow reckonByThePieceWindow = new EmployeeWindow(employee);
                    reckonByThePieceWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    reckonByThePieceWindow.Show();
                    fatherWnd.Close();
                    this.Close();
                }
            if (IsLogin == false)
            {
                MessageBox.Show("帐号或密码错误！");
                passwordBox.Password = null;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = false;
            this.Close();
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin_Click(sender, e);
        }
    }
}
