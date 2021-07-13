using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using Syncfusion.UI.Xaml.Charts;

namespace Common_ItemsSource_WPF
{
    public class SfChartExt : SfChart
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(SfChartExt), new PropertyMetadata(null, OnSourceChanged));

        public static readonly DependencyProperty SeriesTemplateProperty =
            DependencyProperty.Register("SeriesTemplate", typeof(DataTemplate), typeof(SfChartExt), new PropertyMetadata(null, OnSeriesTemplateChanged));

        public static readonly DependencyProperty SeriesTemplateSelectorProperty =
          DependencyProperty.Register("SeriesTemplateSelector", typeof(SeriesDataTemplateSelector), typeof(SfChartExt), new PropertyMetadata(null, OnSeriesTemplateChanged));

        //Gets or sets the ItemsSource of collection of collections.
        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        //Gets or sets the template for the series to be generated.
        public DataTemplate SeriesTemplate
        {
            get { return (DataTemplate)GetValue(SeriesTemplateProperty); }
            set { SetValue(SeriesTemplateProperty, value); }
        }
        
        //Get or sets the DataTemplateSelector for the multiple series generation.
        public SeriesDataTemplateSelector SeriesTemplateSelector
        {
            get { return (SeriesDataTemplateSelector)GetValue(SeriesTemplateSelectorProperty); }
            set { SetValue(SeriesTemplateSelectorProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SfChartExt).GenerateSeries();
        }

        private static void OnSeriesTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SfChartExt).GenerateSeries();
        }

        //Generate the series per counts in the itemssource.
        private void GenerateSeries()
        {
            if (Source == null || (SeriesTemplateSelector == null && SeriesTemplate == null)) return;
            var commonItemsSource = (Source as IEnumerable).GetEnumerator();

            while (commonItemsSource.MoveNext())
            {
                ChartSeries series = null;

                //The conditions checked for setting the SeriesTemplate or SeriesTemplateSelector.
                if (SeriesTemplate != null)
                {
                    series = SeriesTemplate.LoadContent() as ChartSeries;
                }
                else if (SeriesTemplateSelector != null)
                {
                    var selectedseriesTemplate = SeriesTemplateSelector.SelectTemplate(commonItemsSource.Current, null);
                    series = selectedseriesTemplate.LoadContent() as ChartSeries;
                }

                series.DataContext = commonItemsSource.Current;
                Series.Add(series);
            }
        }
    }
}
