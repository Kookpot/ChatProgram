/*
    ChatProgram is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    ChatProgram is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with ChatProgram.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace ChatProgramClient
{
    /// <summary>
    /// chat client main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// main loop
        /// </summary>
        /// <param name="args">possible arguments</param>
        static void Main(string[] args)
        {
            //get chat client
            var instance = ChatClient.GetInstance();
            var userName = args[0];
            instance.Connect(userName, "127.0.0.1", int.Parse(args[1]));
            var quit = false;
            var channelName= string.Empty;
            while (!quit)
            {
                var line = Console.ReadLine();
                if (line == null) continue;
                if (line.StartsWith("/quit"))
                {
                    quit = true;
                    instance.Disconnect();
                }
                else if (line.StartsWith("/register"))
                {
                    if(!string.IsNullOrEmpty(channelName))
                    {
                        instance.DisconnectChannel(channelName);
                    }
                    channelName = line.Split(' ')[1];
                    instance.ConnectChannel(channelName);
                }
                else if (line.StartsWith("/changename"))
                {
                    var userNameToTry = line.Split(' ')[1];
                    if (instance.IsUserNameOk(userNameToTry) && instance.ChangeUserName(userNameToTry))
                    {
                        Console.WriteLine("Old username : " + userName + " and new username : " + userNameToTry); 
                        userName = userNameToTry;
                    }
                }
                else if (line.StartsWith("/unregister"))
                {
                    instance.DisconnectChannel(channelName);
                    channelName = string.Empty;
                }
                else if (!string.IsNullOrEmpty(channelName) && (!line.StartsWith("/")))
                {
                    instance.SpeakInChannel(channelName, line);
                }
            }
            Console.WriteLine("Press <ENTER> to shutdown");
            Console.ReadKey();
        }
    }
}