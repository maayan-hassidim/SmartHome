using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    /// <summary>
    /// An abstract class which represents a dives
    /// Devices of this type have a value field
    /// </summary>
    abstract class ValueDevice : ADevice
    {
        public double DeviceValue { get; private set; }

        /// <summary>
        /// Sets the device value
        /// Returns true if the action was performed and false otherwise  
        /// Devices of this kind has value so that this method always returns true
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public new bool SetValue(double newValue)
        {
            DeviceValue = newValue;
            return true;
        }
    }
}
