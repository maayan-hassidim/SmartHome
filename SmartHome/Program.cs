using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SmartHome
{
    class Program
    {

        public static void Main()
        {
            Server server = new Server();
            server.StartListening();            
        }
    }
}
