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
            t.Start();
           
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
                    log.logView += (user_name +" is accepted.\n");

                    HandleClient h_client = new HandleClient(); // 클라이언트 추가
                    h_client.OnReceived += new HandleClient.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new HandleClient.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.OnSent += new HandleClient.SendJsonHandler(OnSent);
                    h_client.OnSentLoginInfo += new HandleClient.SendLoginJsonHandler(OnSentLoginInfo);
                    h_client.startClient(clientSocket, clientList);
                }
                catch (SocketException se)
                {
                    break; 

                }catch (Exception ex) 
                {
                    break; 
                }

            }

           if(clientSocket != null)
            {
                clientSocket.Close(); // client 소켓 닫기
            }

            
            log.logView += "Server1 closed\n";

        }

        private void OnSentLoginInfo(bool isEnable, JArray jArray)
        {
            var pair = from list in clientList
                       where list.Value == "Login"
                       select list.Key;

            TcpClient client = pair.FirstOrDefault() as TcpClient;
            NetworkStream stream = client.GetStream();

            string JsonString = null;
            byte[] send_JsonData = new byte[23500];


            if (isEnable == true)
            {
                JsonString = "true$" + JsonConvert.SerializeObject(jArray, Formatting.Indented);
                send_JsonData = Encoding.Unicode.GetBytes(JsonString);
            }
            else
            {
                JsonString = "false$";
                send_JsonData = Encoding.Unicode.GetBytes(JsonString);
            }
            
            
            stream.Write(send_JsonData, 0, send_JsonData.Length);
            stream.Flush();
        }

        private void OnSent(JArray jArray)
        {
            var pair = from list in clientList
                       where list.Value == "Dashboard"
                       select list.Key;
            
            TcpClient client = pair.FirstOrDefault() as TcpClient;
            NetworkStream stream = client.GetStream();

            byte[] send_JsonData = new byte[23500];
            string JsonString = JsonConvert.SerializeObject(jArray, Formatting.Indented);
            string JsonWithHeader = "OrderList$" + JsonString;
            send_JsonData = Encoding.Unicode.GetBytes(JsonWithHeader);
            stream.Write(send_JsonData, 0, send_JsonData.Length);
            stream.Flush();
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
                string displayMessage = "leave user : " + user_name +"\n";
                log.logView += displayMessage;
                //SendMessageAll("leaveChat", user_name, true);
            }
            else
            {
                string displayMessage = user_name + " : " + message;
                log.logView += displayMessage + "\n"; // Server단에 출력
                //SendMessageAll(message, user_name, true); // 모든 Client에게 전송
            }
        }

       



    }
}
