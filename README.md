# How to generate dynamic number of series based on common items source in WPF Chart (SfChart)?

This article describes how to generate dynamic number of series by binding a collection (Items source) directly to chart.

You can generate dynamic number of series by extending the WPF Chart (inheriting SfChart) and providing it with the SeriesTemplate (for customizing series type and its properties) and Source properties.

The implementation process is explained in the following steps.

**Step 1:** Define chart’s view model setup

This step describes how to define the chart’s collection (items source) through MVVM.

**Model**
```
public class PriceData
{   
    public string Component { get; set; }
    public double Price { get; set; }
}
```
```
public class AnnualPriceData
{
    public string Year { get; set; }
    public ObservableCollection<PriceData> PriceCollection { get; set; }
}
```

**View Model**
```
public class ViewModel
{
  public ObservableCollection<AnnualPriceData> AnnualPriceCollection { get; set; }
  public ViewModel()
  {
    AnnualPriceCollection = new ObservableCollection<AnnualPriceData>();
    AnnualPriceCollection.Add(new AnnualPriceData()
    {
      Year = "2012",
      PriceCollection = new ObservableCollection<PriceData>()
      {
        new PriceData() {Component = "Hard Disk", Price = 80 },
        new PriceData() {Component = "Scanner", Price = 140  },
        new PriceData() {Component = "Monitor", Price = 150  },
        new PriceData() {Component = "Printer", Price = 180  },
      }
    });
            
    AnnualPriceCollection.Add(new AnnualPriceData()
    {
      Year = "2013",
      PriceCollection = new ObservableCollection<PriceData>()
      {
        new PriceData() {Component = "Hard Disk", Price = 87 },
        new PriceData() {Component = "Scanner", Price = 157  },
        new PriceData() {Component = "Monitor", Price = 155  },
        new PriceData() {Component = "Printer", Price = 192  },
      }
    });
 
    AnnualPriceCollection.Add(new AnnualPriceData()
    {
      Year = "2014",
      PriceCollection = new ObservableCollection<PriceData>()
      {
        new PriceData() {Component = "Hard Disk", Price = 95 },
        new PriceData() {Component = "Scanner", Price = 150  },
        new PriceData() {Component = "Monitor", Price = 163  },
        new PriceData() {Component = "Printer", Price = 185  },
      }
    });
 
    AnnualPriceCollection.Add(new AnnualPriceData()
    {
      Year = "2015",
      PriceCollection = new ObservableCollection<PriceData>()
      {
        new PriceData() {Component = "Hard Disk", Price = 113 },
        new PriceData() {Component = "Scanner", Price = 165  },
        new PriceData() {Component = "Monitor", Price = 175  },
        new PriceData() {Component = "Printer", Price = 212  },
      }
    });
 
    AnnualPriceCollection.Add(new AnnualPriceData()
    {
      Year = "2016",
      PriceCollection = new ObservableCollection<PriceData>()
      {
        new PriceData() {Component = "Hard Disk", Price = 123 },
        new PriceData() {Component = "Scanner", Price = 169 },
        new PriceData() {Component = "Monitor", Price = 184 },
        new PriceData() {Component = "Printer", Price = 224 },
      }
    });
  }
}
```

**Step 2:** Implement SfChart extension

