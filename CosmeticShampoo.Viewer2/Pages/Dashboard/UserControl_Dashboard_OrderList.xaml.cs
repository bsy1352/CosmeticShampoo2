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
    /// UserControl_Dashboard_OrderList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl_Dashboard_OrderList : UserControl
    {
        UserControl_Dashboard_Total total;
        public UserControl_Dashboard_OrderList(UserControl_Dashboard_Total Total)
        {
            total = Total;
            InitializeComponent();
        }
    }
}
