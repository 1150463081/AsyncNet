using System;

namespace AsyncNet
{
    class AsyncServerStart
    {
        static void Main(string[] args)
        {
            AsyncNetServer server = new AsyncNetServer();
            server.StartServer("127.0.0.1", 1997);
            Console.ReadKey();
        }
    }
}
