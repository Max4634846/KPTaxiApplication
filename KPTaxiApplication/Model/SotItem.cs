using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KPTaxiApplication.Model
{
    public class SotItem
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public BitmapImage Image => new BitmapImage(new Uri(ImagePath));

    }
}
