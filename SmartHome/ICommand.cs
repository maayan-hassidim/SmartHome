using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    /// <summary>
    /// Interface ICommand
    /// A contract for all servers commands
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// Method which executes the command
        /// </summary>
        string DoCommand();

        /// <summary>
        /// Method that returns the name of the command
        /// </summary>
        /// <returns>name of the command</returns>
        string GetName();

    }
}
