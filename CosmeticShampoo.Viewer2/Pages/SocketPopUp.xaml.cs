using CosmeticShampoo.Viewer2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace CosmeticShampoo.Viewer2.Pages
{
    /// <summary>
    /// SocketPopUp.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SocketPopUp : Window
    {
        UserControl_Settings Settings;
        public SocketPopUp(UserControl_Settings settings)
        {
            Settings = settings;
            InitializeComponent();
            ipAddress.PreviewTextInput += textBox_PreviewTextInput;
            port.PreviewTextInput += textBox_PreviewTextInput;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string xmlFileName = "Settings.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Environment.CurrentDirectory + "\\xml\\" + xmlFileName);
            XmlNode ipAddressNode = xmlDoc.SelectSingleNode("/descendant::Settings/IpAddress");
            XmlNode portNode = xmlDoc.SelectSingleNode("/descendant::Settings/Port");

            ipAddress.Text = ipAddressNode.InnerText;
            port.Text = portNode.InnerText;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IpAddress.Get().ipAddress = ipAddress.Text;
            IpAddress.Get().port = port.Text;

            //DataTable table = new DataTable("IPAddress");
            //table.Columns.Add(IpAddress.Get().ipAddress);

            //DataSet ds = new DataSet("IpSetting");
            //ds.Tables.Add(table);

            Thread thread = new Thread(Make_XMLFile);
            thread.Start();
            
        }

        

        private void Make_XMLFile()
        {
            try
            {
                string xmlFileName = "Settings.xml";
                FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + "\\xml\\" + xmlFileName);

                if (fileInfo.Exists) //xml파일 수정
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(Environment.CurrentDirectory + "\\xml\\" + xmlFileName);
                    XmlNode ipAddressNode = xmlDoc.SelectSingleNode("/descendant::Settings/IpAddress");
                    XmlNode portNode = xmlDoc.SelectSingleNode("/descendant::Settings/Port");
                    ipAddressNode.InnerText = IpAddress.Get().ipAddress.ToString();
                    portNode.InnerText = IpAddress.Get().port.ToString();
                    xmlDoc.Save(Environment.CurrentDirectory + "\\xml\\" + xmlFileName);
                }
                else //xml파일 생성
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    XmlNode root = xmlDoc.CreateElement("Settings");
                    xmlDoc.AppendChild(root);

                    XmlNode ipAddressNode = xmlDoc.CreateElement("IpAddress");
                    XmlNode portNode = xmlDoc.CreateElement("Port");

                    ipAddressNode.InnerText = IpAddress.Get().ipAddress.ToString();
                    portNode.InnerText = IpAddress.Get().port.ToString();

                    root.AppendChild(ipAddressNode);
                    root.AppendChild(portNode);

                    xmlDoc.Save(Environment.CurrentDirectory + "\\xml\\" + xmlFileName);
                }

                MessageBox.Show("설정되었습니다.");

                this.Dispatcher.BeginInvoke(new Action(delegate
                {
                    Settings._Parent.Opacity = 1;
                    this.Close();

                }));

                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("오류: " + ex);
            }
            
            

            //DirectoryInfo DirInfoi = new DirectoryInfo(Environment.CurrentDirectory + "\\test");



            //while (DirInfoi.Exists == false)

            //{

                //    retryCnt--;

                //    DirInfoi.Create();

                //    DirInfoi = new DirectoryInfo(Environment.CurrentDirectory + "\\test");

                //}

                //if (retryCnt != 0)
                //{
                //    retryCnt = 5;

                //    DirInfoi = new DirectoryInfo(Environment.CurrentDirectory + "\\test");

                //    while (DirInfoi.Exists == false && retryCnt > 0)
                //    {

                //        DirInfoi.Create();
                //        DirInfoi = new DirectoryInfo(Environment.CurrentDirectory + "\\test");

                //    }




                //}
                //else
                //{

                //}




        }

        private XmlDocument Make_XmlDoc(DataTable data)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement inventory = xmlDoc.CreateElement("Settings");
            XmlElement IpAddress_INFO = IPADRESS_INFO(xmlDoc, data);
            inventory.AppendChild(IpAddress_INFO);
            xmlDoc.AppendChild(inventory);
            return xmlDoc;
        }

        private XmlElement IPADRESS_INFO(XmlDocument doc, DataTable data)
        {
            XmlElement ip_INFO = doc.CreateElement("IpAddress");

            List<PropertyInfo> P_List = new List<PropertyInfo>();
            IpAddress address = IpAddress.Get();

            P_List.AddRange(address.GetType().GetProperties());


            




            for (int i = 0; i < P_List.Count(); i++)
            {

                XmlElement tmp_Elm = doc.CreateElement(P_List[i].Name);

                string[] columnNames = data.Columns.Cast<DataColumn>()

                                 .Select(x => x.ColumnName)

                                 .ToArray();

                tmp_Elm.InnerText = columnNames[i].ToString();

                ip_INFO.AppendChild(tmp_Elm);
            }



            return ip_INFO;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings._Parent.Opacity = 1;
            this.Close();
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                MessageBox.Show("숫자만 가능합니다.");
                e.Handled = true;
            }
            
        }

        private bool IsNumeric(string source)
        {

            Regex regex = new Regex("[^0-9.-]+");

            return !regex.IsMatch(source);

        }

        
    }
}
