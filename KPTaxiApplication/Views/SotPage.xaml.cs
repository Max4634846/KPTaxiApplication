using KPTaxiApplication.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using Path = System.IO.Path;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for SotPage.xaml
    /// </summary>
    public partial class SotPage : Page
    {
        ObservableCollection<SotForrCurrentUser> ClientList;
        public SotPage()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<SotForrCurrentUser>(context.Сотрудники.Select(r => new SotForrCurrentUser
                {
                    id_Sot = r.id_Сотрудники,
                    id_Dol = (int)r.id_Должность,
                    Login = r.Логин,
                    Password = r.Пароль,
                    FirstName = r.Имя,
                    SurName = r.Фамилия,
                    Patroniec = r.Отчество,
                    Status = r.Статус,
                    Rating = (float)r.Рейтинг,
                    NumPhone = r.Номер_телефона,
                    Email = r.Почта,
                    Pasport = r.Паспортные_данные,
                    Address = r.Адрес,

                }).ToList());

                DGSotrudniki.ItemsSource = ClientList;
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditPageSot view = new AddEditPageSot(null);
            view.Closed += EditWindow_Closed;
            view.Add.Visibility = Visibility.Visible;
            view.idSot.Visibility = Visibility.Collapsed;
            view.idSotText.Visibility = Visibility.Collapsed;
            view.ShowDialog();
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGSotrudniki.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    SotForrCurrentUser order = obj as SotForrCurrentUser;
                    if (order != null)
                    {
                        return order.id_Sot.ToString().ToLower().Contains(filterText) ||
                               order.id_Dol.ToString().ToLower().Contains(filterText) ||
                               order.FirstName.ToString().ToLower().Contains(filterText) ||
                               order.SurName.ToLower().Contains(filterText) ||
                               order.Patroniec.ToLower().Contains(filterText) ||
                               order.Pasport.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGSotrudniki.SelectedItems.Cast<SotForrCurrentUser>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Сотрудники.Find(client.id_Sot);
                            if (clientToRemove != null)
                                context.Сотрудники.Remove(clientToRemove);
                        }

                        // Сохраняем изменения в базе данных
                        context.SaveChanges();
                    }
                    MessageBox.Show("Данные успешно удалены");
                    UpdateDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void DGSotrudniki_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SotForrCurrentUser selectedItem = DGSotrudniki.SelectedItem as SotForrCurrentUser;

            if (selectedItem != null)
            {
                AddEditPageSot view = new AddEditPageSot(selectedItem);
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

      


    }
}
