using ControlzEx.Standard;
using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Interaction logic for AddEditPageDol.xaml
    /// </summary>
    public partial class AddEditPageDol : Window
    {
        private DolForCurrent currentItem;
        public AddEditPageDol(DolForCurrent item)
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
                idDol.Text = item.id_Dol.ToString();
                NameDol.Text = item.NameDol;
                Opicanie.Text = item.Opicanie;
                SP.Text = item.Sarplata.ToString();
   

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
                    Должность task = new Должность()
                    {
                        Название_должности = NameDol.Text,
                        Описание = Opicanie.Text,
                        Зарплата = Convert.ToDecimal(SP.Text),

                    };

                    context.Должность.Add(task);
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
                int taskID = int.Parse(idDol.Text);

                var item = context.Должность.FirstOrDefault(s => s.id_Должность == taskID);

                if (item != null)
                {
                    item.id_Должность = taskID;
                    item.Название_должности = NameDol.Text;
                    item.Описание = Opicanie.Text;
                    item.Зарплата = Convert.ToDecimal(SP.Text);



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

    }
}
