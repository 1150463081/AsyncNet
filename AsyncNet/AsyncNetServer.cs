using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncNet
{
    public class AsyncNetServer
    {
        private Socket socket;
        private List<AsyncSession> sessionList = new List<AsyncSession>();
        public int backlog = 10;
        public void StartServer(string ip, int port)
        {
            //网络部分是IO密集型操作，使用try catch
            try
            {
                //ip类型ipV4，流式传输
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //绑定ip
                socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                //开启监听，backlog表示有多少个可排队等待处理
                socket.Listen(backlog);
                Utility.ColorLog(LogColor.Green,"Server Start...");
                //等待客户端连过来，开启一个异步接受，链接成功执行回调
                socket.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }
        private void ClientConnectCB(IAsyncResult asyncResult)
        {
            AsyncSession session = new AsyncSession();
            try
            {
                //结束一次连接
                Socket clientSocket = socket.EndAccept(asyncResult);
                Utility.Log("New Client Connect...");
                if (clientSocket.Connected)
                {
                    lock (sessionList)
                    {
                        sessionList.Add(session);
                    }
                    session.InitSession(clientSocket);
                }
                //开始接收下一个新客户端的连接
                socket.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception e)
            {
                Utility.LogError("ClientConnectCB:{0}",e.Message);
            }
        }

    }
}
