using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPTaxiApplication.Views;

namespace KPTaxiApplication.Model
{
    public class ClientForCurrentUser
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Otzestvo { get; set; }
        public string NumberPhone { get; set; }


    }
}
