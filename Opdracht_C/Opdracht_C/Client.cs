using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Opdracht_C
{
    class Client
    {

        private Socket sender;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;
        private readonly byte[] bytes = new byte[1024];

      
        public bool CheckValidIpAddress(string ip)
        {
            if (ip != "")
            {
                //Check user input against regex (check if IP address is not empty).
                Regex regex = new Regex("\\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}\\b");
                Match match = regex.Match(ip);
                return match.Success;
            }
            else return false;
        }
        public void SetConnection(string ip, int port)
        {
            ipAddress = IPAddress.Parse(ip);
            remoteEP = new IPEndPoint(ipAddress, port);

        }
        public void OpenConnection()
        {
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);
        }
        public void CloseConnection()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        public void SendData(string data)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            sender.Send(msg);
        }
        public string ReciveData(string dataToSend)
        {
            SendData(dataToSend);
            int dataRecived = sender.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, dataRecived);
        }
    }
}
