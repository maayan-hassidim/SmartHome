using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class SetStateCommand : ACommand
    {
        private string deviceState = null;
        private string deviceId = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="clientStr"></param>
        /// <param name="server"></param>
        public SetStateCommand(string[] command, string clientStr, Server server) : base(clientStr, server)
        {
            if (!server.IsLogedin(clientStr))
                msg = "Please login first";
            else if (!(command != null && commandLength(command) == 3 && deviceIdValid(command[1], clientStr) && stateIsValid(command[2])))
                msg = "Unknown command";
                
        }

        /// <summary>
        /// Checks is the state is valid
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool stateIsValid(string state)
        {
            state = state.ToLower();
            bool ans = (state == "on" || state == "off");
            if (!ans)
                msg = "State is not valid";
            this.deviceState = state;
            return ans;
        }

        /// <summary>
        /// Checks is the device id is valid
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool deviceIdValid(string deviceId, string clientStr)
        {
            bool ans = this.server.DeviceExsist(deviceId, clientStr);
            if (!ans)
                msg = "Device doen not exist";
            this.deviceId = deviceId;
            return ans;
        }

        /// <summary>
        /// Method which executes the command
        /// </summary>
        public override string DoCommand()
        {
            if (msg != null)
                return msg;
            this.server.SetDeviceState(this.deviceId, this.deviceState, this.clientStr);
            return String.Format("Device {0} is {1}", this.deviceId, this.deviceState);
        }

    }
}
