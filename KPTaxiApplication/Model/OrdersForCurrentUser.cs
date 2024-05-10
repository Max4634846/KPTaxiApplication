using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPTaxiApplication.Model
{
    public class OrdersForCurrentUser
    {
        public int id_Order { get; set; }
        public int id_Avto { get; set; }
        public int id_Client { get; set; }
        public int id_Taff{ get; set; }
        public string Otkuda { get; set; }
        public string Kuda { get; set; }
        public string StartTime { get; set; }
        public string FinishTimne { get; set; }
        public string EndTime { get; set; }
        public DateTime Date { get; set; }
        public float PutKm { get; set; }
        public string StatusOrder { get; set; }
        public decimal Price { get; set; }
        public bool Bagaje { get; set; }
        public string ViborOplate { get; set; }
        public bool Expectation { get; set; }
        public int Estimation { get; set; }
    }
}
