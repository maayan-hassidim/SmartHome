using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class LoginCommand : ACommand
    {
        private string username = null;

        public LoginCommand(string[] command, string clientStr, Server server) : base(clientStr, server)
        {
            this.commandName = "Login";
            if (!(command!=null && commandLength(command) == 2 && usernameIsValid(command[1])))
                msg = "Unknown command";
        }

        /// <summary>
        /// Check if the username is valid and not beliog to another user 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool usernameIsValid(string username)
        {
            StringBuilder tempUname = new StringBuilder();
            if (username == null || username.Length == 0)
            {
                msg = "user name is missing.";
                return false;
            }
            int i = 0;
            //remove white spaces
            while (i < username.Length && char.IsWhiteSpace(username[i]))
                i++;
            while (i< username.Length && !char.IsWhiteSpace(username[i]))
            {
                tempUname.Append(username[i]);
                i++;
            }
            //remove white spaces
            while (i < username.Length && char.IsWhiteSpace(username[i]))
                i++;
            if (i != username.Length)
            {
                msg = "user name must be one word and not only white spaces.";
                return false;
            }
            this.username = tempUname.ToString();
            bool ans = this.server.UsernameExist(username);
            if (ans)
                msg = "This user name is already exists.";
            return !ans;
        }

        /// <summary>
        /// Method which executes the command
        /// </summary>
        public override string DoCommand()
        {
            if (msg != null)
                return msg;
            server.LoginUser(this.clientStr, username);
            return "you are successfully logged in";
        }

    }
}
