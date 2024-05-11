using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using Xceed.Wpf.Toolkit.Primitives;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        ObservableCollection<ClientForCurrentUser> ClientList;
        public ClientPage()
        {
            InitializeComponent();
            //DGClient.ItemsSource = TaxApplicationEntities.GetContext().Клиенты.ToList();
            StyleGrid(); 
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<ClientForCurrentUser>(context.Клиенты.Select(r => new ClientForCurrentUser
                {
                        id = r.id_Клиенты,
                        FirstName = r.Имя,
                        SurName = r.Фамилия,
                        Otzestvo = r.Отчество,
                        NumberPhone = r. Номер_телефона,


                    }).ToList());

                DGClient.ItemsSource = ClientList;
            }
        }
        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGClient.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    ClientForCurrentUser order = obj as ClientForCurrentUser;
                    if (order != null)
                    {
                        return order.id.ToString().ToLower().Contains(filterText) ||
                               order.FirstName.ToString().ToLower().Contains(filterText) ||
                               order.SurName.ToLower().Contains(filterText) ||
                               order.Otzestvo.ToLower().Contains(filterText) ||
                               order.NumberPhone.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }
        private void AddBD_Click(object sender, RoutedEventArgs e)
        {

            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
     
                AddEditPageClient view = new AddEditPageClient(null);
                view.Closed += EditWindow_Closed;
                view.AddClient.Visibility = Visibility.Collapsed;
                view.id.Visibility = Visibility.Collapsed;
                view.idText.Visibility = Visibility.Collapsed;
                view.ShowDialog();
 
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGClient.SelectedItems.Cast<ClientForCurrentUser>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Клиенты.Find(client.id);
                            if (clientToRemove != null)
                                context.Клиенты.Remove(clientToRemove);
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

        private void DGClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClientForCurrentUser selectedItem = DGClient.SelectedItem as ClientForCurrentUser;

            if (selectedItem != null)
            {
                AddEditPageClient view = new AddEditPageClient(selectedItem);
                view.Closed += EditWindow_Closed;
                view.Edit.Visibility = Visibility.Collapsed;
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
            if (CurrentUser.Status == 2)
            {
                HeaderOne.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                HeaderTwo.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                HeaderThree.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                HeaderFour.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                HeaderSix.Visibility = Visibility.Collapsed;
                Add.Style = (Style)Application.Current.Resources["addButtonDis"];

            }
        }



    }
}
