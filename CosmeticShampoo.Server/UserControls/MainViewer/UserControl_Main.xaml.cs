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
        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();
        private Log log = new Log();

        private bool isOn { get; set; } = false;

        public UserControl_Main()
        {
            InitializeComponent();
            this.DataContext = log;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            start = StartBtn.Template.FindName("ButtonBackground", StartBtn) as Image;
        }

        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            if(isOn == false)
            {
                isOn = true;

                start.Source = new BitmapImage(new Uri("pack://application:,,,/CosmeticShampoo.Server;component/Images/Icons/stop.png"));

                //PC + 안드로이드 클라이언트 서버 오픈

                Thread t = new Thread(InitSocket);
                t.IsBackground = true;
                t.Start();

                //PLC서버에 붙히기
            }
            else
            {
                server.Stop();
                start.Source = new BitmapImage(new Uri("pack://application:,,,/CosmeticShampoo.Server;component/Images/Icons/play-button.png"));
                isOn = false;
            }
            
            


        }

        private void InitSocket()
        {
            log.logView += "서버 시작...\n";
            server = new TcpListener(IPAddress.Any, 9999);
            clientSocket = default(TcpClient);
            server.Start();
            log.logView += "서버 오픈 성공\n";

            while (true)
            {
                try
                {
                    log.logView += "클라이언트 대기중\n";
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
            server.Stop(); // 서버 종료

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
                string displayMessage = "From client : " + user_name + " : " + message;
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



    }
}
