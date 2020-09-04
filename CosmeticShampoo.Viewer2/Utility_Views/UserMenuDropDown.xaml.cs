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
        public UserMenuDropDown(ItemMenu itemMenu, MainWindow parent)
        {
            this.DataContext = itemMenu;
            _parent = parent;


            InitializeComponent();

            

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
            Console.WriteLine(dropdownPanel.ActualHeight);
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            
            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = 200;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.8));

            
            Arrow.RenderTransform = rotation;
            Arrow.RenderTransformOrigin = new Point(0.5, 0.5);
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false; // not needed
        }

        #endregion
    }
}
