using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class SetValueCommand : ACommand
    {
        private string deviceId = null;
        private double value = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="clientStr"></param>
        /// <param name="server"></param>
        public SetValueCommand(string[] command, string clientStr, Server server) : base(clientStr, server)
        {
            if (!server.IsLogedin(clientStr))
                msg = "Please login first";
            else if (!(command != null && commandLength(command) == 3 && deviceIdValid(command[1], clientStr) && valueIsValid(command[2])))
                msg = "Unknown command";  
        }

        /// <summary>
        /// Checks is the value is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool valueIsValid(string value)
        {
            bool ans = (Double.TryParse(value, out this.value) && this.value > 0);
            if (!ans)
                msg = "value is not valid";
            return ans;
        }

        /// <summary>
        /// Method which executes the command
        /// </summary>
        public override string DoCommand()
        {
            if (msg != null)
                return msg;
            string ans = this.server.SetDeviceValue(this.deviceId, this.value, this.clientStr);
            if (ans==null)
                return String.Format("Device {0} new value is {1}", this.deviceId, this.deviceId);
            return ans;
        }

        /// <summary>
        /// Checks is the device id is valid
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private bool deviceIdValid(string deviceId, string clientStr)
        {
            bool ans = this.server.DeviceExsist(deviceId, clientStr);
            if (!ans)
                msg = "Device doen not exist";
            this.deviceId = deviceId;
            return ans;
        }
    }
}
