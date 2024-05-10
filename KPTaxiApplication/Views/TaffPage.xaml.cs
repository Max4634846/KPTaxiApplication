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
    /// Interaction logic for TaffPage.xaml
    /// </summary>
    public partial class TaffPage : Page
    {
        ObservableCollection<TaffForCurrent> ClientList;
        public TaffPage()
        {
            InitializeComponent();
            UpdateDataGrid();
            StylePage();
        }
        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<TaffForCurrent>(context.Тарифы.Select(r => new TaffForCurrent
                {
                    id_Taff = r.id_Тарифы,
                    NameTaff = r.Название_тарифа,
                    Opicanie = r.Описание,
                    PriceKM = (decimal)r.Стоимость_за_км,
                    

                }).ToList());

                DGTaff.ItemsSource = ClientList;
            }
        }

        private void DGTaff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaffForCurrent selectedItem = DGTaff.SelectedItem as TaffForCurrent;

            if (selectedItem != null)
            {
                AddEditPageTaff view = new AddEditPageTaff(selectedItem);
                view.Closed += EditWindow_Closed;
                view.Add.Visibility = Visibility.Collapsed;
                view.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditPageTaff view = new AddEditPageTaff(null);
            view.Closed += EditWindow_Closed;
            view.Add.Visibility = Visibility.Visible;
            view.idTaff.Visibility = Visibility.Collapsed;
            view.idTaffText.Visibility = Visibility.Collapsed;
            view.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGTaff.SelectedItems.Cast<TaffForCurrent>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Тарифы.Find(client.id_Taff);
                            if (clientToRemove != null)
                                context.Тарифы.Remove(clientToRemove);
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

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(DGTaff.ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) =>
                {
                    TaffForCurrent order = obj as TaffForCurrent;
                    if (order != null)
                    {
                        return order.NameTaff.ToString().ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        public void StylePage()
        {
            if (CurrentUser.Status == 2)
            {

                Header.Visibility = Visibility.Collapsed;
                One.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Two.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Three.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Add.Style = (Style)Application.Current.Resources["addButtonDis"];


            }
        }
    }
}
