using AsyncNetProtocol;
using System;

namespace AsyncNet
{
    class AsyncServerStart
    {
        static void Main(string[] args)
        {
            AsyncNet server = new AsyncNet();
            server.StartAsServer("127.0.0.1", 1997);
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "quit")
                {
                    server.CloseServer();
                    break;
                }
                else
                {
                    AsyncMsg msg = new NetMsg() { Str = input };
                    byte[] data = Utility.PackLenInfo(Utility.Serialize(msg));
                    for (int i = 0; i < server.sessionList.Count; i++)
                    {
                        server.sessionList[i].SendMsg(data);
                    }
                }
            }
        }
    }
}
