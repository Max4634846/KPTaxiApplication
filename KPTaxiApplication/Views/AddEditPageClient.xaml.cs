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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KPTaxiApplication.Views   
{

    public partial class AddEditPageClient : Window
    {
        private ClientForCurrentUser currentItem;
        public AddEditPageClient(ClientForCurrentUser item)
        {
            InitializeComponent();
            StylePage();

            currentItem = item;

            if (currentItem == null)
            {
                // Добавление нового элемента
                AddClient.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Редактирование существующего элемента
                id.Text = item.id.ToString();
                nameTextBox.Text = item.FirstName;
                surnameTextBox.Text = item.SurName;
                patronymicTextBox.Text = item.Otzestvo;
                numberTextBox.Text = item.NumberPhone;

            }


        }

        private void AddBD_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                int taskID = int.Parse(id.Text);

                var item = context.Клиенты.FirstOrDefault(s => s.id_Клиенты == taskID);

                if (item != null)
                {
                    item.id_Клиенты = taskID;
                    item.Имя = nameTextBox.Text;
                    item.Фамилия = surnameTextBox.Text;
                    item.Отчество = patronymicTextBox.Text;
                    item.Номер_телефона = numberTextBox.Text;


                    context.SaveChanges();

                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Элемент с указанным кодом не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public void StylePage()
        {
            if (CurrentUser.Status == 2)
            {
                Border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));
                Back.Style = (Style)Application.Current.Resources["addButtonDis"];
                AddClient.Style = (Style)Application.Current.Resources["addButtonDis"];
                Edit.Style = (Style)Application.Current.Resources["addButtonDis"];
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                if (currentItem == null)
                {
                    // Создание нового элемента
                    Клиенты task = new Клиенты()
                    {
                        Имя = nameTextBox.Text,
                        Фамилия = surnameTextBox.Text,
                        Отчество = patronymicTextBox.Text,
                        Номер_телефона = numberTextBox.Text,

                    };

                    context.Клиенты.Add(task);
                    context.SaveChanges();
                    this.Close();
                    MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
