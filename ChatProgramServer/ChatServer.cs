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
using System.Globalization;
using ChatProgramInterface;
using CookComputing.XmlRpc;

namespace ChatProgramServer
{
    /// <summary>
    /// Chatserver
    /// </summary>
    public class ChatServer : MarshalByRefObject, IServer
    {
        #region Public Methods

        #region General

        /// <summary>
        /// close all
        /// </summary>
        /// <param name="message">message to send with the shut down of the server</param>
        public static void CloseAll(string message)
        {
            foreach (var player in ChatManager.GetInstance().Speakers.Values)
            {
                player.ServerDown(message);
            }
        }

        /// <summary>
        /// connect
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="server">name of the server</param>
        /// <param name="port">port number</param>
        /// <returns>true if connected and false otherwise</returns>
        public bool Connect(string userName, string server, int port)
        {
            var instance = ChatManager.GetInstance();
            //usernama taken
            if (instance.Speakers.ContainsKey(userName))
            {
                throw new XmlRpcFaultException(1, "Username already taken! Choose another name.");
            }
            try
            {
                //create client proxy with its IP addres and port
                var proxy = XmlRpcProxyGen.Create<IClientProxy>();
                //public methods of client accesible at client.rem
                proxy.Url = string.Format("http://{0}:{1}/client.rem", server, port.ToString(CultureInfo.InvariantCulture));
                //add speaker in chat manager
                instance.Speakers.Add(userName, new Speaker(userName, proxy));
                return true;
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception(ex.FaultString);
            }
            catch (Exception)
            {
                throw new Exception("Server is not active. Contact Koen Plasmans!");
            }
        }

        /// <summary>
        /// disconnect
        /// </summary>
        /// <param name="userName">name of the user to disconnect</param>
        public void Disconnect(string userName)
        {
            var instance = ChatManager.GetInstance();
            //for each channel of the user, let everyone know that the user disconnected
            foreach (var channel in instance.Speakers[userName].Channels.Values)
            {
                channel.Speakers.Remove(userName);
                channel.Speak(string.Empty, userName + " has disconnected from the server!");
            }
            //remove user from chat manager
            instance.Speakers.Remove(userName);
        }

        /// <summary>
        /// change username
        /// </summary>
        /// <param name="userName">old name</param>
        /// <param name="newUserName">new name of the user</param>
        /// <returns>true if changed, false otherwise</returns>
        public bool ChangeUserName(string userName, string newUserName)
        {
            try
            {
                var instance = ChatManager.GetInstance();
                var speaker = instance.Speakers[userName];
                speaker.UserName = newUserName;
                //change name in chatmanager
                instance.Speakers.Remove(userName);
                instance.Speakers.Add(newUserName, speaker);
                //for each channel the user is in
                foreach (var channel in speaker.Channels.Values)
                {
                    //change name in channel
                    channel.Speakers.Remove(userName);
                    //let all other users know that the user's name changed
                    channel.Speak(string.Empty, userName + " is now known as " + newUserName);
                    channel.Speakers.Add(newUserName,speaker);
                }
                return true;
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception(ex.FaultString);
            }
            catch (Exception)
            {
                throw new Exception("Server is not active. Contact Koen Plasmans!");
            }
        }

        /// <summary>
        /// ask username
        /// </summary>
        /// <param name="userName">name of the user to check for existence</param>
        /// <returns>true if not contains name, false otherwise</returns>
        public bool IsUserNameOk(string userName)
        {
            return !ChatManager.GetInstance().Speakers.ContainsKey(userName);
        }

        #endregion

        #region Channel

        /// <summary>
        /// a user speaks in a channel
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text to speak</param>
        public void SpeakChannel(string channelName, string userName, string text)
        {
            var instance = ChatManager.GetInstance();
            //test if channel exists
            if (instance.Channels.ContainsKey(channelName))
            {
                var channel = instance.Channels[channelName];
                //only speakers in channel can speak
                if (channel.Speakers.ContainsKey(userName))
                {
                    channel.Speak(userName, text);
                }
            }
        }

        /// <summary>
        /// user connects to a channel
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="channelName">name of the channel</param>
        public void ConnectChannel(string userName, string channelName)
        {
            var instance = ChatManager.GetInstance();

            //if channel does not exists yet, create it
            if (!instance.Channels.ContainsKey(channelName))
            {
                instance.Channels.Add(channelName, new Channel(channelName));
            }
            //add channel to speaker and speaker to channel
            instance.Speakers[userName].AddChannel(instance.Channels[channelName]);
            var channel = instance.Channels[channelName];
            channel.Speakers.Add(userName, instance.Speakers[userName]);
            //let all users know that a new user entered the channel
            channel.SpeakExcept(string.Empty, userName + " has joined the channel", userName);
        }

        /// <summary>
        /// user disconnects from channel
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="channelName">name of the channel</param>
        /// <returns>true if disconnected, false otherwise</returns>
        public bool DisconnectChannel(string userName, string channelName)
        {
            var instance = ChatManager.GetInstance();
            //only if channels exists
            if (instance.Channels.ContainsKey(channelName))
            {
                //remove channel from speaker and speaker from channel
                instance.Speakers[userName].Channels.Remove(channelName);
                var channel = instance.Channels[channelName];
                channel.Speakers.Remove(userName);
                //notify all other users in the channel
                channel.Speak(string.Empty, userName + " has left the channel");
            }
            return true;
        }

        #endregion

        #endregion
    }
}