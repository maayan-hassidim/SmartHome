using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace SmartHome
{
    /// <summary>
    /// This class represent the server
    /// </summary>
    public class Server
    {
        /// <summary>
        /// A data structure for the connectes users 
        /// The key is a string- the client ip and theclient port combine
        /// The value is a User
        /// </summary>
        private Dictionary<string, User> users;
        /// <summary>
        /// A data structure for the usernames
        /// Use for check if a username is exist- O(1)
        /// 
        /// 
        /// : username is unique
        /// </summary>
        HashSet<string> usernames;

        /// <summary>
        /// A costructor
        /// Initialize the tata structurs
        /// </summary>
        public Server()
        {
            users = new Dictionary<string, User>();
            usernames = new HashSet<string>();
        }

        /// <summary>
        /// This method starts the server
        /// </summary>
        public void StartListening()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine(String.Format("Connected to client ip: {0} , port: {1}" , ((IPEndPoint)client.Client.RemoteEndPoint).Address, ((IPEndPoint)client.Client.RemoteEndPoint).Port));
                    string key = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    User user = new User(client);
                    users.Add(key, user);

                    // Get a stream object for reading and writing
                    NetworkStream clientStream = client.GetStream();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes("You are connected!");
                    clientStream.Write(msg, 0, msg.Length);

                    //t The main loop that reada the user msgs
                    ReadMsgs(client, clientStream);
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        /// <summary>
        /// A This method checks if a given username already belond to one of the connected users
        /// </summary>
        /// <param name="clientStr"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        internal bool CheckUsermane(string clientStr, string username)
        {
            if (UserExist(clientStr) && users[clientStr].Username == username)
                return true;
            return false;
        }

        /// <summary>
        /// This method login the user
        /// </summary>
        /// <param name="clientStr"></param>
        /// <param name="username"></param>
        internal void LoginUser(string clientStr, string username)
        {
            if (UserExist(clientStr))
            {
                users[clientStr].Loggedin = true;
                users[clientStr].Username = username;
            }

        }

        /// <summary>
        /// this methot set the value of the devive (if possible)
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="value"></param>
        /// <param name="clientStr"></param>
        /// <returns>returns a msg. if the device hasn't a value property, returns null</returns>
        internal string SetDeviceValue(string deviceId, double value, string clientStr)
        {
            ADevice device = GetClientDeviceById(deviceId, clientStr);
            bool valueChanged = device.SetValue(value);
            if (!valueChanged)
                return "This device dose not support this command";
            else
                return null;
        }

        /// <summary>
        /// rThis method returns a device by its id
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        private ADevice GetClientDeviceById(string deviceId, string clientStr)
        {
            if (DeviceExsist(deviceId, clientStr))
                return users[clientStr].UserDevices[deviceId];
            else
                return null;
        }

        /// <summary>
        /// This method changes the given device state
        /// It is possible to change the state to the previous one (meaning the user can "change" state from 'on' to 'on')
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="deviceState"></param>
        /// <param name="clientStr"></param>
        internal void SetDeviceState(string deviceId, string deviceState, string clientStr)
        {
            ADevice device = GetClientDeviceById(deviceId, clientStr);
            device.DeviceState = deviceState;
        }

        /// <summary>
        /// This method checks if a given device is exists
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        internal bool DeviceExsist(string deviceId, string clientStr)
        {
            return users.ContainsKey(clientStr) && users[clientStr].UserDevices.ContainsKey(deviceId);
        }

        /// <summary>
        /// This method returns all client devices
        /// </summary>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        internal Dictionary<string, ADevice> GetClientDevices(string clientStr)
        {
            if (UserExist(clientStr))
                return users[clientStr].UserDevices;
            return null;
        }

        /// <summary>
        /// Returns the client by its ip and port
        /// </summary>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        internal TcpClient GetClient(string clientStr)
        {
            if (UserExist(clientStr))
                return users[clientStr].Client;
            return null;
        }

        /// <summary>
        /// Checks if a user exists
        /// </summary>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        public bool UserExist(string clientStr)
        {
            return users.ContainsKey(clientStr);
        }

        /// <summary>
        /// Checks if a username exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UsernameExist(string username)
        {
            return usernames.Contains(username);
        }

        /// <summary>
        /// This method reads all client msgs
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientStream"></param>
        private void ReadMsgs(TcpClient client, NetworkStream clientStream)
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;
            // Get a stream object for reading and writing
            int i;
            // Loop to receive all the data sent by the client.
            while ((i = clientStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                if (String.IsNullOrWhiteSpace(data) || String.IsNullOrEmpty(data))
                    continue;

                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                data = data.ToLower();

                string clientStr = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                string msgStr = HandleCommand(data, clientStr);
                
               // Send back a response.
               if(msgStr != null) {
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(msgStr);
                    clientStream.Write(msg, 0, msg.Length);
                }
                    
            }
        }

        /// <summary>
        /// This method handels every given msg
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clientStr"></param>
        /// <returns>A respons for to client</returns>
        private string HandleCommand(string data, string clientStr)
        {
            string[] splitCommand = data.Split();
            ICommand command = null;
            switch (splitCommand[0])
            {
                case "login":
                    command = new LoginCommand(splitCommand, clientStr, this);
                    break;
                case "listdevices":
                    command = new ListDevicesCommand(splitCommand, clientStr, this);
                    break;
                case "setstate":
                    command = new SetStateCommand(splitCommand, clientStr, this);
                    break;
                case "setvalue":
                    command = new SetValueCommand(splitCommand, clientStr, this);
                    break;
                default:
                    break;
            }
            if (command != null)
            {
                string commandRes = command.DoCommand();
                return String.Format("command {0} is done. command msg: {1}", command.GetName(), commandRes);
            }
            else
                return "Unknown command";
        }


        /// <summary>
        /// Check if a user logged in
        /// </summary>
        /// <param name="clientStr"></param>
        /// <returns></returns>
        public bool IsLogedin(string clientStr)
        {
            return users.ContainsKey(clientStr) && users[clientStr].Loggedin;
        }
    }
}



