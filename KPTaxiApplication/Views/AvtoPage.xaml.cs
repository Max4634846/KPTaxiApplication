using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AvtoPage.xaml
    /// </summary>
    public partial class AvtoPage : Page
    {
        ObservableCollection<AvtoForCurrent> ClientList;
        public AvtoPage()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<AvtoForCurrent>(context.Автомобили.Select(r => new AvtoForCurrent
                {
                    id_Avto = r.id_Автомобили,
                    id_Model = (int)r.id_Модель,
                    id_Sot = (int)r.id_Сотрудники,

                }).ToList());

                DGSot.ItemsSource = ClientList;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditPageAvto view = new AddEditPageAvto(null);
            view.Closed += EditWindow_Closed;
            view.Add.Visibility = Visibility.Visible;
            view.idAvto.Visibility = Visibility.Collapsed;
            view.idAvtoText.Visibility = Visibility.Collapsed;
            view.ShowDialog();
        }


        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGSot.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    AvtoForCurrent order = obj as AvtoForCurrent;
                    if (order != null)
                    {
                        return order.id_Model.ToString().ToLower().Contains(filterText) ||
                               order.id_Sot.ToString().ToLower().Contains(filterText);

                    }
                    return false;
                };
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGSot.SelectedItems.Cast<AvtoForCurrent>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Автомобили.Find(client.id_Avto);
                            if (clientToRemove != null)
                                context.Автомобили.Remove(clientToRemove);
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


        private void DGSot_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AvtoForCurrent selectedItem = DGSot.SelectedItem as AvtoForCurrent;

            if (selectedItem != null)
            {
                AddEditPageAvto view = new AddEditPageAvto(selectedItem);
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
