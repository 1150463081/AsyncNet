using AsyncProtocol;
using System;

namespace AsyncNet
{
    class AsyncServerStart
    {
        static void Main(string[] args)
        {
            AsyncNetServer server = new AsyncNetServer();
            server.StartServer("127.0.0.1", 1997);
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
                    AsyncMsg msg = new AsyncMsg() { Str = input };
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
