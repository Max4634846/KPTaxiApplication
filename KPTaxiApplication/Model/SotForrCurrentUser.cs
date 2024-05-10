using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPTaxiApplication.Model
{
    public class SotForrCurrentUser
    {
        public int id_Sot { get; set; }
        public int id_Dol { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patroniec { get; set; }
        public string Status { get; set; }
        public float Rating { get; set; }
        public string NumPhone { get; set; }
        public string Email { get; set; }
        public string Pasport { get; set; }
        public string Address { get; set; }
       
    }
}
