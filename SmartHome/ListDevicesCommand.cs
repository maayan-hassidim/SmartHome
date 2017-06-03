using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class ListDevicesCommand : ACommand
    {
        /// <summary>
        /// Costracture
        /// </summary>
        /// <param name="command"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        public ListDevicesCommand(string[] command, string client, Server server) : base(client, server)
        {
            this.commandName = "ListDevices";
            if (!server.IsLogedin(clientStr))
                msg = "Please login first";
            else if (command==null || commandLength(command) != 0)
                msg = "The given parametersare not required.";
        }


        /// <summary>
        /// Method which executes the command
        /// </summary>
        public override string DoCommand()
        {
            if (msg == null){
                Dictionary<string, ADevice> devices = this.server.GetClientDevices(this.clientStr);
                StringBuilder devicesList = new StringBuilder();
                if (devicesList == null || devicesList.Length == 0)
                    return "This Client Doen not have devices.";
                int i = 1;
                foreach (string d in devices.Keys)
                {
                    devicesList.Append(String.Format("{0}. {1}: id={2}, state={3}\n", i, devices[d].DeviceName, d, devices[d].DeviceState));
                }
                return devicesList.ToString();
            }
            return msg;
        }

    }
}
