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
using System.Windows.Threading;

namespace CosmeticShampoo.Viewer2.Utility_Views
{
    /// <summary>
    /// Opening.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Opening : Window
    {
        DispatcherTimer timer;
        public Opening()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(15);    //시간간격 설정
            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가
            timer.Start();

            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            progressBar.Width += 10;
            if (progressBar.Width >= 800)
            {
                timer.Stop();
                Login login = new Login();
                login.Show();
                this.Hide();
            }

        }
    }
}
