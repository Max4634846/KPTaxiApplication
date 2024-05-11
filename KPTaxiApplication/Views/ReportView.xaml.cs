using ControlzEx.Standard;
using KPTaxiApplication.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;
using Table = System.Windows.Documents.Table;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Page
    {
        public ReportView()
        {
            InitializeComponent();
        }
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNameReport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните название файлы.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(nameDis.Text) && string.IsNullOrWhiteSpace(furstNameDis.Text) && string.IsNullOrWhiteSpace(otchectvoDis.Text) && string.IsNullOrWhiteSpace(emailDis.Text) && string.IsNullOrWhiteSpace(pasportDis.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля перед созданием отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            
            string txtName = txtNameReport.Text;
            string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{txtName}.docx";

            txtNameReport.Text = "";

            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;
            decimal result = CalculateTotalRevenue(selectedDate);

            using (var doc = DocX.Create(filePath))
            {
                // Заголовок отчета
                var titleParagraph = doc.InsertParagraph("Отчет о заказе");
                titleParagraph.FontSize(16d).Bold().Alignment = Alignment.center;
                titleParagraph.SpacingAfter(20d);

                // Данные о диспетчере
                var dispatcherParagraph = doc.InsertParagraph("Отчет сформирован диспетчером:");
                dispatcherParagraph.FontSize(14d).Bold().Alignment = Alignment.left;
                dispatcherParagraph.SpacingAfter(10d);
                doc.InsertParagraph($"Имя: {nameDis.Text}");
                doc.InsertParagraph($"Фамилия: {furstNameDis.Text}");
                doc.InsertParagraph($"Отчество: {otchectvoDis.Text}");
                doc.InsertParagraph($"Почта: {emailDis.Text}");
                doc.InsertParagraph($"Паспортные данные: {pasportDis.Text}");
                doc.InsertParagraph($"Общая стоимость за день: {selectedDate}: {result}");





                // Создание таблицы для отображения данных о заказах
                using (var context = new TaxApplicationEntities())
                {
                    var orders = context.Заказы.Take(60).ToList();

                    if (orders.Any())
                    {
                        // Заголовок таблицы
                        var table = doc.AddTable(1, 5);
                        table.Design = TableDesign.TableGrid;
                        table.Alignment = Alignment.center;
                        var header = table.Rows[0];
                        header.Cells[0].Paragraphs.First().Append("Номер заказа");
                        header.Cells[1].Paragraphs.First().Append("Ф.И.О Клиента");
                        header.Cells[2].Paragraphs.First().Append("Дата заказа");
                        header.Cells[3].Paragraphs.First().Append("Стоимость заказа");
                        header.Cells[4].Paragraphs.First().Append("Ф.И.О Водителя");

                        // Данные в таблице
                        foreach (var order in orders)
                        {
                            var row = table.InsertRow();
                            row.Cells[0].Paragraphs.First().Append(order.id_Заказы.ToString());
                            row.Cells[1].Paragraphs.First().Append($"{order.Клиенты.Имя} {order.Клиенты.Фамилия} {order.Клиенты.Отчество}");
                            row.Cells[2].Paragraphs.First().Append(order.Дата.ToString());
                            row.Cells[3].Paragraphs.First().Append(order.Стоимость.ToString());
                            row.Cells[4].Paragraphs.First().Append($"{order.Автомобили.Сотрудники.Имя} {order.Автомобили.Сотрудники.Фамилия} {order.Автомобили.Сотрудники.Отчество}");
                        }

                        // Добавление таблицы в отчет
                        doc.InsertTable(table);
                    }
                    else
                    {
                        doc.InsertParagraph("Нет данных об автомобилях для отображения.").Bold();
                    }
                }

                // Сохранение и открытие файла отчета
                doc.Save();
                Process.Start(filePath);
                RefreshPage();
            }

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string[] reportFiles = Directory.GetFiles("C:\\Users\\ultra\\Desktop\\Отчеты", "*.docx");

            foreach (string file in reportFiles)
            {
                reportsComboBox.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }

        }

        private void BtnComboBox_Clikc(object sender, RoutedEventArgs e)
        {
            string selectedReport = (string)reportsComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedReport))
            {
                string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{selectedReport}.docx";

                Process.Start(filePath);
            }
        }

        private void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            string selectedReport = (string)reportsComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedReport))
            {
                string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{selectedReport}.docx";

                // Удаляем файл с диска
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Удаляем элемент из ComboBox
                reportsComboBox.Items.Remove(selectedReport);
            }
        }

        private void RefreshPage()
        {


            reportsComboBox.Items.Clear();

            txtNameReport.Text = "";
            nameDis.Text = "";
            furstNameDis.Text = "";
            otchectvoDis.Text = "";
            emailDis.Text = "";
            pasportDis.Text = "";

            string[] reportFiles = Directory.GetFiles("C:\\Users\\ultra\\Desktop\\Отчеты", "*.docx");

            foreach (string file in reportFiles)
            {
                reportsComboBox.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private decimal CalculateTotalRevenue(DateTime selectedDate)
        {
            // Получение экземпляра контекста
            var context = TaxApplicationEntities.GetContext();

            // Вызов хранимой процедуры
            var result = context.Database.SqlQuery<decimal>("EXEC CalculateTotalRevenueByDate @Date", new SqlParameter("@Date", selectedDate)).FirstOrDefault();

            return result;
        }
    }
}
