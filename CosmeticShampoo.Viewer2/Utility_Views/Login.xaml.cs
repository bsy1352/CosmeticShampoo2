using CosmeticShampoo.Viewer2.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using System.Windows.Shapes;

namespace CosmeticShampoo.Viewer2.Utility_Views
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window, INotifyPropertyChanged
    {
        HttpClient client = new HttpClient();
        private users _myInstance;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public users myInstance {
            get { return _myInstance; }
            set
            {
                this._myInstance = value;
                OnPropertyChanged("myInstance");
            }
        }

        public MainWindow Parent { get; set; }
        public Login(MainWindow parent = null)
        {
            Parent = parent;
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("종료하시겠습니까?", "Exit", MessageBoxButton.OKCancel);
            if(messageBoxResult == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
            
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            usernameHint.Visibility = Visibility = Visibility.Visible;
            if (username.Text.Length > 0)
            {
                usernameHint.Visibility = Visibility.Hidden;
            }
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordHint.Visibility = Visibility = Visibility.Visible;
            if (password.Text.Length > 0)
            {
                passwordHint.Visibility = Visibility.Hidden;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private async void comfirmbtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                LoginData user = new LoginData()
                {
                    //name = NameTxtBox.Text,
                    //author = AuthorTxtBox.Text,
                    //isbn = ISBNTxtBox.Text
                    UserID = username.Text,
                    UserPW = password.Text

                };

                
                var req = await client.PostAsJsonAsync("api/Login", user);
                /*req.EnsureSuccessStatusCode()*/;// 오류 코드를 던집니다.


                if (req.IsSuccessStatusCode)
                {
                    var resp = await client.GetAsync("api/Login");
                    myInstance = JsonConvert.DeserializeObject<users>(
                                            await resp.Content.ReadAsStringAsync());
                    
                    
                    MainWindow main = new MainWindow(myInstance, client);
                    main.Show();
                    this.Hide();
                }
                else
                {
                    var message = await req.Content.ReadAsStringAsync();
                    MessageBox.Show(message);
                }

                
                /*resp.EnsureSuccessStatusCode();*/ // 오류 코드를 던집니다.


                

                

                

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            

            //try
            //{
            //    IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9991);
            //    clientSocket = new TcpClient();
            //    clientSocket.Connect(serverAddress); // 접속 IP 및 포트
            //    stream = clientSocket.GetStream();

            //}
            //catch (Exception e2)
            //{
            //    MessageBox.Show("서버가 실행중이 아닙니다.", "연결 실패!");
            //    return;
            //}

            //byte[] buffer = Encoding.Unicode.GetBytes("Login$");
            //stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();

            //Thread.Sleep(1000);

            //buffer = Encoding.Unicode.GetBytes("Login$"+ username.Text+"_"+ password.Text);
            //stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();

            //int BUFFERSIZE = clientSocket.ReceiveBufferSize;
            //buffer = new byte[BUFFERSIZE];
            //int bytes = stream.Read(buffer, 0, buffer.Length);

            //string raw_message = Encoding.Unicode.GetString(buffer, 0, bytes);

            //string trueOrFalse = raw_message.Substring(0, raw_message.IndexOf("$"));
            //string UserJson = raw_message.Remove(0, trueOrFalse.Length + 1);

            //if(trueOrFalse == "true")
            //{

            //    JArray array = JsonConvert.DeserializeObject<JArray>(UserJson);


            //    string datas = JsonConvert.SerializeObject(array.First);
            //    users User = JsonConvert.DeserializeObject<users>(datas);

            //    MainWindow main = new MainWindow(User);
            //    main.Show();
            //    this.Hide();
            //}
            //else
            //{
            //    MessageBox.Show("아이디 혹은 비밀번호가 틀렸습니다.");
            //}

            //string id = "bong";
            //string pw = "1";



            //if (id == username.Text)
            //{
            //    if(pw == password.Text)
            //    {
            //        if(Parent != null)
            //        {

            //            this.Hide();
            //            Parent.ScreenLock.Visibility = Visibility.Collapsed;
            //            Parent.Opacity = 1;
            //            return;
            //        }
            //        MainWindow main = new MainWindow();
            //        main.Show();
            //        this.Hide();
            //    }
            //    else
            //    {
            //        MessageBox.Show("아이디 혹은 비밀번호가 틀립니다.");
            //    }

            //}
            //else
            //{
            //    MessageBox.Show("아이디 혹은 비밀번호가 틀립니다.");
            //}
        }

        

        private void username_GotFocus(object sender, RoutedEventArgs e)
        {
            username.Clear();
            password.Clear();
        }

        private void password_GotFocus(object sender, RoutedEventArgs e)
        {
            password.Clear();
        }

        private void EnterBtn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.comfirmbtn_Click(sender, e);
            }


        }
    }
}
