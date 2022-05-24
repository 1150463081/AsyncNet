using System;
using AsyncNet;

namespace AsyncNetProtocol
{
    [Serializable]
    public class NetMsg:AsyncMsg
    {
        public string Str;
    }
}
