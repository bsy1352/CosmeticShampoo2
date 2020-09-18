using CosmeticShampoo.Server.UserControls.MainViewer;
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
using WinForms = System.Windows.Forms;

namespace CosmeticShampoo.Server
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl_Main main { get; set; }
        private bool isHided { get; set; }

        private System.Windows.Forms.ContextMenu menu { get; set; }
        public MainWindow()
        {
            InitializeComponent();

        }

        private System.Windows.Threading.DispatcherTimer timer;
        public System.Windows.Forms.NotifyIcon notify;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                menu = new System.Windows.Forms.ContextMenu();
                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
                System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem();
                
                
                menu.MenuItems.Add(item2);
                menu.MenuItems.Add(item3);                
                menu.MenuItems.Add(item1);
                
                

                item1.Index = 0;
                item1.Text = "Hide";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    this.Hide();
                    isHided = true;
                    menu.MenuItems[0].Enabled = false;
                };

                item2.Index = 1;
                item2.Text = "show";
                item2.Click += delegate (object click, EventArgs eClick)
                {
                    this.Show();
                    isHided = false;
                    menu.MenuItems[0].Enabled = true;
                };

                item3.Index = 2;
                item3.Text = "프로그램 종료";
                item3.Click += delegate (object click, EventArgs eClick)
                {
                    MessageBoxResult result = MessageBox.Show("종료하시겠습니까?", "Exit", MessageBoxButton.YesNo);
                    
                    if(result == MessageBoxResult.Yes)
                    {
                        Application.Current.Shutdown();
                    }
                    
                };

                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = CosmeticShampoo.Server.Properties.Resources.server_cloud;

                notify.Visible = true;
                
                
                //notify.DoubleClick +=
                //    delegate (object senders, EventArgs args)
                //    {
                //        this.Show();
                //        menu.MenuItems[0].Enabled = true;
                //        this.WindowState = WindowState.Normal;
                //    };
                notify.ContextMenu = menu;
                notify.Text = "Test";

                //timer = new System.Windows.Threading.DispatcherTimer();
                //timer.Interval = new TimeSpan(0, 0, 2);
                //timer.Tick += new EventHandler(timer_Tick);
                //timer.Start();
            }
            catch(Exception ee)
            {

            }
            



            main = new UserControl_Main();
            Main.Children.Add(main);
        }

        private int c = 0;

        void timer_Tick(object sender, EventArgs e)
        {
            notify.BalloonTipTitle = "Title";
            notify.BalloonTipText = "BalloonTip Text";
            notify.ShowBalloonTip(1000);
            ++c;
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState.Minimized.Equals(WindowState))
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        private void MenuItem_HomeClick(object sender, RoutedEventArgs e)
        {
            Main.Children.Clear();
            Main.Children.Add(main);
        }

        private void MenuItem_ExitClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("종료하시겠습니까?", "Exit", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            isHided = true;
            menu.MenuItems[0].Enabled = false;
        }
    }
}
