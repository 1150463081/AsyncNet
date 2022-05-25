using AsyncNetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncNet
{
    class AsyncClientStart
    {
        static void Main(string[] args)
        {
            AsyncNet<ClientSession,NetMsg> client = new AsyncNet<ClientSession,NetMsg>();
            client.StartAsClient("127.0.0.1", 1997);
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "quit")
                {
                    client.CloseClient();
                    break;
                }
                else
                {
                    client.Session.SendMsg(new NetMsg() { Str = input });
                }
            }
        }
    }
}
