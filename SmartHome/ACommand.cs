using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    /// <summary>
    /// An abstract class which represents a command
    /// </summary>
    abstract class ACommand : ICommand
    {
        protected string commandName;
        protected string clientStr;
        protected Server server;
        protected string msg;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="command"></param>
        public ACommand(string clientStr, Server server)
        {
            this.clientStr = clientStr;
            this.server = server;
            this.msg = null;
        }

        /// <summary>
        /// Method which executes the command
        /// </summary>
        /// <returns>command status</returns>
        public abstract string DoCommand();

        /// <summary>
        /// Method which return the command name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.commandName;
        }

        /// <summary>
        /// Returns the number of words in thr command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int commandLength(string[] command)
        {
            int count = 0;
            foreach (string s in command) { 
                if (s != "\t" && s != "\n")
                    count++;
                else break;
            }
            return count;
        }
    }
}
