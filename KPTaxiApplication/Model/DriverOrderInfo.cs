using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPTaxiApplication.Model
{
    public class DriverOrderInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderCost { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int id_avto { get; set; }
    }
}
