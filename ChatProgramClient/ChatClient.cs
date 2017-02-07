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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using ChatProgramInterface;
using CookComputing.XmlRpc;
using System.Collections;

namespace ChatProgramClient
{
    /// <summary>
    /// controller for chatting
    /// </summary>
    class ChatClient : MarshalByRefObject, IClient
    {
        #region Members

        //NETWORK
        public IServerProxy Server { get; set; }
        public bool Active;

        //username
        public string UserName { get; set; }

        private static ChatClient _instance;

        #endregion

        #region Public Methods - From IClient

        #region General

        /// <summary>
        /// server down
        /// </summary>
        /// <param name="message">message of serverdown event</param>
        public void ServerDown(string message)
        {
            Console.WriteLine("Server Down : " + message);
        }

        #endregion

        #region Channels

        /// <summary>
        /// we receive a message from a user in a channel
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text that was sent</param>
        public void ReceiveMessage(string channelName, string userName, string text)
        {
            if(userName.Equals(string.Empty))
            {
                Console.WriteLine(text);
            } 
            else
            {
                Console.WriteLine(userName + " says in channel " + channelName + " : " + text);
            }
        }

        /// <summary>
        /// we are connected to a channel
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        public void ConnectedToChannel(string channelName)
        {
            Console.WriteLine("You are now talking in channel " + channelName);
        }

        /// <summary>
        /// we are disconnected from a channel
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        public void DisconnectedFromChannel(string channelName)
        {
            Console.WriteLine("You have been removed from channel " + channelName);
        }

        #endregion

        #endregion

        #region Public Methods

        #region General

        /// <summary>
        /// connect
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="server">name of the server</param>
        /// <param name="port">port to be used</param>
        public void Connect(string userName, string server, int port)
        {
            try
            {
                var objProps = new Hashtable();
                objProps["name"] = "MyHttpChannel";
                objProps["port"] = port;
                var objChannel = new HttpChannel(objProps, new XmlRpcClientFormatterSinkProvider(), new XmlRpcServerFormatterSinkProvider());
                ChannelServices.RegisterChannel(objChannel, false);
                Server = XmlRpcProxyGen.Create<IServerProxy>();
                UserName = userName;
                //expose public methods of chatclient on client.rem
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatClient), "client.rem", WellKnownObjectMode.Singleton);
                Server.Connect(userName, server, port);
                Active = true;
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception(ex.FaultString);
            }
            catch (Exception)
            {
                throw new Exception("ServerIP is not active. Contact Koen Plasmans!");
            }
        }

        /// <summary>
        /// disconnect
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (Server != null && UserName != null)
                {
                    Server.Disconnect(UserName);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// get instance
        /// </summary>
        public static ChatClient GetInstance()
        {
            return _instance ?? (_instance = new ChatClient());
        }

        /// <summary>
        /// ask user name
        /// </summary>
        /// <param name="userName">new name to check</param>
        /// <returns>true if ok, false otherwise</returns>
        public bool IsUserNameOk(string userName)
        {
            try
            {
                return Server.IsUserNameOk(userName);
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception(ex.FaultString);
            }
            catch (Exception)
            {
                throw new Exception("ServerIP is not active. Contact Koen Plasmans!");
            }
        }

        /// <summary>
        /// ask user name
        /// </summary>
        /// <param name="userName">new name to check</param>
        /// <returns>true if ok, false otherwise</returns>
        public bool ChangeUserName(string userName)
        {
            try
            {
                if(Server.ChangeUserName(UserName, userName))
                {
                    UserName = userName;
                    return true;
                }
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception(ex.FaultString);
            }
            catch (Exception)
            {
                throw new Exception("ServerIP is not active. Contact Koen Plasmans!");
            }
            return false;
        }

        #endregion

        #region Channels

        /// <summary>
        /// speak in channel
        /// </summary>
        /// <param name="channelName">channel name</param>
        /// <param name="text">text to speak</param>
        public void SpeakInChannel(string channelName, string text)
        {
            try
            {
                Server.SpeakChannel(channelName, UserName, text);
            }
            catch (XmlRpcFaultException)
            {
            }
            catch
            {
            }
        }

        /// <summary>
        /// disconnect from channel
        /// </summary>
        /// <param name="channelName">channel name to disconnect</param>
        public void DisconnectChannel(string channelName)
        {
            try
            {
                Server.DisconnectChannel(UserName, channelName);
            }
            catch (XmlRpcFaultException)
            {
            }
            catch
            {
            }
        }

        /// <summary>
        /// connect to channel
        /// </summary>
        /// <param name="channelName">channel to connect to</param>
        public void ConnectChannel(string channelName)
        {
            try
            {
                if (Active)
                {
                    Server.ConnectChannel(UserName, channelName);
                }
            }
            catch (XmlRpcFaultException)
            {
            }
            catch
            {
            }
        }

        #endregion

        #endregion
    }
}