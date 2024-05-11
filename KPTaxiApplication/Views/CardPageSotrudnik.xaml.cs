using KPTaxiApplication.Model;
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
using System.Windows.Shapes;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for CardPageSotrudnik.xaml
    /// </summary>
    public partial class CardPageSotrudnik : Window
    {
        public CardPageSotrudnik()
        {
            InitializeComponent();
            Styles();
            txtStatus.Text = $"Стаутс: {CurrentUser.Status}";
            txtNameUser.Text = $"Ф.И.О: {CurrentUser.FirstName} {CurrentUser.SurName} {CurrentUser.Patronies}";
            txtNumberPhone.Text = $"Номер телефона: {CurrentUser.PhoneNumber}";
            txtMail.Text = $"Email: {CurrentUser.Email}";
            txtPasport.Text = $"Паспортные данные: {CurrentUser.Pasport}";
            txtOpicanie.Text = $"Описание области: \n{CurrentUser.Opicanie}";
            HelpBtn.Visibility = Visibility.Collapsed;

            if (CurrentUser.Status == 2)
            {
                HelpBtn.Visibility = Visibility.Visible;
            }
        }

        private void HelpAdmin()
        {
            using (var context = new TaxApplicationEntities())
            {
                var adminDetails = context.Сотрудники
                    .Where(s => s.id_Должность == 3) // Выбираем только администраторов
                    .Select(s => new { FullName = s.Имя + " " + s.Фамилия, PhoneNumber = s.Номер_телефона, Email = s.Почта }) // Выбираем имя, фамилию и номер телефона администраторов
                    .ToList();

                if (adminDetails.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var admin in adminDetails)
                    {
                        sb.AppendLine($"Имя: {admin.FullName}, Номер телефона: {admin.PhoneNumber}, Почта: {admin.Email}");
                    }
                    MessageBox.Show($"Данные администраторов:{Environment.NewLine}{sb.ToString()}", "Информация об администраторах", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Нет администраторов с таким статусом.", "Номера телефонов администраторов", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void BtnCardSot_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Styles()
        { 
            if(CurrentUser.Status == 3)
            {
                BorderCard.Background = new SolidColorBrush(Color.FromArgb(255, 0x81, 0x6a, 0xba));
                BtnCardSot.Style = (Style)Application.Current.Resources["addButton"];
                BtnCardSot.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                BtnCardSot.Foreground = new SolidColorBrush(Color.FromArgb(255, 0xf, 0xf, 0xf));
                Border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0x81, 0x6a, 0xba));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HelpAdmin();
        }
    }
}
