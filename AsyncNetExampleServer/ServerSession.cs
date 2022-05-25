using System;
using System.Collections.Generic;
using System.Text;
using AsyncNetProtocol;

namespace AsyncNet
{
    public class ServerSession : AsyncSession<NetMsg>
    {
        protected override void OnConnected(bool result)
        {
            Utility.ColorLog(LogColor.Green, "New Client Connected:{0}", result);
        }

        protected override void OnDisConnected()
        {
            Utility.ColorLog(LogColor.Yellow, "Client DisConnected...");

        }

        protected override void OnReceiveMsg(NetMsg msg)
        {
            Utility.Log("RcvMsg:{0}", msg.Str);
        }
    }
}
