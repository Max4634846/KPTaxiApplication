using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for AddEditPageModel.xaml
    /// </summary>
    public partial class AddEditPageModel : Window
    {
        private ModelForCurrent currentItem;
        public AddEditPageModel(ModelForCurrent item)
        {
            InitializeComponent();
            StylePage();

            currentItem = item;

            if (currentItem == null)
            {
                // Добавление нового элемента
                Add.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Редактирование существующего элемента
                idModel.Text = item.id_Model.ToString();
                Mod.Text = item.Model;
                Marka.Text = item.Marka;
                Colore.Text = item.Svet;
                God.Text = item.God;
                Kus.Text = item.Tip_Kus;



            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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
                    Модель task = new Модель()
                    {
                        Модель1 = Mod.Text,
                        Марка = Marka.Text,
                        Цвет = Colore.Text,
                        Год = God.Text,
                        Тип_кузова = Kus.Text,

                    };

                    context.Модель.Add(task);
                    context.SaveChanges();
                    this.Close();
                    MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                int taskID = int.Parse(idModel.Text);

                var item = context.Модель.FirstOrDefault(s => s.id_Модель == taskID);

                if (item != null)
                {
                    item.id_Модель = taskID;
                    item.Модель1 = Mod.Text;
                    item.Марка = Marka.Text;
                    item.Цвет = Colore.Text;
                    item.Год = God.Text;
                    item.Тип_кузова = Kus.Text;



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
    }
}
