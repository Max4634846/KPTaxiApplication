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
    /// Interaction logic for AddEditPageAvto.xaml
    /// </summary>
    public partial class AddEditPageAvto : Window
    {
        private AvtoForCurrent currentItem;
        public AddEditPageAvto(AvtoForCurrent item)
        {
            InitializeComponent();
            currentItem = item;

            if (currentItem == null)
            {
                // Добавление нового элемента
                Add.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Редактирование существующего элемента
                idAvto.Text = item.id_Avto.ToString();
                numberMod.Text = item.id_Model.ToString();
                numberSot.Text = item.id_Sot.ToString();



            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                if (currentItem == null)
                {
                    // Создание нового элемента
                    Автомобили task = new Автомобили()
                    {
                        id_Модель = Convert.ToInt32(numberMod.Text),
                        id_Сотрудники = Convert.ToInt32(numberSot.Text),

                    };

                    context.Автомобили.Add(task);
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
                int taskID = int.Parse(idAvto.Text);

                var item = context.Автомобили.FirstOrDefault(s => s.id_Автомобили == taskID);

                if (item != null)
                {
                    item.id_Автомобили = taskID;
                    item.id_Модель = Convert.ToInt32(numberMod.Text);
                    item.id_Сотрудники = Convert.ToInt32(numberSot.Text);



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

       
    }
}
