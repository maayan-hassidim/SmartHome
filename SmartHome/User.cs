using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class User
    {
        public string Username { get; set; }
        public TcpClient Client { get; set; }
        public bool Loggedin { get; set; }
        public Dictionary<string, ADevice> UserDevices { get;}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public User(TcpClient client)
        {
            this.Client = client;
            UserDevices = new Dictionary<string, ADevice>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="loggedin"></param>
        public User(TcpClient client, string username, bool loggedin)
        {
            this.Client = client;
            this.Username = username;
            this.Loggedin = loggedin;
            this.UserDevices = new Dictionary<string, ADevice>();
        }

    }
}
