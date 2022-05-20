using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncNet
{
    public class AsyncNetClient
    {
        private AsyncSession session;
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
        private void ServerConnectCB(IAsyncResult ar)
        {
            session = new AsyncSession();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    session.InitSession(socket);
                }
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }
    }
}
