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
            AsyncNetClient client = new AsyncNetClient();
            client.StartClient("127.0.0.1", 1997);
            Console.ReadKey();
        }
    }
}
