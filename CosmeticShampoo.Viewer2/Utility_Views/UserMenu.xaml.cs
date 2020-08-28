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
        static bool isExpanded = false;
        MainWindow _parent;

        public UserMenu(ItemMenu itemMenu, MainWindow Parent)
        {

            InitializeComponent();

            _parent = Parent;


            this.DataContext = itemMenu;



        }

        private static ListView listView { get; set; }


        private static ListViewItem listItem { get; set; }

        //private void ExpanderMenu_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    this.ExpanderMenu.IsExpanded = false;
        //}

        //private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (isExpanded == false)
        //    {
        //        isExpanded = true;
        //        this.ExpanderMenu.IsExpanded = true;
        //    }
        //    else if (isExpanded == true)
        //    {
        //        isExpanded = false;
        //        this.ExpanderMenu.IsExpanded = false;
        //    }
        //}



        //private void ExpanderMenu_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ExpanderMenu.IsExpanded = true;
        //}

        //private void ExpanderMenu_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    ExpanderMenu.IsExpanded = false;
        //}



        //private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (listView != null)
        //    {
        //        listView.SelectedItem = null;
        //    }
        //    listView = (ListView)sender;
        //    try
        //    {
        //        try
        //        {
        //            _parent.SwitchScreens(((SubItem)(listView.SelectedItem)).Screen);

        //        }
        //        catch (InvalidCastException)
        //        {
        //            _parent.SwitchScreens(((ItemMenu)((ListViewItem)listView.SelectedItem).DataContext).Screen);
        //        }

        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return;
        //    }




        //}

        private void ListViewMenu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;

        }

    }

    public class personInListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = (string)value;
            List<SubItem> items = (parameter as ObjectDataProvider).ObjectInstance as List<SubItem>;

            return items.Exists(item => name.Equals(item.Name));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

