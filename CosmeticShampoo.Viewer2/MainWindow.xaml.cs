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
        UserMenu Create;
        UserMenu Message;
        UserMenu Ticket;

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

        private void Window_Initialized(object sender, EventArgs e)
        {
            


   
            //menuSetting.Add(new SubItem("Employees"));
            //menuSetting.Add(new SubItem("Products"));

            var menuControl = new List<SubItem>();
            menuControl.Add(new SubItem("Customer"));
            menuControl.Add(new SubItem("Provider"));
            menuControl.Add(new SubItem("Employees"));
            menuControl.Add(new SubItem("Products"));

            var item1 = new ItemMenu("Control", menuControl, PackIconMaterialKind.Account);

            var menuTickets = new List<SubItem>();
            menuTickets.Add(new SubItem("Customer"));
            menuTickets.Add(new SubItem("Provider"));
            menuTickets.Add(new SubItem("Employees"));
            menuTickets.Add(new SubItem("Products"));

            var item2 = new ItemMenu("Tickets", menuTickets, PackIconMaterialKind.Account);

            var menuMessage = new List<SubItem>();
            menuMessage.Add(new SubItem("Customer"));
            menuMessage.Add(new SubItem("Provider"));
            menuMessage.Add(new SubItem("Employees"));
            menuMessage.Add(new SubItem("Products"));

            var item3 = new ItemMenu("Message", menuMessage, PackIconMaterialKind.Account);


            Create = new UserMenu(item1, this);
            Ticket = new UserMenu(item2, this);
            Message = new UserMenu(item3, this);


            Menu.Children.Add(Create);
            Menu.Children.Add(Ticket);
            Menu.Children.Add(Message);
            DataContext = this;

        }
    }
}
