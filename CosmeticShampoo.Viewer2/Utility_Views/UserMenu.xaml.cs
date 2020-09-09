using CosmeticShampoo.Viewer2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CosmeticShampoo.Viewer2.Utility_Views
{
    /// <summary>
    /// Menu.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserMenu : UserControl
    {
      
        MainWindow _parent;
        public UserMenuDropDown userMenuDrop { get; set; }
        public event EventHandler ButtonClickedEvent;
        public UserMenu(ItemMenu itemMenu=null, MainWindow Parent=null)
        {

            InitializeComponent();

            _parent = Parent;
            this.DataContext = itemMenu;

        }
        protected virtual void OnCloseButtonClicked(EventArgs e)
        {
            var handler = ButtonClickedEvent;
            if (handler != null)
                handler(this, e);
        }


        private void ListViewMenu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;

        }

        private void ShowViewButton_Clicked(object sender, RoutedEventArgs e)
        {
            OnCloseButtonClicked(e);
            _parent.SwitchScreens(((ItemMenu)DataContext).Screen);
        }

        
      
    }

    
}

