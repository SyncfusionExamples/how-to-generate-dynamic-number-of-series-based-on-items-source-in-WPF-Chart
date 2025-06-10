using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Common_ItemsSource_WPF
{
    // The DataTemplateSelector for the mulitple series is defined.
    public class SeriesDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ColumnSeriesTemplate { get; set; }
        public DataTemplate LineSeriesTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //Check required conditions and return the selected template.

            DataTemplate selectedDataTemplate = null;

            string year = (item as AnnualPriceData).Year;

            if (year == "2015" || year == "2016")
                selectedDataTemplate = LineSeriesTemplate;
            else
                selectedDataTemplate = ColumnSeriesTemplate;

            return selectedDataTemplate;
        }
    }
}
