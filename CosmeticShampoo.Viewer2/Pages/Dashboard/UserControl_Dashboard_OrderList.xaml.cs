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
        TcpClient clientSocket = new TcpClient(); // 소켓

        NetworkStream stream = default(NetworkStream);

        string message = string.Empty;

        UserControl_Dashboard_Total total;
        public UserControl_Dashboard_OrderList(UserControl_Dashboard_Total Total)
        {
            total = Total;
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try

            {

                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9991);

                clientSocket.Connect(serverAddress); // 접속 IP 및 포트

                stream = clientSocket.GetStream();

            }

            catch (Exception e2)

            {

                MessageBox.Show("서버가 실행중이 아닙니다.", "연결 실패!");

                Application.Current.Shutdown();
                Environment.Exit(0);

            }



            //클라이언트 이름
            byte[] buffer = Encoding.Unicode.GetBytes("OrderTable$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();


            Thread Receive_handler = new Thread(SendMessage);
            Receive_handler.IsBackground = true;
            Receive_handler.Start();

            Thread Get_handler = new Thread(GetData);
            Get_handler.IsBackground = true;
            Get_handler.Start();



        }

        private void SendMessage()
        {
            while (true)
            {
                Thread.Sleep(2000);
                byte[] buffer = Encoding.Unicode.GetBytes("ShowOrders$");
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            
        }

        private void GetData() // 메세지 받기
        {

            while (true)
            {

                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);


                string message = Encoding.Unicode.GetString(buffer, 0, bytes);

                //Json 거르기
                if (message.StartsWith("["))
                {

                    List<Orders> orderlists = new List<Orders>();
                    JArray array = JsonConvert.DeserializeObject<JArray>(message);

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


                    continue;
                }

            }


        }
    }
}
