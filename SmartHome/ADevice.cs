using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    /// <summary>
    /// An abstract class which represents a simple dives
    /// Devices of this type do not have a value field
    /// </summary>
    abstract class ADevice
    {
        public string DeviceName { get; }
        public string DeviceId { get; }
        public string DeviceState { get; set; }

        /// <summary>
        /// Sets the device value
        /// Returns true if the action was performed and false otherwise  
        /// Devices of this kind dose not have value so that this method always returns false
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public bool SetValue(double newValue)
        {
            return false;
        }


    }
}
