using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for ModelPage.xaml
    /// </summary>
    public partial class ModelPage : Page
    {
        ObservableCollection<ModelForCurrent> ClientList;
        public ModelPage()
        {
            InitializeComponent();
            UpdateDataGrid();
            StylePage();
        }

        private void UpdateDataGrid()
        {
            using (var context = new TaxApplicationEntities())
            {
                ClientList = new ObservableCollection<ModelForCurrent>(context.Модель.Select(r => new ModelForCurrent
                {
                    id_Model = r.id_Модель,
                    Model = r.Модель1,
                    Marka = r.Марка,
                    Svet = r.Цвет,
                    God = r.Год,
                    Tip_Kus = r.Тип_кузова,

                }).ToList());

                DGSot.ItemsSource = ClientList;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditPageModel view = new AddEditPageModel(null);
            view.Closed += EditWindow_Closed;
            view.Add.Visibility = Visibility.Visible;
            view.idModel.Visibility = Visibility.Collapsed;
            view.idModelText.Visibility = Visibility.Collapsed;
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
                    ModelForCurrent order = obj as ModelForCurrent;
                    if (order != null)
                    {
                        return order.Model.ToString().ToLower().Contains(filterText) ||
                               order.Marka.ToString().ToLower().Contains(filterText) ||
                               order.God.ToString().ToLower().Contains(filterText) ||
                               order.Tip_Kus.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGSot.SelectedItems.Cast<ModelForCurrent>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedClients.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TaxApplicationEntities())
                    {
                        foreach (var client in selectedClients)
                        {
                            // Находим объект клиента по его ID и добавляем его в коллекцию для удаления
                            var clientToRemove = context.Модель.Find(client.id_Model);
                            if (clientToRemove != null)
                                context.Модель.Remove(clientToRemove);
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
            ModelForCurrent selectedItem = DGSot.SelectedItem as ModelForCurrent;

            if (selectedItem != null)
            {
                AddEditPageModel view = new AddEditPageModel(selectedItem);
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
        public void StylePage()
        {
            if (CurrentUser.Status == 2)
            {

                HeaderSix.Visibility = Visibility.Collapsed;
                One.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Two.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Three.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Four.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Five.HeaderStyle = (Style)Application.Current.Resources["dataGridHeaderDis"];
                Add.Style = (Style)Application.Current.Resources["addButtonDis"];


            }
        }
    }
}
