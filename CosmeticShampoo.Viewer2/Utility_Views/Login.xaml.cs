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
using System.Windows.Shapes;

namespace CosmeticShampoo.Viewer2.Utility_Views
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        public MainWindow Parent { get; set; }
        public Login(MainWindow parent = null)
        {
            Parent = parent;
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

        private void comfirmbtn_Click(object sender, RoutedEventArgs e)
        {
            string id = "bong";
            string pw = "1";

            if (id == username.Text)
            {
                if(pw == password.Text)
                {
                    if(Parent != null)
                    {

                        this.Hide();
                        Parent.ScreenLock.Visibility = Visibility.Collapsed;
                        return;
                    }
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("아이디 혹은 비밀번호가 틀립니다.");
                }

            }
            else
            {
                MessageBox.Show("아이디 혹은 비밀번호가 틀립니다.");
            }
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
