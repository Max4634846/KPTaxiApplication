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
using System.Windows.Threading;

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
            Loaded += AdminWin_Loaded;
            txtUserName.Text = $"{CurrentUser.FirstName} {CurrentUser.SurName}";

            

        }
        private void AdminWin_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
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
            MainFrame.Navigate(new OrdersPage());
        }

        private void Sot_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.Status == 2)
            {

                MessageBox.Show("Сотрудники меню не доступно для диспетчера. \n Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
            else
            {
                MainFrame.Navigate(new SotPage());
            }
        }

        private void Tarig_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TaffPage());
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.Status == 2)
            {

                MessageBox.Show("Должности меню не доступно для диспетчера. \n Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            else
            {
                MainFrame.Navigate(new DolPage());
            }
        }

        private void Model_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ModelPage());
        }

        private void Car_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AvtoPage());
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
                    this.Width = 1180;
                    this.Height = 820;

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
