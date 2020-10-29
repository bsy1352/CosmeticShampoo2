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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CosmeticShampoo.Viewer2.Utility_Views
{
    /// <summary>
    /// UserMenuDropDown.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserMenuDropDown : UserControl
    {
        MainWindow _parent;
        DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        DoubleAnimation myDoubleAnimationArrowUp = new DoubleAnimation(0, 180, new Duration(TimeSpan.FromSeconds(0.3)));

        DoubleAnimation myDoubleAnimationArrowDown = new DoubleAnimation(180, 0, new Duration(TimeSpan.FromSeconds(0.3)));
        RotateTransform rotation = new RotateTransform();
        private bool isCollapsed = true;

        Image Arrow;

        public UserMenu userMenu { get; set; }
        public UserMenuDropDown userMenuDrop { get; set; }
        private ListView listview { get; set; }

        public UserMenuDropDown(ItemMenu itemMenu=null, MainWindow parent=null, users user=null)
        {
            this.DataContext = itemMenu;
            _parent = parent;
            
            InitializeComponent();
        }


        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hello");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isCollapsed)
            {
                dropdownPanel.Visibility = Visibility.Visible;
                dropdownPanel.BeginAnimation(ListView.HeightProperty, myDoubleAnimation);
                rotation.BeginAnimation(RotateTransform.AngleProperty, myDoubleAnimationArrowUp);
                isCollapsed = false;
            }
            else
            {
                dropdownPanel.BeginAnimation(ListView.HeightProperty, myDoubleAnimation);
                rotation.BeginAnimation(RotateTransform.AngleProperty, myDoubleAnimationArrowDown);
                dropdownPanel.Visibility = Visibility.Collapsed;
                isCollapsed = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Arrow = btn.Template.FindName("Arrow", btn) as Image;
            
            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = getListviewLenght();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));



            Arrow.RenderTransform = rotation;
            Arrow.RenderTransformOrigin = new Point(0.5, 0.5);

            userMenu.ButtonClickedEvent += turnOff_Menu;
            userMenuDrop.ButtonClickedEvent += turnOff_Menu;
        }

        private void turnOff_Menu(object sender, EventArgs e)
        {
            if(listview != null && listview.SelectedItem != null)
            {
                listview.UnselectAll();
            }
        }

       

        private void dropdownPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listview = (ListView)sender;
            if(listview.SelectedItem != null)
            {
                _parent.SwitchScreens(((SubItem)(listview.SelectedItem)).Screen);
                OnCloseButtonClicked(e);
            }
            
        }

        private int getListviewLenght()
        {

            int length = dropdownPanel.Items.Count * (48);

            return length;
        }

        public event EventHandler ButtonClickedEvent;
        protected virtual void OnCloseButtonClicked(EventArgs e)
        {
            var handler = ButtonClickedEvent;
            if (handler != null)
                handler(this, e);
        }
    }

    
}
