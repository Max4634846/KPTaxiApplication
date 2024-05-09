using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPTaxiApplication.Views;

namespace KPTaxiApplication.Model
{
    internal class CurrentUser
    {
        public static int ID { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string FirstName { get; set; }
        public static string SurName { get; set; }
        public static string Email { get; set; }
        public static string PhoneNumber { get; set; }
        public static int Status { get; set; }

        public static string GetFullName()
        {
            return $"{FirstName} {SurName} ";
        }
    }
}
