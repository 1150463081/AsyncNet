using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace AsyncNet
{
    public class AsyncNet<T,K>
        where T:AsyncSession<K>,new()
        where K:AsyncMsg,new()
    {
        public T Session { get; private set; }
        private Socket socket;
        public List<T> sessionList { get; private set; } = new List<T>();
        public int backlog = 10;
        #region Client
        public void StartAsClient(string ip, int port)
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
            Session = new T();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    Session.InitSession(socket, null);
                }
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }
        #endregion
        #region Server
        public void StartAsServer(string ip, int port)
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
                Utility.ColorLog(LogColor.Green, "Server Start...");
                //等待客户端连过来，开启一个异步接受，链接成功执行回调
                socket.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
        }
        public void CloseServer()
        {
            for (int i = 0; i < sessionList.Count; i++)
            {
                sessionList[i].CloseSession();
            }
            sessionList.Clear();
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }
        private void ClientConnectCB(IAsyncResult asyncResult)
        {
            T session = new T();
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
                    session.InitSession(clientSocket, () => {
                        if (sessionList.Contains(session))
                        {
                            lock (sessionList)
                            {
                                if (sessionList.Remove(session))
                                {
                                    Utility.Log("Clear ServerSession Success");
                                }
                                else
                                {
                                    Utility.LogError("Clear ServerSession Fail");
                                }
                            }
                        }
                    });
                }
                //开始接收下一个新客户端的连接
                socket.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception e)
            {
                Utility.LogError("ClientConnectCB:{0}", e.Message);
            }
        }
        #endregion

    }
}
