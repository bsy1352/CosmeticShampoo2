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
    /// UserControl_Dashboard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Dashboard : UserControl
    {
        List<Button> btnlist = new List<Button>();
        public UserControl_Dashboard()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in btnlist)
            {
                btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            }

            Button Selectedbtn = (Button)sender;
            Selectedbtn.Background = (Brush)new BrushConverter().ConvertFrom("#ffffff");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in FindVisualChildren<Button>(this))
            {
                btnlist.Add(btn);
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
