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

namespace CosmeticShampoo.Viewer2.Pages
{
    /// <summary>
    /// AddUserView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddUserView : Window
    {
        UserControl_AdminSetting parent;
        public AddUserView(UserControl_AdminSetting Parent)
        {
            parent = Parent;

            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = parent._Parent;
            main.Opacity = 1;
            this.Close();

        }
    }
}
