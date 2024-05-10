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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KPTaxiApplication.Views
{
    /// <summary>
    /// Interaction logic for PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Page
    {
        public PersonalAccount()
        {
            InitializeComponent();
            txtStatus.Text = $"Стаутс: {CurrentUser.Status}";
            txtNameUser.Text = $"Ф.И.О: {CurrentUser.FirstName} {CurrentUser.SurName} {CurrentUser.Patronies}";
            txtNumberPhone.Text = $"Номер телефона: {CurrentUser.PhoneNumber}";
            txtMail.Text = $"Email: {CurrentUser.Email}";
            txtMail.Text = $"Паспортные данные: {CurrentUser.Pasport}";
            txtOpicanie.Text = $"Описание области: {CurrentUser.Opicanie}";



        }
    }
}
