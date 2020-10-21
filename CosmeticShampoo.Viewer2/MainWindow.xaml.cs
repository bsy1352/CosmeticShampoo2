using CosmeticShampoo.Viewer2.Pages;
using CosmeticShampoo.Viewer2.Utility_Views;
using CosmeticShampoo.Viewer2.ViewModels;
using MahApps.Metro.IconPacks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace CosmeticShampoo.Viewer2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        UserMenu Dashboard;
        UserMenuDropDown ProgramSetting;

        UserMenuDropDown Statistics;

        UserControl_Settings settings;

        TcpClient clientSocket; // 소켓

        NetworkStream stream = default(NetworkStream);

        string message = string.Empty;

        public delegate void ShowOrdersHandler(string msg);
        public event ShowOrdersHandler ShowOrders;
        public MainWindow()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();

            
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("종료하시겠습니까?", "Exit", MessageBoxButton.OKCancel);

            if (messageBoxResult == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
            
        }

        internal void SwitchScreens(object sender)
        {
            var screen = (UserControl)sender;


            if (screen != null)
            {
                MainView.Children.Clear();
                MainView.Children.Add(screen);
                return;
            }
        }

        private string getIpAddress()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Environment.CurrentDirectory + "\\xml\\Settings.xml");
            XmlNode xNode = xmlDoc.SelectSingleNode("/descendant::Settings/IpAddress");

            string ipAddress = xNode.InnerText;


            return ipAddress;
        }

        private string getPort()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Environment.CurrentDirectory + "\\xml\\Settings.xml");
            XmlNode xNode = xmlDoc.SelectSingleNode("/descendant::Settings/Port");

            string Port = xNode.InnerText;


            return Port;
        }

        private bool disconnectToServer()
        {
            try
            {
                byte[] buffer = Encoding.Unicode.GetBytes("leaveChat" + "$");
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();

                Thread.Sleep(1000);
                stream.Close();
                clientSocket.Close();
                stream.Dispose();
                clientSocket.Dispose();


            }catch(Exception ex)
            {
                return false;
            }
            

            return true;
        }
        private bool connectToServer()
        {
            try
            {

                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(getIpAddress()), Convert.ToInt32(getPort()));
                clientSocket = new TcpClient();
                clientSocket.Connect(serverAddress); // 접속 IP 및 포트
                stream = clientSocket.GetStream();

            }
            catch (Exception e2)
            {

                MessageBox.Show("서버가 실행중이 아닙니다.", "연결 실패!");
                return false;
            }

            
            //클라이언트 이름
            byte[] buffer = Encoding.Unicode.GetBytes("Dashboard$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Thread ReceiveMsgthread = new Thread(ReceiveMsg);
            ReceiveMsgthread.IsBackground = true;
            ReceiveMsgthread.Start();
            
            return true;
        }

        private void SendMessage(string msg)
        {

            byte[] buffer = Encoding.Unicode.GetBytes(msg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

        }

        private void ShowOrder(string msg)
        {
            ShowOrders(msg);
        }

        private void ReceiveMsg() // 메세지 받기
        {

            while (true)
            {

                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                
                string raw_message = Encoding.Unicode.GetString(buffer, 0, bytes);
                string Header = raw_message.Substring(0, raw_message.IndexOf("$"));
                string message = raw_message.Remove(0, Header.Length + 1);



                switch (Header)
                {
                    case "OrderList":
                        Thread OrderlistThread = new Thread(() => {
                            ShowOrder(message);
                        });
                        OrderlistThread.IsBackground = true;
                        OrderlistThread.Start();
                        break;

                    default:
                        break;
                }
                
               

            }


        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            settings = new UserControl_Settings(this);

            settings.OnConnected += new UserControl_Settings.connectedHandler(connectToServer);
            settings.OnDisconnected += new UserControl_Settings.DisconnectedHandler(disconnectToServer);
            //menuSetting.Add(new SubItem("Employees"));
            //menuSetting.Add(new SubItem("Products"));

            UserControl_Dashboard dashboard = new UserControl_Dashboard(this);
            dashboard.total.orderList.SendMsg += new UserControl_Dashboard_OrderList.SendMessage(SendMessage);

            var item1 = new ItemMenu("대시보드", PackIconMaterialKind.MonitorDashboard, dashboard);

            var menuProgramSetting = new List<SubItem>();
            menuProgramSetting.Add(new SubItem("관리자 설정", new UserControl_AdminSetting(this)));

            var item2 = new ItemMenu("프로그램 설정", menuProgramSetting, PackIconMaterialKind.Robot);

            var Statistics = new List<SubItem>();
            Statistics.Add(new SubItem("오류", new UserControl_Errors()));

            var item3 = new ItemMenu("통계", Statistics, PackIconMaterialKind.ChartBar);

            Dashboard = new UserMenu(item1, this);
            ProgramSetting = new UserMenuDropDown(item2, this);
            this.Statistics = new UserMenuDropDown(item3, this);
            
            this.Dashboard.userMenuDrop = this.ProgramSetting;
            this.ProgramSetting.userMenu = this.Dashboard;
            this.Statistics.userMenu = this.Dashboard;
            this.ProgramSetting.userMenuDrop = this.Statistics;
            this.Statistics.userMenuDrop = this.ProgramSetting;

            Menu.Children.Add(Dashboard);
            Menu.Children.Add(ProgramSetting);
            Menu.Children.Add(this.Statistics);

            DataContext = this;

        }

        private void PopUpMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            PopUpMenu.Visibility = Visibility.Visible;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("로그아웃 하시겠습니까?" + Environment.NewLine + "로그아웃 시, 잠금화면으로 전환됩니다", "Log Out", MessageBoxButton.OKCancel);

            if(messageBoxResult == MessageBoxResult.OK)
            {
                ScreenLock.Visibility = Visibility.Visible;
                Login login = new Login(this);
                PopUpMenu.Visibility = Visibility.Collapsed;
                this.Opacity = 0.6;
                login.ShowDialog();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PopUpMenu.Visibility = Visibility.Collapsed;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Time_Hour.Text = DateTime.Now.ToString("HH : mm : ss");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreens(settings);
        }
    }

   
}
