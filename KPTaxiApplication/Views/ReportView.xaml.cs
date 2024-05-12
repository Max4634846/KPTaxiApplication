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
using System.Runtime.InteropServices.ComTypes;
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
            nameDis.Text = $"{CurrentUser.FirstName}";
            furstNameDis.Text = $"{CurrentUser.SurName}";
            otchectvoDis.Text = $"{CurrentUser.Patronies}";
            emailDis.Text = $"{CurrentUser.Email}";
            pasportDis.Text = $"{CurrentUser.Pasport}";
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtNameReport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните название файла.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(nameDis.Text) || string.IsNullOrWhiteSpace(furstNameDis.Text) || string.IsNullOrWhiteSpace(otchectvoDis.Text) || string.IsNullOrWhiteSpace(emailDis.Text) || string.IsNullOrWhiteSpace(pasportDis.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля перед созданием отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string txtName = txtNameReport.Text;
            string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{txtName}.docx";

            txtNameReport.Text = "";

            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;
            List<DriverOrderInfo> driverOrderInfos = CalculateTotalRevenue(selectedDate);

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
                doc.InsertParagraph($"Имя: {nameDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Фамилия: {furstNameDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Отчество: {otchectvoDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Почта: {emailDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Паспортные данные: {pasportDis.Text}").FontSize(12d).SpacingAfter(5d);


                doc.InsertParagraph($"Общая стоимость всех водителей за день: {driverOrderInfos.Sum(info => info.OrderCost)}").FontSize(12d).SpacingAfter(20d);

                // Создание таблицы для отображения данных о заказах
                using (var context = new TaxApplicationEntities())
                {
                    var orders = context.Заказы.Take(60).ToList();

                    if (orders.Any())
                    {
                        // Заголовок таблицы
                        var table = doc.AddTable(1, 7);
                        table.Design = TableDesign.TableGrid;
                        table.Alignment = Alignment.center;
                        var header = table.Rows[0];
                        header.Cells[0].Paragraphs.First().Append("Номер сотрудника");
                        header.Cells[1].Paragraphs.First().Append("Номер автомобиля");
                        header.Cells[2].Paragraphs.First().Append("Ф.И.О Водителей");
                        header.Cells[3].Paragraphs.First().Append("Марка автомобиля");
                        header.Cells[4].Paragraphs.First().Append("Модель автомобиля");
                        header.Cells[5].Paragraphs.First().Append("Дата заказа");
                        header.Cells[6].Paragraphs.First().Append("Стоимость заказа");

                        // Данные в таблице
                        foreach (var orderInfo in driverOrderInfos)
                        {
                            var row = table.InsertRow();
                            row.Cells[0].Paragraphs.First().Append(orderInfo.Id.ToString());
                            row.Cells[1].Paragraphs.First().Append(orderInfo.id_avto.ToString());
                            row.Cells[2].Paragraphs.First().Append($"{orderInfo.FirstName} {orderInfo.LastName} {orderInfo.Patronymic}");
                            row.Cells[3].Paragraphs.First().Append(orderInfo.Marka.ToString());
                            row.Cells[4].Paragraphs.First().Append(orderInfo.Model.ToString());
                            row.Cells[5].Paragraphs.First().Append(selectedDate.ToShortDateString());
                            row.Cells[6].Paragraphs.First().Append(orderInfo.OrderCost.ToString());
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
            RefreshPage();
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
            //nameDis.Text = "";
            //furstNameDis.Text = "";
            //otchectvoDis.Text = "";
            //emailDis.Text = "";
            //pasportDis.Text = "";

            string[] reportFiles = Directory.GetFiles("C:\\Users\\ultra\\Desktop\\Отчеты", "*.docx");

            foreach (string file in reportFiles)
            {
                reportsComboBox.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }
        }

            private List<DriverOrderInfo> CalculateTotalRevenue(DateTime selectedDate)
            {
                using (var context = new TaxApplicationEntities())
                {
                    var queryResult = context.Database.SqlQuery<DriverOrderInfo>(
                        @"SELECT 
                            Сотрудники.id_Сотрудники AS Id,
                            Автомобили.id_Автомобили AS id_avto,
                            Сотрудники.Имя AS FirstName,
                            Сотрудники.Фамилия AS LastName,
                            Сотрудники.Отчество AS Patronymic,
                            Модель.Марка AS Marka,
                            Модель.Модель AS Model,
                            SUM(ISNULL(Заказы.Стоимость, 0)) AS OrderCost
                        FROM 
                            Сотрудники
                        LEFT JOIN 
                            Автомобили ON Сотрудники.id_Сотрудники = Автомобили.id_Сотрудники
                        LEFT JOIN 
                            Модель ON Автомобили.id_Модель = Модель.id_Модель
                        LEFT JOIN 
                            Заказы ON Автомобили.id_Автомобили = Заказы.id_Автомобили
                        WHERE 
                            Сотрудники.id_Должность = 1
                            AND CONVERT(DATE, Заказы.Дата) = @OrderDate
                        GROUP BY 
                            Сотрудники.id_Сотрудники, Сотрудники.Имя, Сотрудники.Фамилия, Сотрудники.Отчество, Автомобили.id_Автомобили, Модель.Марка, Модель.Модель;",
                        new SqlParameter("@OrderDate", selectedDate))
                        .ToList();

                        return queryResult;
                    }
            }


        private void GenerateReportPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNameReport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните название файла.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(nameDis.Text) || string.IsNullOrWhiteSpace(furstNameDis.Text) || string.IsNullOrWhiteSpace(otchectvoDis.Text) || string.IsNullOrWhiteSpace(emailDis.Text) || string.IsNullOrWhiteSpace(pasportDis.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля перед созданием отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string txtName = txtNameReport.Text;
            string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{txtName}.docx";

            txtNameReport.Text = "";

            DateTime selectedDateOt = datePickerOne.SelectedDate ?? DateTime.Now;
            DateTime selectedDateDo = datePickerTwo.SelectedDate ?? DateTime.Now;
            List<DriverOrderInfoPeriod> driverOrderInfos = CalculateTotalPeriod(selectedDateOt, selectedDateDo);

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
                doc.InsertParagraph($"Имя: {nameDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Фамилия: {furstNameDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Отчество: {otchectvoDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Почта: {emailDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Паспортные данные: {pasportDis.Text}").FontSize(12d).SpacingAfter(5d);


                doc.InsertParagraph($"Общая стоимость всех водителей за период: {driverOrderInfos.Sum(info => info.TotalRevenue)}").FontSize(12d).SpacingAfter(20d);

                // Создание таблицы для отображения данных о заказах
                using (var context = new TaxApplicationEntities())
                {
                    var orders = context.Заказы.Take(60).ToList();

                    if (orders.Any())
                    {
                        // Заголовок таблицы
                        var table = doc.AddTable(1, 7);
                        table.Design = TableDesign.TableGrid;
                        table.Alignment = Alignment.center;
                        var header = table.Rows[0];
                        header.Cells[0].Paragraphs.First().Append("Номер сотрудника");
                        header.Cells[1].Paragraphs.First().Append("Номер автомобиля");
                        header.Cells[2].Paragraphs.First().Append("Ф.И.О Водителей");
                        header.Cells[3].Paragraphs.First().Append("Марка автомобиля");
                        header.Cells[4].Paragraphs.First().Append("Модель автомобиля");
                        header.Cells[5].Paragraphs.First().Append("Дата заказа");
                        header.Cells[6].Paragraphs.First().Append("Стоимость заказа");

                        // Данные в таблице
                        foreach (var orderInfo in driverOrderInfos)
                        {
                            var row = table.InsertRow();
                            row.Cells[0].Paragraphs.First().Append(orderInfo.Id.ToString());
                            row.Cells[1].Paragraphs.First().Append(orderInfo.id_Avto.ToString());
                            row.Cells[2].Paragraphs.First().Append($"{orderInfo.FirstName} {orderInfo.LastName} {orderInfo.Patronymic}");
                            row.Cells[3].Paragraphs.First().Append(orderInfo.Marka.ToString());
                            row.Cells[4].Paragraphs.First().Append(orderInfo.Model.ToString());
                            row.Cells[5].Paragraphs.First().Append($"{selectedDateOt.ToShortDateString()} до {selectedDateDo.ToShortDateString()}");
                            row.Cells[6].Paragraphs.First().Append(orderInfo.TotalRevenue.ToString());
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

        private List<DriverOrderInfoPeriod> CalculateTotalPeriod(DateTime selectedDateOt, DateTime selectedDateDo)
        {
            using (var context = new TaxApplicationEntities())
            {
                var queryResult = context.Database.SqlQuery<DriverOrderInfoPeriod>(
                    @"SELECT
                        Сотрудники.id_Сотрудники AS Id,
                        Автомобили.id_Автомобили AS id_Avto,
                        Сотрудники.Имя AS FirstName,
                        Сотрудники.Фамилия AS LastName,
                        Сотрудники.Отчество AS Patronymic,
                        Модель.Марка AS Marka,
                        Модель.Модель AS Model,
                        SUM(ISNULL(Заказы.Стоимость, 0)) AS TotalRevenue
                    FROM 
                        Заказы
                    INNER JOIN 
                        Автомобили ON Заказы.id_Автомобили = Автомобили.id_Автомобили
                    INNER JOIN 
                        Модель ON Автомобили.id_Модель = Модель.id_Модель
                    INNER JOIN 
                        Сотрудники ON Автомобили.id_Сотрудники = Сотрудники.id_Сотрудники
                    WHERE 
                        Заказы.Дата BETWEEN @StartDate AND @EndDate
                    GROUP BY 
                        Автомобили.id_Автомобили, Модель.Марка, Модель.Модель, Сотрудники.Имя, Сотрудники.Фамилия, Сотрудники.Отчество,  Сотрудники.id_Сотрудники;",
                    new SqlParameter("@StartDate", selectedDateOt),
                    new SqlParameter("@EndDate", selectedDateDo))
                    .ToList();

                return queryResult;
            }
        }

        private void GenerateReportOrders_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNameReport.Text))
            {
                MessageBox.Show("Пожалуйста, заполните название файла.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(nameDis.Text) || string.IsNullOrWhiteSpace(furstNameDis.Text) || string.IsNullOrWhiteSpace(otchectvoDis.Text) || string.IsNullOrWhiteSpace(emailDis.Text) || string.IsNullOrWhiteSpace(pasportDis.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля перед созданием отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string txtName = txtNameReport.Text;
            string filePath = $"C:\\Users\\ultra\\Desktop\\Отчеты\\{txtName}.docx";

            txtNameReport.Text = "";


            List<OrdersInfo> driverOrderInfose = CalculateTotalOrder();

            using (var doc = DocX.Create(filePath))
            {
                // Заголовок отчета
                var titleParagraph = doc.InsertParagraph("Отчет о заказе");
                titleParagraph.FontSize(20d).Bold().Alignment = Alignment.center;
                titleParagraph.SpacingAfter(40d);

                // Данные о диспетчере
                var dispatcherParagraph = doc.InsertParagraph("Отчет сформирован диспетчером:");
                dispatcherParagraph.FontSize(14d).Bold().Alignment = Alignment.left;
                dispatcherParagraph.SpacingAfter(10d);
                doc.InsertParagraph($"Ф.И.О: {nameDis.Text} {furstNameDis.Text} {otchectvoDis.Text} ").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Почта: {emailDis.Text}").FontSize(12d).SpacingAfter(5d);
                doc.InsertParagraph($"Паспортные данные: {pasportDis.Text}").FontSize(12d).SpacingAfter(20d);

                // Общая стоимость всех водителей
                var totalCostParagraph = doc.InsertParagraph($"Общая стоимость всех заказов: {driverOrderInfose.Sum(info => info.Total)}");
                totalCostParagraph.FontSize(14d).Bold().SpacingAfter(20d);

                // Создание таблицы для отображения данных о заказах
                if (driverOrderInfose.Any())
                {
                    var table = doc.AddTable(1, 12);
                    table.Design = TableDesign.TableGrid;
                    table.Alignment = Alignment.center;
                    var header = table.Rows[0];
                    header.Cells[0].Paragraphs.First().Append("Номер клиента");
                    header.Cells[1].Paragraphs.First().Append("Ф.И.О клиента");
                    header.Cells[2].Paragraphs.First().Append("Дата");
                    header.Cells[3].Paragraphs.First().Append("Время начала заказа");
                    header.Cells[4].Paragraphs.First().Append("Время конца заказа");
                    header.Cells[5].Paragraphs.First().Append("Стоимость заказа");
                    header.Cells[6].Paragraphs.First().Append("Марка");
                    header.Cells[7].Paragraphs.First().Append("Модель");
                    header.Cells[8].Paragraphs.First().Append("Год");
                    header.Cells[9].Paragraphs.First().Append("Номер автомобиля");
                    header.Cells[10].Paragraphs.First().Append("Номер сотрудника");
                    header.Cells[11].Paragraphs.First().Append("Ф.И.О водителя");

                    // Добавление данных в таблицу
                    foreach (var orderInfo in driverOrderInfose)
                    {
                        var row = table.InsertRow();
                        row.Cells[0].Paragraphs.First().Append(orderInfo.IdClient.ToString());
                        row.Cells[1].Paragraphs.First().Append($"{orderInfo.NameClient} {orderInfo.SurNameClient} {orderInfo.PatroniecClient}");
                        row.Cells[2].Paragraphs.First().Append(orderInfo.Date.ToString("dd.MM.yyyy"));
                        row.Cells[3].Paragraphs.First().Append(orderInfo.TimeStart.ToString());
                        row.Cells[4].Paragraphs.First().Append(orderInfo.TimeEnd.ToString());
                        row.Cells[5].Paragraphs.First().Append(orderInfo.OrderCosts.ToString("0.00"));
                        row.Cells[6].Paragraphs.First().Append(orderInfo.Marka.ToString());
                        row.Cells[7].Paragraphs.First().Append(orderInfo.Model.ToString());
                        row.Cells[8].Paragraphs.First().Append(orderInfo.God.ToString());
                        row.Cells[9].Paragraphs.First().Append(orderInfo.IdAvto.ToString());
                        row.Cells[10].Paragraphs.First().Append(orderInfo.IdSot.ToString());
                        row.Cells[11].Paragraphs.First().Append($"{orderInfo.FirstNameVod} {orderInfo.SurNameVod} {orderInfo.PatroniecVod}");
                    }

                    // Добавление таблицы в документ
                    doc.InsertTable(table);
                }
                else
                {
                    doc.InsertParagraph("Нет данных об автомобилях для отображения.").Bold();
                }

                // Сохранение и открытие файла отчета
                doc.Save();
                Process.Start(filePath);
                RefreshPage();
            }
        }

        private List<OrdersInfo> CalculateTotalOrder()
        {
            using (var context = new TaxApplicationEntities())
            {
                var queryResult = context.Database.SqlQuery<OrdersInfo>(
                    @"SELECT 
                Клиенты.id_Клиенты AS IdClient,
                Клиенты.Имя AS NameClient,
                Клиенты.Фамилия AS SurNameClient,
                Клиенты.Отчество AS PatroniecClient,
                Заказы.Дата AS Date,
                Заказы.[Время начала заказа] AS TimeStart,
                Заказы.[Время окончания заказа] AS TimeEnd,
                Заказы.Стоимость AS OrderCosts,
                Модель.Марка AS Marka,
                Модель.Модель AS Model,
                Модель.Год AS God,
                Автомобили.id_Автомобили AS IdAvto,
                Сотрудники.id_Сотрудники AS IdSot,
                Сотрудники.Имя AS FirstNameVod,
                Сотрудники.Фамилия AS SurNameVod,
                Сотрудники.Отчество AS PatroniecVod,
                (SELECT SUM(Заказы.Стоимость) FROM Заказы) AS Total
            FROM 
                Заказы
            INNER JOIN 
                Клиенты ON Заказы.id_Клиенты = Клиенты.id_Клиенты
            INNER JOIN 
                Автомобили ON Заказы.id_Автомобили = Автомобили.id_Автомобили
            INNER JOIN 
                Модель ON Автомобили.id_Модель = Модель.id_Модель
            INNER JOIN 
                Сотрудники ON Автомобили.id_Сотрудники = Сотрудники.id_Сотрудники;");

                return queryResult.ToList();
            }
        }

    }
}
