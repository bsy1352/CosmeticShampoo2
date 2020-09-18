using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;


namespace CosmeticShampoo.Viewer2.Pages.Dashboard
{
    /// <summary>
    /// UserControl_Dashboard_Statistics.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Dashboard_Statistics : UserControl
    {
        UserControl_Dashboard_Total total;
        public UserControl_Dashboard_Statistics(UserControl_Dashboard_Total Total)
        {
            InitializeComponent();
            total = Total;
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2020",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            SeriesCollection.Add(new LineSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42, 50 },
                Fill = Brushes.Transparent
            }) ;


            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "09.11", "09.12", "09.13", "09.14" };
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        
    }
}