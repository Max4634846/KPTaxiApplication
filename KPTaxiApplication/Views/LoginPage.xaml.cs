using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        BitmapImage imageSource = new BitmapImage(new Uri("C:\\Users\\ultra\\source\\repos\\KPTaxiApplication\\KPTaxiApplication\\Images\\iconPodderj.png"));

        public event PropertyChangedEventHandler PropertyChanged;

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private string LogInPassword;

        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            LogInPassword = passwordBox.Password;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {

                var user1 = context.Сотрудники
                    .FirstOrDefault(u => u.Логин == txtLogin.Text
                    && u.Пароль == LogInPassword);

                if (user1 != null)
                {
                    if(user1.id_Должность == 3)
                    {
                        CurrentUser.ID = user1.id_Сотрудники;
                        CurrentUser.Login = user1.Логин;
                        CurrentUser.Password = user1.Пароль;
                        CurrentUser.Status = (int)user1.id_Должность;
                        CurrentUser.FirstName = user1.Имя;
                        CurrentUser.SurName = user1.Фамилия;

                        UsersPanelWindow admin = new UsersPanelWindow();
                        admin.Show();

                        Window mainWindow = Window.GetWindow(this);
                        mainWindow.Close();
                    }
                    else if(user1.id_Должность == 2)
                    {
                        CurrentUser.ID = user1.id_Сотрудники;
                        CurrentUser.Login = user1.Логин;
                        CurrentUser.Password = user1.Пароль;
                        CurrentUser.Status = (int)user1.id_Должность;
                        CurrentUser.FirstName = user1.Имя;
                        CurrentUser.SurName = user1.Фамилия;

                        UsersPanelWindow users = new UsersPanelWindow();
                        
                        


                        users.txtStatus.Text = "Диспетчер";
                        users.txtOpicanie.Text = "Диспетчер принимает\n звонки клиентов";
                        users.imageUser.ImageSource = imageSource;
                        users.panelMenu.Background = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));
                        users.Time.Background = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));
                        users.passAc.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.home.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.sot.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.taff.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.client.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.dol.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.mod.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.avt.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.exit.Style = (Style)Application.Current.Resources["menuButtonDis"];
                        users.separator.Background = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));




                        users.Show();

                        Window mainWindow = Window.GetWindow(this);
                        mainWindow.Close();
                    }
                }

                else
                {
                    ErrorMessage = "Invalid username or password.";
                }
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
