using CosmeticShampoo.Viewer2.Pages;
using CosmeticShampoo.Viewer2.Utility_Views;
using CosmeticShampoo.Viewer2.ViewModels;
using MahApps.Metro.IconPacks;
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
using System.Windows.Threading;

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

        
        private void Window_Initialized(object sender, EventArgs e)
        {
            


   
            //menuSetting.Add(new SubItem("Employees"));
            //menuSetting.Add(new SubItem("Products"));

          
            var item1 = new ItemMenu("대시보드", PackIconMaterialKind.MonitorDashboard, new UserControl_Dashboard());

            var menuProgramSetting = new List<SubItem>();
            menuProgramSetting.Add(new SubItem("관리자 설정", new UserControl_AdminSetting()));
            menuProgramSetting.Add(new SubItem("컨트롤 설정", new UserControl_Settings("컨트롤 설정")));

            var item2 = new ItemMenu("프로그램 설정", menuProgramSetting, PackIconMaterialKind.Robot);

            var Statistics = new List<SubItem>();
            Statistics.Add(new SubItem("오류", new UserControl_Settings("오류 목록")));

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

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("hello");
        }
    }

   
}
