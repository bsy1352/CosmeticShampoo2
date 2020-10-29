using CosmeticShampoo.Server.DBmodel;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


// PC 서버
namespace CosmeticShampoo.Server.ViewModels
{
    public class HandleClient
    {
        TcpClient clientSocket = null;
        public Dictionary<TcpClient, string> clientList = null;
        

        public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
        {
            this.clientSocket = clientSocket;
            this.clientList = clientList;

            Thread t_hanlder = new Thread(doChat);
            t_hanlder.IsBackground = true;
            t_hanlder.Start();
        }

        public delegate void MessageDisplayHandler(string message, string user_name);
        public event MessageDisplayHandler OnReceived;

        public delegate void DisconnectedHandler(TcpClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        
        public delegate void SendJsonHandler(JArray jArray);
        public event SendJsonHandler OnSent;

        public delegate void SendLoginJsonHandler(bool isEnable, JArray jArray);
        public event SendLoginJsonHandler OnSentLoginInfo;

        private void doChat()
        {
            NetworkStream stream = null;
            try
            {
                byte[] buffer = new byte[1024];
                string msg = string.Empty;
                string header = string.Empty;
                int bytes = 0;
                int MessageCount = 0;

                while (true)
                {                   
                    MessageCount++;
                    stream = clientSocket.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    header = msg.Substring(0, msg.IndexOf("$"));

                    switch (header)
                    {
                        case "ShowOrders":
                            if (OnReceived != null)
                                OnReceived(msg, clientList[clientSocket].ToString());
                            Thread newThread = new Thread(ShowOrders);
                            newThread.Start();
                            break;

                        case "leaveChat":
                            OnReceived(msg, clientList[clientSocket].ToString());
                            OnDisconnected(clientSocket);
                            break;

                        case "Login":
                            OnReceived("로그인 시도", clientList[clientSocket].ToString());
                            string message = msg.Remove(0, header.Length + 1);
                            Thread LoginThread = new Thread(() => {

                                Login(message);
                            });
                            LoginThread.Start();
                            break;

                        default:
                            break;
                    }
                    
                    

                    
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message));

                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("doChat - Exception : {0}", ex.Message));

                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
        }

        private void Login(string param)
        {
            JArray jArray = new JArray();
            string[] result = param.Split('_');
            string id = result[0];
            string password = result[1];

            var db = new grancoEntities();

            

            users user = db.users.FirstOrDefault<users>(u => u.UserID.Equals(id) && u.UserPW.Equals(password));

            if(user != null)
            {
                var jsonString = JObject.FromObject(user);
                jArray.Add(jsonString);
                OnSentLoginInfo(true, jArray);
                
            }
            else
            {
                OnSentLoginInfo(false, null);
            }
            

            
        }

        private void ShowOrders()
        {
            JArray jArray = new JArray();
            Orders orders = new Orders();
            string strconn = "Server=127.0.0.1;Database=granco;Uid=admin;Pwd=1qhdtldbs!;";
            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd;
            MySqlDataReader reader;

            conn.Open();
            string select = "Select * from orders";
            cmd = new MySqlCommand(select, conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.OrderNum = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                orders.OrderDate = reader.IsDBNull(2) ? "null" : reader.GetString(2);
                orders.OrderDetail = reader.IsDBNull(1) ? "null" : reader.GetString(1);
                orders.OrderState = reader.IsDBNull(3) ? "null" : reader.GetString(3);

                var jsonString = JObject.FromObject(orders);
                jArray.Add(jsonString);
            }


            reader.Close();

            conn.Close();

            OnSent(jArray);
        }



    }

}
