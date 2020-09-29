using CosmeticShampoo.Server.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CosmeticShampoo.Server.UserControls.MainViewer
{
    /// <summary>
    /// UserControl_Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Main : UserControl
    {
        private Image start { get; set; }

        TcpListener server = null;
        TcpClient clientSocket = null;
        

        TcpListener server2 = null;
        TcpClient clientSocket2 = null;
        

        TcpListener server3 = null;
        TcpClient clientSocket3 = null;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();


        private Log log = new Log();

        delegate void stopMachineryDelegate(); //delegate를 정의하고
        private stopMachineryDelegate stopMachinery;


        

        private bool isOn { get; set; } = false;

        public UserControl_Main()
        {
            InitializeComponent();
            this.DataContext = log;

            
        }


        

        

        public void StartUpServer()
        {
            Thread t = new Thread(InitSocket);
            t.IsBackground = true;
            Thread t2 = new Thread(InitSocket2);
            t2.IsBackground = true;
            Thread t3 = new Thread(InitSocket3);
            t3.IsBackground = true;

            t.Start();
            t2.Start();
            t3.Start();
        }
        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            start = StartBtn.Template.FindName("ButtonBackground", StartBtn) as Image;
        }

        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            

            if (isOn == false)
            {
                isOn = true;

                start.Source = new BitmapImage(new Uri("pack://application:,,,/CosmeticShampoo.Server;component/Images/Icons/stop.png"));

                //PC + 안드로이드 클라이언트 서버 오픈

                StartUpServer();

                //PLC서버에 붙히기


            }
            else
            {
                this.stopMachinery();
                start.Source = new BitmapImage(new Uri("pack://application:,,,/CosmeticShampoo.Server;component/Images/Icons/play-button.png"));
                isOn = false;
                log.logView += "End Server.\n";
            }
            
            


        }

        private void InitSocket()
        {
            log.logView += "Server1 Start...\n";
            server = new TcpListener(IPAddress.Any, 9991);
            this.stopMachinery += server.Stop;
            clientSocket = default(TcpClient);
            server.Start();
            log.logView += "Server1 opened successfully\n";

            while (true)
            {
                try
                {
                    log.logView += "Server1 is waiting for clients...\n";
                    clientSocket = server.AcceptTcpClient(); // client 소켓 접속 허용
                    

                    NetworkStream stream = clientSocket.GetStream();

                    byte[] buffer = new byte[1024]; // 버퍼
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$")); // client 사용자 명

                    clientList.Add(clientSocket, user_name); // cleint 리스트에 추가

                    SendMessageAll(user_name + " 님이 입장하셨습니다.", "", false); // 모든 client에게 메세지 전송

                    HandleClient h_client = new HandleClient(); // 클라이언트 추가
                    h_client.OnReceived += new HandleClient.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new HandleClient.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.startClient(clientSocket, clientList);
                }

                catch (SocketException se) { break; }

                catch (Exception ex) { break; }

            }

           if(clientSocket != null)
            {
                clientSocket.Close(); // client 소켓 닫기
            }

            
            log.logView += "Server1 closed\n";

        }

        private void InitSocket2()
        {
            log.logView += "Server2 Start...\n";
            server2 = new TcpListener(IPAddress.Any, 9992);
            this.stopMachinery += server2.Stop;
            clientSocket2 = default(TcpClient);
            server2.Start();
            log.logView += "Server2 opened successfully\n";

            while (true)
            {
                try
                {
                    log.logView += "Server2 is waiting for clients...\n";
                    clientSocket2 = server2.AcceptTcpClient(); // client 소켓 접속 허용


                    NetworkStream stream = clientSocket2.GetStream();

                    byte[] buffer = new byte[1024]; // 버퍼
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$")); // client 사용자 명

                    clientList.Add(clientSocket2, user_name); // cleint 리스트에 추가

                    SendMessageAll(user_name + " 님이 입장하셨습니다.", "", false); // 모든 client에게 메세지 전송

                    HandleClient2 h_client = new HandleClient2(); // 클라이언트 추가
                    h_client.OnReceived += new HandleClient2.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new HandleClient2.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.startClient(clientSocket2, clientList);
                }

                catch (SocketException se) { break; }

                catch (Exception ex) { break; }

            }

            if (clientSocket2 != null)
            {
                clientSocket2.Close(); // client 소켓 닫기
            }
            
            log.logView += "Server2 closed\n";

        }

        private void InitSocket3()
        {
            log.logView += "Server3 Start...\n";
            server3 = new TcpListener(IPAddress.Any, 9993);
            this.stopMachinery += server3.Stop;
            clientSocket3 = default(TcpClient);
            server3.Start();
            log.logView += "Server3 opened successfully\n";

            while (true)
            {
                try
                {
                    log.logView += "Server3 is waiting for clients...\n";
                    clientSocket3 = server3.AcceptTcpClient(); // client 소켓 접속 허용


                    NetworkStream stream = clientSocket3.GetStream();

                    byte[] buffer = new byte[1024]; // 버퍼
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = "Dashboard"; // client 사용자 명

                    clientList.Add(clientSocket3, user_name); // cleint 리스트에 추가

                    SendMessageAll(user_name + " 님이 입장하셨습니다.", "", false); // 모든 client에게 메세지 전송

                    HandleClient3 h_client = new HandleClient3(); // 클라이언트 추가
                    h_client.OnReceived += new HandleClient3.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new HandleClient3.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.SendData += new HandleClient3.DataSendHandler(SendData);
                    h_client.startClient(clientSocket3, clientList);
                }

                catch (SocketException se) { break; }

                catch (Exception ex) { break; }

            }

            if (clientSocket3 != null)
            {
                clientSocket3.Close(); // client 소켓 닫기
            }
            
            log.logView += "Server3 closed\n";

        }

        

        void h_client_OnDisconnected(TcpClient clientSocket) // cleint 접속 해제 핸들러
        {
            if (clientList.ContainsKey(clientSocket))
                clientList.Remove(clientSocket);
        }

        private void OnReceived(string message, string user_name) // cleint로 부터 받은 데이터
        {
            if (message.Equals("leaveChat"))
            {
                string displayMessage = "leave user : " + user_name;
                log.logView += displayMessage + "\n";
                SendMessageAll("leaveChat", user_name, true);
            }
            else
            {
                string displayMessage = user_name + " : " + message;
                log.logView += displayMessage + "\n"; // Server단에 출력
                SendMessageAll(message, user_name, true); // 모든 Client에게 전송
            }
        }

        public void SendMessageAll(string message, string user_name, bool flag)
        {
            foreach (var pair in clientList)
            {
                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();
                byte[] buffer = null;

                if (flag)
                {
                    if (message.Equals("leaveChat"))
                        buffer = Encoding.Unicode.GetBytes(user_name + " 님이 대화방을 나갔습니다.");
                    else
                        buffer = Encoding.Unicode.GetBytes(user_name + " : " + message);
                }
                else
                {
                    buffer = Encoding.Unicode.GetBytes(message);
                }

                stream.Write(buffer, 0, buffer.Length); // 버퍼 쓰기
                stream.Flush();
            }
        }

        public void SendData(JArray data)
        {
            var pair = from list in clientList
                       where list.Value == "Dashboard"
                       select list.Key;

            TcpClient client = pair as TcpClient;
            NetworkStream stream = client.GetStream();

            byte[] send_data = new byte[23500];
            string send_st = JsonConvert.SerializeObject(data);
            send_data = Encoding.Unicode.GetBytes(send_st);

            stream.Write(send_data, 0, send_data.Length);
            stream.Flush();

        }



    }
}
