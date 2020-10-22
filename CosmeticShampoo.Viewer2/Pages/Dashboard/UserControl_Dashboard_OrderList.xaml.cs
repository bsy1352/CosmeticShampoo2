using CosmeticShampoo.Viewer2.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CosmeticShampoo.Viewer2.Pages
{
    /// <summary>
    /// UserControl_Dashboard_OrderList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Dashboard_OrderList : UserControl
    {
        
        string message = string.Empty;

        public UserControl_Dashboard_Total total;

        public delegate void SendMessage(string msg);
        public event SendMessage SendMsg;

        bool isDisconnected;

        public UserControl_Dashboard_OrderList(UserControl_Dashboard_Total Total)
        {
            total = Total;
            total.Parent.Parent.ShowOrders += new MainWindow.ShowOrdersHandler(GetData);

            Thread newTh = new Thread(OnSend);
            newTh.IsBackground = true;
            newTh.Start();

            InitializeComponent();
            
        }

        

        private void OnSend()
        {
            while (true)
            {
                if (!total.Parent.Parent.isConnected)
                {
                    continue;
                }

                Thread.Sleep(100);
                SendMsg("ShowOrders");
            }
        }



        private void GetData(string msg) // 메세지 받기
        {

            List<Orders> orderlists = new List<Orders>();
            JArray array = JsonConvert.DeserializeObject<JArray>(msg);

            foreach (JObject data in array.Reverse())
            {
                string datas = JsonConvert.SerializeObject(data);
                orderlists.Add(JsonConvert.DeserializeObject<Orders>(datas));
            }

            OrderGrid.Dispatcher.BeginInvoke(new Action(delegate
            {
                OrderGrid.ItemsSource = null;
                OrderGrid.Items.Refresh();
                OrderGrid.ItemsSource = orderlists;

            }));

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
