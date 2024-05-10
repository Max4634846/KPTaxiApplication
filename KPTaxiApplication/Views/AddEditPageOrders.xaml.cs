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
                NumAvto.SelectedItem = item.id_Avto;
                NumClient.SelectedItem = item.id_Client;
                NumTariff.SelectedItem = item.id_Taff;
                Otkuda.Text = item.Otkuda;
                Kuda.Text = item.Kuda;
                TimeStartSakasa.Text = item.StartTime;
                TimeFinishSakasa.Text = item.FinishTimne;
                TimeSakasa.Text = item.EndTime.ToString();
                Data.SelectedDate = item.Date;
                PutiKm.Text = item.PutKm.ToString();
                StatusSakasa.SelectedItem = item.StatusOrder;
                Bagaje.SelectedItem = item.Bagaje;
                VuborOplati.SelectedItem = item.ViborOplate;
                Ojidanie.SelectedItem = item.Expectation;
                Ocenka.SelectedItem = item.Estimation;
            }


        }

        private void ItemSource()
        {
            var avtoList = _context.Автомобили.Select(el => new AvtoItem { Id = el.id_Автомобили, Name = el.Сотрудники.Должность.Название_должности }).ToList();

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
                    item.id_Автомобили = (int)NumAvto.SelectedValue;
                    item.id_Клиенты = (int)NumClient.SelectedItem;
                    item.id_Тарифы = (int)NumTariff.SelectedValue;
                    item.Откуда = Otkuda.Text;
                    item.Место_отправления = Kuda.Text;
                    item.Время_начала_заказа = TimeStartSakasa.Text;
                    item.Время_окончания_заказа = TimeFinishSakasa.Text;
                    item.Время_заказа = TimeSakasa.Text;
                    item.Дата = Data.SelectedDate;
                    item.Путь_в_км = Convert.ToInt32(PutiKm.Text);
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
    }
}
