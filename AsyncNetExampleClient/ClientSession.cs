using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncNetProtocol;

namespace AsyncNet
{
    public class ClientSession : AsyncSession<NetMsg>
    {
        protected override void OnConnected(bool result)
        {
            Utility.ColorLog(LogColor.Green, "Client Connected:{0}", result);
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
