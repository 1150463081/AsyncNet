using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncNet
{
    public class AsyncNetClient
    {
        public AsyncSession Session { get; private set; }
        private Socket socket;
        public void StartClient(string ip, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Utility.ColorLog(LogColor.Green, "Client Start...");
                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.BeginConnect(endPoint, new AsyncCallback(ServerConnectCB), null);
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }

        public void CloseClient()
        {
            if (Session != null)
            {
                Session.CloseSession();
                Session = null;
            }
            if (socket != null)
            {
                socket = null;
            }
        }
        private void ServerConnectCB(IAsyncResult ar)
        {
            Session = new AsyncSession();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    Session.InitSession(socket,null);
                }
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }
    }
}
