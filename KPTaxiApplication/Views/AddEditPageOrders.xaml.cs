using ControlzEx.Standard;
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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Security.Policy;
using Application = System.Windows.Application;
using static MaterialDesignThemes.Wpf.Theme;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for AddEditPageOrders.xaml
    /// </summary>
    public partial class AddEditPageOrders : Window
    {
        private OrdersForCurrentUser currentItem;
        private TaxApplicationEntities _context;
        public AddEditPageOrders(OrdersForCurrentUser item)
        {
            InitializeComponent();
            StyleGrid();

            _context = new TaxApplicationEntities();

            ItemSource();

            currentItem = item;

            if (currentItem == null)
            {
                // Добавление нового элемента
                Edit.Visibility = Visibility.Collapsed;
            }
            else
            {
                idOrder.Text = item.id_Order.ToString();
                NumAvto.SelectedValue = item.id_Avto.ToString();
                NumClient.SelectedItem = item.id_Client;
                NumTariff.SelectedValue = item.id_Taff;
                Otkuda.Text = item.Otkuda;
                Kuda.Text = item.Kuda;
                TimeStartSakasa.Text = item.StartTime;
                TimeFinishSakasa.Text = item.FinishTimne;
                TimeSakasa.Text = item.EndTime;
                Data.SelectedDate = item.Date;
                PutiKm.Text = item.PutKm.ToString();
                StatusSakasa.Text = item.StatusOrder.ToString();
                Bagaje.Text = item.Bagaje.ToString();
                VuborOplati.Text = item.ViborOplate.ToString();
                Ojidanie.Text = item.Expectation.ToString();
                Ocenka.Text = item.Estimation.ToString();
                NumAvto.SelectionChanged += NumAvto_SelectedIndexChanged;



            }



        }

        private void ShowAvailableDrivers()
        {
            using (var context = new TaxApplicationEntities())
            {
                var availableDrivers = context.Сотрудники
                    .Where(s => s.Автомобили.All(a => a.Заказы.All(z => z.Статус_заказа == "Завершен")) && s.Должность.id_Должность == 1)
                    .Select(s => new { FullName = s.Имя + " " + s.Фамилия, PhoneNumber = s.Номер_телефона})
                    .ToList();

                if (availableDrivers.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var driver in availableDrivers)
                    {
                        sb.AppendLine($"Имя: {driver.FullName}, Номер телефона: {driver.PhoneNumber}");
                    }
                    MessageBox.Show($"Доступные водители:{Environment.NewLine}{sb.ToString()}", "Свободные водители", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("На данный момент свободных водителей нет.", "Свободные водители", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ItemSource()
        {
            //var avtoList = _context.Автомобили.Select(el => new AvtoItem { Id = el.id_Автомобили, Name = el.Сотрудники.Должность.Название_должности }).ToList();

            //NumAvto.ItemsSource = avtoList;
            //NumAvto.DisplayMemberPath = "Name"; 
            //NumAvto.SelectedValuePath = "Id";


            var avtoList = _context.Автомобили
                .Where(avto => avto.Сотрудники.Должность.id_Должность == 1) // Фильтр по должности
                .Select(el => new AvtoItem { Id = el.id_Автомобили, Name = el.Сотрудники.Имя })
                .ToList();

            NumAvto.ItemsSource = avtoList;
            NumAvto.DisplayMemberPath = "Name";
            NumAvto.SelectedValuePath = "Id";


            var ListClient = _context.Клиенты.Select(el => el.id_Клиенты).ToList();

            NumClient.ItemsSource = ListClient;

            var taffList = _context.Тарифы.Select(el => new AvtoItem { Id = el.id_Тарифы, Name = el.Название_тарифа }).ToList();

            NumTariff.ItemsSource = taffList;
            NumTariff.DisplayMemberPath = "Name";
            NumTariff.SelectedValuePath = "Id";

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                int taskID = int.Parse(idOrder.Text);

                var item = context.Заказы.FirstOrDefault(s => s.id_Заказы == taskID);

                if (item != null)
                {
                    item.id_Заказы = taskID;
                    item.id_Автомобили = (int)((AvtoItem)NumAvto.SelectedItem).Id;
                    item.id_Клиенты = (int)NumClient.SelectedItem;
                    item.id_Тарифы = (int)NumTariff.SelectedValue;
                    item.Откуда = Otkuda.Text;
                    item.Место_отправления = Kuda.Text;
                    item.Время_начала_заказа = TimeStartSakasa.Text;
                    item.Время_окончания_заказа = TimeFinishSakasa.Text;
                    item.Время_заказа = TimeSakasa.Text;
                    item.Дата = Data.SelectedDate;
                    item.Путь_в_км = Convert.ToDouble(PutiKm.Text);
                    item.Статус_заказа = StatusSakasa.Text;
                    item.Багаж = Convert.ToBoolean(Bagaje.Text);
                    item.Выбор_оплаты = VuborOplati.Text;
                    item.Ожидание = Convert.ToBoolean(Ojidanie.Text);
                    item.Оценка_за_обслуживание = Convert.ToInt32(Ocenka.Text);





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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                if (currentItem == null)
                {
                    // Создание нового элемента
                    Заказы task = new Заказы()
                    {

                        id_Автомобили = Convert.ToInt32(NumAvto.SelectedValue),
                        id_Клиенты = Convert.ToInt32(NumClient.SelectedItem),
                        id_Тарифы = Convert.ToInt32(NumTariff.SelectedValue),
                        Откуда = Otkuda.Text,
                        Место_отправления = Kuda.Text,
                        Время_начала_заказа = TimeStartSakasa.Text,
                        Время_окончания_заказа = TimeFinishSakasa.Text,
                        Время_заказа = TimeSakasa.Text,
                        Дата = Convert.ToDateTime(Data.Text),
                        Путь_в_км = Convert.ToDouble(PutiKm.Text),
                        Статус_заказа = StatusSakasa.Text,
                        Багаж = Convert.ToBoolean(Bagaje.Text),
                        Выбор_оплаты = VuborOplati.Text,
                        Ожидание = Convert.ToBoolean(Ojidanie.Text),
                        Оценка_за_обслуживание = Convert.ToInt32(Ocenka.Text),
                    };

                    context.Заказы.Add(task);
                    context.SaveChanges();
                    this.Close();
                    MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

            public void StyleGrid()
            {
                if (CurrentUser.Status == 2)
                {
                    Border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0x87, 0x87, 0x87));
                    Back.Style = (Style)Application.Current.Resources["addButtonDis"];
                    Add.Style = (Style)Application.Current.Resources["addButtonDis"];
                    Edit.Style = (Style)Application.Current.Resources["addButtonDis"];

                }
            }

        private void NumAvto_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedCarId = Convert.ToInt32(NumAvto.SelectedValue);

            // Подключение к базе данных и выполнение запроса, чтобы проверить статус выбранного автомобиля
            using (var context = new TaxApplicationEntities())
            {
                var order = context.Заказы.FirstOrDefault(o => o.id_Автомобили == selectedCarId && o.Статус_заказа == "Активный");

                if (order != null)
                {
                    MessageBox.Show("Данный водитель сейчас занят.");
                }
                else
                {
                    // Если водитель свободен, вызываем метод для вывода доступных водителей
                    ShowAvailableDrivers();
                }
            }
        }

        private void StatusSakasa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSakasa.Text != null && StatusSakasa.Text.ToString() == "Завершен")
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                TimeFinishSakasa.Text = currentTime.ToString("hh\\:mm\\:ss");
            }
        }
    }
}
