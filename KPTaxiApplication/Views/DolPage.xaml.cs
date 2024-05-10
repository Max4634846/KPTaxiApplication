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
using Xceed.Wpf.Toolkit.Primitives;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for DolPage.xaml
    /// </summary>
    public partial class DolPage : Page
    {
        ObservableCollection<DolForCurrent> ClientList;
        public DolPage()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<DolForCurrent>(context.Должность.Select(r => new DolForCurrent
                {
                    id_Dol = r.id_Должность,
                    NameDol = r.Название_должности,
                    Opicanie = r.Описание,
                    Sarplata = (decimal)r.Зарплата,

                }).ToList());

                DGSot.ItemsSource = ClientList;
            }
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGSot.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    DolForCurrent order = obj as DolForCurrent;
                    if (order != null)
                    {
                        return order.NameDol.ToString().ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditPageDol view = new AddEditPageDol(null);
            view.Closed += EditWindow_Closed;
            view.Add.Visibility = Visibility.Visible;
            view.idDol.Visibility = Visibility.Collapsed;
            view.idDolText.Visibility = Visibility.Collapsed;
            view.ShowDialog();
        }

        private void DGSot_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DolForCurrent selectedItem = DGSot.SelectedItem as DolForCurrent;

            if (selectedItem != null)
            {
                AddEditPageDol view = new AddEditPageDol(selectedItem);
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGSot.SelectedItems.Cast<DolForCurrent>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Должность.Find(client.id_Dol);
                            if (clientToRemove != null)
                                context.Должность.Remove(clientToRemove);
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
    }
}
