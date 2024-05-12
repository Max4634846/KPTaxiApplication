using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPTaxiApplication.Model
{
    public class OrdersInfo
    {
        public int IdClient { get; set; }
        public string NameClient { get; set; }
        public string SurNameClient { get; set; }
        public string PatroniecClient { get; set; }
        public DateTime Date { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public decimal OrderCosts { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string God { get; set; }
        public int IdAvto { get; set; }
        public int IdSot { get; set; }
        public string SurNameVod { get; set; }
        public string FirstNameVod { get; set; }
        public string PatroniecVod { get; set; }
        public decimal Total { get; set; }
    }
}
