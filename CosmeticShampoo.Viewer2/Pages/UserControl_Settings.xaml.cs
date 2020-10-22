using CosmeticShampoo.Viewer2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// UserControl_Settings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Settings : UserControl
    {
        public MainWindow _Parent;
        public UserControl_Settings(MainWindow parent)
        {
            _Parent = parent;
            InitializeComponent();
        }
        public delegate bool connectedHandler();
        public event connectedHandler OnConnected;

        public delegate bool DisconnectedHandler();
        public event DisconnectedHandler OnDisconnected;
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (OnConnected())
            {
                ConnetionCheckCircle.Fill = new SolidColorBrush(Colors.Green);
                ConnectBtn.Click -= Connect_Click;
                ConnectBtn.Click += Disconnect_Click;
                ConnectBtn.Content = "중지";
            }

        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {

            if (OnDisconnected())
            {
                ConnetionCheckCircle.Fill = new SolidColorBrush(Colors.Red);
                ConnectBtn.Click -= Disconnect_Click;
                ConnectBtn.Click += Connect_Click;
                ConnectBtn.Content = "시작";
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            
            SocketPopUp addUser = new SocketPopUp(this);

            _Parent.Opacity = 0.6;
            addUser.ShowDialog();
        }
    }
}
