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

namespace CosmeticShampoo.Viewer2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        UserMenu Dashboard;
        UserMenuDropDown ProgramSetting;

        public MainWindow()
        {
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

          
            var item1 = new ItemMenu("대시보드", PackIconMaterialKind.MonitorDashboard);

            var menuProgramSetting = new List<SubItem>();
            menuProgramSetting.Add(new SubItem("Customer"));
            menuProgramSetting.Add(new SubItem("Provider"));
            menuProgramSetting.Add(new SubItem("Employees"));
            menuProgramSetting.Add(new SubItem("Products"));

            var item2 = new ItemMenu("프로그램 설정", menuProgramSetting, PackIconMaterialKind.Robot);

            Dashboard = new UserMenu(item1, this);
            ProgramSetting = new UserMenuDropDown(item2, this);
            


            Menu.Children.Add(Dashboard);
            Menu.Children.Add(ProgramSetting);
            DataContext = this;

        }

        private void PopUpMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            PopUpMenu.Visibility = Visibility.Visible;
        }
    }

   
}
