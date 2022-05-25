using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace AsyncNet
{
    //网络会话
    public abstract class AsyncSession<T>
        where T:AsyncMsg,new ()
    {
        private Socket socket;
        private Action closeCB;
        public AsyncSessionState SessionState { get; protected set; } = AsyncSessionState.None;
        public void InitSession(Socket socket,Action closeCB)
        {
            bool result = false;
            try
            {
                this.socket = socket;
                this.closeCB = closeCB;
                AsyncPkg pkg = new AsyncPkg();
                socket.BeginReceive(
                    pkg.headBuff,
                    0,
                    AsyncPkg.HeadLen,
                    SocketFlags.None,
                    new AsyncCallback(RcvHeadData),
                    pkg);
                result = true;
                SessionState = AsyncSessionState.Connected;
            }
            catch (Exception e)
            {
                Utility.LogError(e.Message);
            }
            finally
            {
                OnConnected(result);
            }
        }
        private void RcvHeadData(IAsyncResult ar)
        {
            try
            {
                if (socket == null || socket.Connected == false)
                {
                    Utility.LogWarn("Socket is null or not connected");
                    return;
                }
                int len = socket.EndReceive(ar);
                AsyncPkg pkg = ar.AsyncState as AsyncPkg;
                if (len == 0)//下线了
                {
                    Utility.LogWarn("远程连接正常下线");
                    CloseSession();
                    return;
                }
                pkg.headIndex += len;
                if (pkg.headIndex < AsyncPkg.HeadLen)//数据未接收完全
                {
                    socket.BeginReceive(
                        pkg.headBuff,
                        pkg.headIndex,
                        AsyncPkg.HeadLen - pkg.headIndex,
                        SocketFlags.None,
                        new AsyncCallback(RcvHeadData),
                        pkg
                        );
                }
                else
                {
                    pkg.InitBodyBuff();
                    socket.BeginReceive(
                        pkg.bodyBuff,
                        0,
                        pkg.bodyLen,
                        SocketFlags.None,
                        new AsyncCallback(RcvBodyData),
                        pkg
                        );
                }
            }
            catch (Exception e)
            {
                Utility.LogWarn("RcvHeadWarn:{0}", e.Message);
                CloseSession();
            }
        }
        private void RcvBodyData(IAsyncResult ar)
        {
            try
            {
                if (socket == null || socket.Connected == false)
                {
                    Utility.LogWarn("Socket is null or not connected");
                    return;
                }
                int len = socket.EndReceive(ar);
                AsyncPkg pkg = ar.AsyncState as AsyncPkg;
                if (len == 0)//下线了
                {
                    Utility.LogWarn("远程连接正常下线");
                    CloseSession();
                    return;
                }
                pkg.bodyIndex += len;
                if (pkg.bodyIndex < pkg.bodyLen)
                {
                    socket.BeginReceive(
                        pkg.bodyBuff,
                        pkg.bodyIndex,
                        pkg.bodyLen - pkg.bodyIndex,
                        SocketFlags.None,
                        new AsyncCallback(RcvBodyData),
                        pkg
                        );
                }
                else
                {
                    //todo数据反序列化
                    T msg = Utility.DeSerialize<T>(pkg.bodyBuff);
                    OnReceiveMsg(msg);

                    pkg.Reset();
                    socket.BeginReceive(
                        pkg.headBuff,
                        0,
                        AsyncPkg.HeadLen,
                        SocketFlags.None,
                        new AsyncCallback(RcvHeadData),
                        pkg);
                }
            }
            catch (Exception e)
            {
                Utility.LogWarn("RcvBodyWarn:{0}", e.Message);
                CloseSession();
            }
        }

        public bool SendMsg(AsyncMsg msg)
        {
            byte[] data = Utility.PackLenInfo(Utility.Serialize(msg));
            return SendMsg(data);
        }
        public bool SendMsg(byte[] data)
        {
            bool result = false;
            if (SessionState != AsyncSessionState.Connected)
            {
                Utility.LogWarn("Connection is DisConnected,can not send msg");
            }
            else
            {
                NetworkStream ns;
                try
                {
                    ns = new NetworkStream(socket);
                    if (ns.CanWrite)
                    {
                        ns.BeginWrite(
                            data,
                            0,
                            data.Length,
                            new AsyncCallback(SendCB),
                            ns
                            );
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    Utility.LogError("SendMsgError:{0}", e.Message);
                }
            }
            return result;
        }

        private void SendCB(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            try
            {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
            }
            catch (Exception e)
            {
                Utility.LogError("SendMsgError:{0}", e.Message);
            }
        }

        protected abstract void OnConnected(bool result);
        protected abstract void OnReceiveMsg(T msg);
        protected abstract void OnDisConnected();

        public void CloseSession()
        {
            SessionState = AsyncSessionState.DisConnected;
            OnDisConnected();
            closeCB?.Invoke();
            try
            {
                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                }
            }
            catch (Exception e)
            {
                Utility.LogError("Socket shut down error:{0}", e.Message);
            }
        }
    }
    public enum AsyncSessionState
    {
        None,
        Connected,
        DisConnected
    }
}
