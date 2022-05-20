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
                AsyncPkg pkg=new AsyncPkg();
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
        protected void RcvHeadData(IAsyncResult ar)
        {
            try
            {

            }catch(Exception e)
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
