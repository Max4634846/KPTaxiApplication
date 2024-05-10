using KPTaxiApplication.Model;
using MaterialDesignThemes.Wpf;
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
    /// <summary>
    /// Interaction logic for AddEditPageSot.xaml
    /// </summary>
    public partial class AddEditPageSot : Window
    {
        private TaxApplicationEntities _context;
        private SotForrCurrentUser currentItem;
        public AddEditPageSot(SotForrCurrentUser item)
        {
            InitializeComponent();
            _context = new TaxApplicationEntities();
            StylePage();
            ItemSource();

            currentItem = item;

            if (currentItem == null)
            {
                // Добавление нового элемента
                Add.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Редактирование существующего элемента
                idSot.Text = item.id_Sot.ToString();
                NumDol.SelectedItem = item.id_Dol;
                Login.Text = item.Login;
                Password.Text = item.Password;
                Name.Text = item.FirstName;
                Fam.Text = item.SurName;
                Pot.Text = item.Patroniec;
                Status.Text = item.Status;
                Reiting.SelectedItem = item.Rating;
                NumTel.Text = item.NumPhone;
                Mail.Text = item.Email;
                PaspDanie.Text = item.Pasport;
                Adress.Text = item.Address;

            }
        }
        private void ItemSource()
        {
            var avtoList = _context.Должность.Select(el => new AvtoItem { Id = el.id_Должность, Name = el.Название_должности }).ToList();

            NumDol.ItemsSource = avtoList;
            NumDol.DisplayMemberPath = "Name";
            NumDol.SelectedValuePath = "Id";


        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                int taskID = int.Parse(idSot.Text);

                var item = context.Сотрудники.FirstOrDefault(s => s.id_Сотрудники == taskID);

                if (item != null)
                {
                    item.id_Сотрудники = taskID;
                    item.id_Должность = Convert.ToInt32(NumDol.SelectedValue);
                    item.Логин = Login.Text;
                    item.Пароль = Password.Text;
                    item.Имя = Name.Text;
                    item.Фамилия = Fam.Text;
                    item.Отчество = Pot.Text;
                    item.Статус = Status.Text;
                    item.Рейтинг = Convert.ToDouble(Reiting.Text);
                    item.Номер_телефона = NumTel.Text;
                    item.Почта = Mail.Text;
                    item.Паспортные_данные = PaspDanie.Text;
                    item.Адрес = Adress.Text;

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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                if (currentItem == null)
                {
                    // Создание нового элемента
                    Сотрудники task = new Сотрудники()
                    {
                        id_Должность = Convert.ToInt32(NumDol.SelectedValue),
                        Логин = Login.Text,
                        Пароль = Password.Text,
                        Имя = Name.Text,
                        Фамилия = Fam.Text,
                        Отчество = Pot.Text,
                        Статус = Convert.ToString(Status.SelectedItem),
                        Рейтинг = Convert.ToDouble(Reiting.Text),
                        Номер_телефона = NumTel.Text,
                        Почта = Mail.Text,
                        Паспортные_данные = PaspDanie.Text,
                        Адрес = Adress.Text
                    };

                    context.Сотрудники.Add(task);
                    context.SaveChanges();
                    this.Close();
                    MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        public void StylePage()
        {
            if (CurrentUser.Status == 2)
            {
                Border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));
                Back.Style = (Style)Application.Current.Resources["addButtonDis"];
                Add.Style = (Style)Application.Current.Resources["addButtonDis"];
                Edit.Style = (Style)Application.Current.Resources["addButtonDis"];
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
