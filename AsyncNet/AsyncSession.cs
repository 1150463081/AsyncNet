using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace AsyncNet
{
    //网络会话
    public class AsyncSession
    {
        private Socket socket;
        public AsyncSessionState SessionState { get; protected set; } = AsyncSessionState.None;
        public void InitSession(Socket socket)
        {
            bool result = false;
            try
            {
                this.socket = socket;
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
                else
                {
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
                else
                {

                }
            }
            catch (Exception e)
            {
                Utility.LogWarn("RcvHeadWarn:{0}", e.Message);
                CloseSession();
            }
        }

        private void OnConnected(bool result)
        {
            Utility.Log("Client Connect:{0}", result);
        }
        public void CloseSession()
        {

        }
    }
    public enum AsyncSessionState
    {
        None,
        Connected,
        DisConnected
    }
}
