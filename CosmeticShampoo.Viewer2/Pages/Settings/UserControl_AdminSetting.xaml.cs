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
    /// UserControl_AdminSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_AdminSetting : UserControl
    {
        public MainWindow _Parent { get; set; }
        public UserControl_AdminSetting(MainWindow parent)
        {
            _Parent = parent;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUserView addUser = new AddUserView(this);

            _Parent.Opacity = 0.6;
            addUser.ShowDialog();

        }
    }
}