To achieve this requirement, use the custom [WPF Chart (SfChart)](https://help.syncfusion.com/wpf/charts/getting-started), which inherits from SfChart and defines two properties, namely the **Source** and **SeriesTemplate** properties.

**Source:**

The **Source** property is used to bind the items source (collection of collections) to chart. The chart series will be generated per item in the collection.
```
//Gets or sets the ItemsSource (collection of collections).
public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(SfChartExt), new PropertyMetadata(null, OnSourceChanged));
 
public object Source
{
  get { return (object)GetValue(SourceProperty); }
  set { SetValue(SourceProperty, value); }
}
```
**SeriesTemplate:**

The **SeriesTemplate** property is used to define the type and visual appearance of chart series. This template is more flexible, and it allows you to define any type of series and all its properties since the content of the template is the series.

```
//Gets or sets the template for the series to be generated.
public static readonly DependencyProperty SeriesTemplateProperty = DependencyProperty.Register("SeriesTemplate", typeof(DataTemplate), typeof(SfChartExt), new PropertyMetadata(null, OnSeriesTemplateChanged));
 
public DataTemplate SeriesTemplate
{
  get { return (DataTemplate)GetValue(SeriesTemplateProperty); }
  set { SetValue(SeriesTemplateProperty, value); }
}
```

The following code illustrates generating dynamic number of series based on data(collection).
```
public class SfChartExt :  SfChart
{
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(SfChartExt), new PropertyMetadata(null, OnPropertyChanged));
  public static readonly DependencyProperty SeriesTemplateProperty = DependencyProperty.Register("SeriesTemplate", typeof(DataTemplate), typeof(SfChartExt), new PropertyMetadata(null, OnPropertyChanged));
 
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
  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)     
  {
    (d as SfChartExt).GenerateSeries();
  }
  //Generate the series per the counts in the itemssource.
  private void GenerateSeries()
  {
    if (Source == null || SeriesTemplate == null) 
      return;                
    var commonItemsSource = (Source as IEnumerable).GetEnumerator();
    while(commonItemsSource.MoveNext())
    {
       ChartSeries series = SeriesTemplate.LoadContent() as ChartSeries;
       series.DataContext = commonItemsSource.Current;
       Series.Add(series);
    }   
  }
}
```

```
<local:SfChartExt Source="{Binding AnnualPriceCollection}" >
<--The SeriesTemplate is defined for generating the series.-->
    <local:SfChartExt.SeriesTemplate>
          <DataTemplate>
               <chart:ColumnSeries XBindingPath="Component"     
                                                    YBindingPath="Price"     
                                                    ItemsSource="{Binding PriceCollection}"
                                                    Label="{Binding Year}"/>
          </DataTemplate>
    </local:SfChartExt.SeriesTemplate>
<--Define required chart properties.-->
</local:SfChartExt> 
```

The following illustrates the result of above code examples.

![image](https://user-images.githubusercontent.com/53489303/200557020-101820b2-580b-4120-81d2-9cbb32133b63.png)

**To generate different types of chart series**

You can generate multiple types of series by using the **DataTemplateSelector** property. A new class of **SeriesDataTemplateSelector** can be created by inheriting the DataTemplateSelector to achieve this requirement.

```
public class SeriesDataTemplateSelector : DataTemplateSelector
{
   public DataTemplate ColumnSeriesTemplate { get; set; }
   public DataTemplate LineSeriesTemplate { get; set; }
   public override DataTemplate SelectTemplate(object item, DependencyObject container)
   {
       //Check required conditions and return the selected template.
       DataTemplate selectedDataTemplate = null;
       string year = (item as AnnualPriceData).Year;
       if (year == "2015" || year =="2016")
         selectedDataTemplate = LineSeriesTemplate;
       else
         selectedDataTemplate = ColumnSeriesTemplate;
       return selectedDataTemplate;    
   }       
}
```

**SfChart extension with SeriesTemplateSelector:**

The **SeriesTemplateSelector** property is used to select the different series template in single chart based on item.
```
public class SfChartExt : SfChart
{
  public static readonly DependencyProperty SeriesTemplateSelectorProperty = DependencyProperty.Register("SeriesTemplateSelector", typeof(SeriesDataTemplateSelector), typeof(SfChartExt), new PropertyMetadata(null, OnPropertyChanged));
 
  //Get or sets the DataTemplateSelector for the multiple series generation.
  public SeriesDataTemplateSelector SeriesTemplateSelector
  {
   get { return (SeriesDataTemplateSelector)GetValue(SeriesTemplateSelectorProperty);}
   set { SetValue(SeriesTemplateSelectorProperty, value); }
  }  
 
  //Generate the series according to the counts in the itemssource.
  private void GenerateSeries()
  {
    if (Source == null || (SeriesTemplateSelector == null && SeriesTemplate == null)) 
      return;     
    var commonItemsSource = (Source as IEnumerable).GetEnumerator();
    while(commonItemsSource.MoveNext())
    {
       ChartSeries series = null;
       //The conditions checked for setting the SeriesTemplate or SeriesTemplateSelector.
       if (SeriesTemplate != null)
       {
          series = SeriesTemplate.LoadContent() as ChartSeries;
       }
       else if(SeriesTemplateSelector !=null)
       {
          var selectedseriesTemplate = SeriesTemplateSelector.SelectTemplate(commonItemsSource.Current, null);     
          series = selectedseriesTemplate.LoadContent() as ChartSeries;
       }
       series.DataContext = commonItemsSource.Current;
       Series.Add(series);
    }
  }
}
```

**SeriesDataTemplateSelector definition:**

The different types of templates are defined in the resources for the **SeriesTemplateSelector** property.
```
<Grid.Resources>
    <DataTemplate x:Key="columnSeriesTemplate">
        <chart:ColumnSeries XBindingPath="Component" YBindingPath="Price" 
                                          ItemsSource="{Binding PriceCollection}"
                                          Label="{Binding Year}"/>
    </DataTemplate>
    <DataTemplate x:Key="lineSeriesTemplate" >
        <chart:LineSeries XBindingPath="Component" YBindingPath="Price" 
                                    ItemsSource="{Binding PriceCollection}"
                                    Label="{Binding Year}"/>
    </DataTemplate>
</Grid.Resources> 
 
<local:SfChartExt Source="{Binding AnnualPriceCollection}">
     <!--The SeriesDataTemplateSelector is defined for generating the series.-->
     <local:SfChartExt.SeriesTemplateSelector>
                <local:SeriesDataTemplateSelector 
                                                  ColumnSeriesTemplate="{StaticResource
                                                  columnSeriesTemplate}"                    
                                                  LineSeriesTemplate="{StaticResource 
                                                  lineSeriesTemplate}" />
     </local:SfChartExt.SeriesTemplateSelector> 
</local:SfChartExt> 
```

The following column and line series (multiple series) are created as the result of above code examples.

![image](https://user-images.githubusercontent.com/53489303/200557340-1f010c57-0245-4cb4-b99f-871876ceb2bc.png)

KB article - [How to generate dynamic number of series based on common items source in WPF Chart (SfChart)?](https://www.syncfusion.com/kb/7578/how-to-generate-dynamic-number-of-series-based-on-common-items-source-in-wpf-chart-sfchart)
