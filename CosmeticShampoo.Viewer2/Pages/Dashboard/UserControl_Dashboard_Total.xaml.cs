using CosmeticShampoo.Viewer2.Pages.Dashboard;
using System.Windows;
using System.Windows.Controls;

namespace CosmeticShampoo.Viewer2.Pages
{
    /// <summary>
    /// UserControl_Dashboard_Total.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Dashboard_Total : UserControl
    {
        UserControl_Dashboard parent;
        UserControl_Dashboard_OrderList orderList;
        UserControl_Dashboard_Statistics statistics;
        public UserControl_Dashboard_Total(UserControl_Dashboard Parent)
        {
            parent = Parent;
            orderList = new UserControl_Dashboard_OrderList(this);
            statistics = new UserControl_Dashboard_Statistics(this);
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            All_Clear();
            OrderListView.Children.Add(orderList);
            StatisticView.Children.Add(statistics);
        }

        private void All_Clear()
        {
            StatisticView.Children.Clear();
            OrderListView.Children.Clear();
        }
    }
}
