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
using Style = System.Windows.Style;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for AddEditPageTaff.xaml
    /// </summary>
    public partial class AddEditPageTaff : Window
    {
        
        private TaffForCurrent currentItem;
        public AddEditPageTaff(TaffForCurrent item)
        {
            InitializeComponent();
            StylePage();

            currentItem = item;

            if (currentItem == null)
            {
                
                Add.Visibility = Visibility.Collapsed;
            }
            else
            {
                
                idTaff.Text = item.id_Taff.ToString();
                NameTaff.Text = item.NameTaff;
                Opicanie.Text = item.Opicanie;
                CtoimictiKm.Text = item.PriceKM.ToString();
                


            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                int taskID = int.Parse(idTaff.Text);

                var item = context.Тарифы.FirstOrDefault(s => s.id_Тарифы == taskID);

                if (item != null)
                {
                    item.id_Тарифы = taskID;
                    item.Название_тарифа = NameTaff.Text;
                    item.Описание = Opicanie.Text;
                    item.Стоимость_за_км = Convert.ToDecimal(CtoimictiKm.Text);


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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TaxApplicationEntities())
            {
                if (currentItem == null)
                {
                    // Создание нового элемента
                    Тарифы task = new Тарифы()
                    {
                        Название_тарифа = NameTaff.Text,
                        Описание = Opicanie.Text,
                        Стоимость_за_км = Convert.ToDecimal(CtoimictiKm.Text),
                       
                    };

                    context.Тарифы.Add(task);
                    context.SaveChanges();
                    this.Close();
                    MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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
