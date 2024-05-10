using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        ObservableCollection<OrdersForCurrentUser> OrderList;
        public OrdersPage()
        {
            InitializeComponent();
            StyleGrid();
            UpdateDataGrid();
            Loaded += Page_Loaded;

        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                OrderList = new ObservableCollection<OrdersForCurrentUser>(context.Заказы.Select(r => new OrdersForCurrentUser
                {
                    id_Order = r.id_Заказы,
                    id_Avto = (int)r.id_Автомобили,
                    id_Client = (int)r.id_Клиенты,
                    id_Taff = (int)r.id_Тарифы,
                    Otkuda = r.Откуда,
                    Kuda = r.Место_отправления,
                    StartTime = r.Время_начала_заказа,
                    FinishTimne = r.Время_окончания_заказа,
                    EndTime = r.Время_заказа,
                    Date = (DateTime)r.Дата,
                    PutKm = (float)r.Путь_в_км,
                    StatusOrder = r.Статус_заказа,
                    Price = (decimal)r.Стоимость,
                    Bagaje = (bool)r.Багаж,
                    ViborOplate = r.Выбор_оплаты,
                    Expectation = (bool)r.Ожидание,
                    Estimation = (int)r.Оценка_за_обслуживание,

                }).ToList());

                DGOrder.ItemsSource = OrderList;
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void WebDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                AddEditPageOrders editPageOrders = new AddEditPageOrders(null);

                // Выполнение скрипта JavaScript для получения значения поля на веб-странице
                string script = "document.querySelector('#sb_ifc50 input').value\r\n";
                string script1 = "document.querySelector('#sb_ifc51 input').value\r\n";
                string script2 = "document.querySelector('#QA0Szd .Fk3sm.fontHeadlineSmall.delay-heavy ').textContent\r\n";
                string script3 = "document.querySelector('#QA0Szd .ivN21e.tUEI8e.fontBodyMedium').textContent\r\n";
                string script4 = "document.querySelector('#QA0Szd .delay-medium').textContent\r\n";

                var result = await webView.CoreWebView2.ExecuteScriptAsync(script);
                var result1 = await webView.CoreWebView2.ExecuteScriptAsync(script1);
                var result2 = await webView.CoreWebView2.ExecuteScriptAsync(script2);
                var result3 = await webView.CoreWebView2.ExecuteScriptAsync(script3);
                var result4 = await webView.CoreWebView2.ExecuteScriptAsync(script4);

                // Отображение результата в TextBox
                editPageOrders.Otkuda.Text = result;
                editPageOrders.Kuda.Text = result1;
                editPageOrders.TimeSakasa.Text = result2;
                editPageOrders.TimeSakasa.Text = result4;
                editPageOrders.PutiKm.Text = result3;
                editPageOrders.NumOrders.Visibility = Visibility.Collapsed;


                double value;
                if (double.TryParse(Regex.Replace(result3, @"[^\d\.,]+", ""), out value))
                {

                    editPageOrders.PutiKm.Text = value.ToString(); // Отображение числового значения
                    
                }
                editPageOrders.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void TextBox_Change(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGOrder.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    Заказы order = obj as Заказы;
                    if (order != null)
                    {
                        return order.id_Автомобили.ToString().ToLower().Contains(filterText) ||
                               order.id_Клиенты.ToString().ToLower().Contains(filterText) ||
                               order.id_Тарифы.ToString().ToLower().Contains(filterText) ||
                               order.Откуда.ToLower().Contains(filterText) ||
                               order.Место_отправления.ToLower().Contains(filterText) ||
                               order.Время_начала_заказа.ToLower().Contains(filterText) ||
                               order.Время_окончания_заказа.ToLower().Contains(filterText) ||
                               order.Путь_в_км.ToString().ToLower().Contains(filterText) ||
                               order.Статус_заказа.ToLower().Contains(filterText) ||
                               order.Стоимость.ToString().ToLower().Contains(filterText) ||
                               order.Выбор_оплаты.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }

        private void DGOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrdersForCurrentUser selectedItem = DGOrder.SelectedItem as OrdersForCurrentUser;

            if (selectedItem != null)
            {
                AddEditPageOrders view = new AddEditPageOrders(selectedItem);
                view.Closed += EditWindow_Closed;
                view.Add.Visibility = Visibility.Collapsed;
                view.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        public void StyleGrid()
        {
            if (CurrentUser.Status == 3)
            {
                web.Style = (Style)Application.Current.Resources["addButton"];
                one.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                two.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                three.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                fout.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                five.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                six.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                seven.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                eqiht.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                nine.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                ten.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                eleven.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                twelve.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                thriteen.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                fourteen.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                fifteen.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];
                sixteen.HeaderStyle = (Style)Application.Current.Resources["dataGridHeader"];


            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
