using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for AdminPanelWindow.xaml
    /// </summary>
    public partial class UsersPanelWindow : Window
    {
        private bool IsMaximized = false;
        public UsersPanelWindow()
        {
            InitializeComponent();
            txtUserName.Text = $"{CurrentUser.FirstName} {CurrentUser.SurName}";

        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientPage());
        }

        private void Glav_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalAccount());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Sot_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tarig_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Model_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Car_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left)
            //{
            //    this.DragMove();
            ////////}
        }

        private void Border_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                if (this.IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }
    }
}
