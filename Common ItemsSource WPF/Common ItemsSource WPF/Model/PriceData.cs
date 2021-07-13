using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace Common_ItemsSource_WPF
{
    public class PriceData
    {
        public string Component { get; set; }

        public double Price { get; set; }
    }

    public class AnnualPriceData
    {
        public string Year { get; set; }

        public ObservableCollection<PriceData> PriceCollection { get; set; }     
    }
}
