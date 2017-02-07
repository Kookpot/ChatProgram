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

using System.Collections.Generic;
using ChatProgramInterface;
using System.Threading;

namespace ChatProgramServer
{
    /// <summary>
    /// speaker
    /// </summary>
    public class Speaker
    {
        #region Properties

        //connection to client functionality
        public IClient Client { get; set; }
        //channels of speaker
        public Dictionary<string, Channel> Channels { get; set; }
        //name of the speaker
        public string UserName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// <param name="userName">name of the speaker</param>
        /// <param name="client">connected client</param>
        /// </summary>
        public Speaker(string userName, IClient client)
        {
            UserName = userName;
            Client = client;
            Channels = new Dictionary<string, Channel>();
        }

        #endregion

        #region public methods

        /// <summary>
        /// server down
        /// </summary>
        /// <param name="message">message for speakers of the chat program when closing down the server</param>
        public void ServerDown(string message)
        {
            //in thread to do a fire and forget
            new Thread(() => ServerDownThreaded(message)).Start();
        }

        /// <summary>
        /// server down
        /// </summary>
        /// <param name="message">message for speakers of the chat program when closing down the server</param>
        private void ServerDownThreaded(string message)
        {
            Client.ServerDown(message);
        }

        /// <summary>
        /// speak
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        /// <param name="user">name of the user</param>
        /// <param name="text">text to speak</param>
        public void Speak(string channelName, string user, string text)
        {
            //in thread to do a fire and forget
            new Thread(() => SpeakThreaded(channelName, user, text)).Start();
        }

        /// <summary>
        /// speak (threaded)
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text to speak</param>
        private void SpeakThreaded(string channelName, string userName, string text)
        {
            try
            {
                Client.ReceiveMessage(channelName, userName, text);
            }
            catch
            {
                //swallow any exceptions : this is non-production code ;-)
            }
        }

        /// <summary>
        /// add channel
        /// </summary>
        /// <param name="channel">channel to add to speaker</param>
        public void AddChannel(Channel channel)
        {
            //check if the speaker doesn't have the channel yet
            if (!Channels.ContainsKey(channel.Name))
            {
                Channels.Add(channel.Name, channel);
                new Thread(() => AddChannelThreaded(channel)).Start();
            }
        }

        /// <summary>
        /// add channel (threaded)
        /// </summary>
        /// <param name="channel">channel to add to speaker</param>
        private void AddChannelThreaded(Channel channel)
        {
            try
            {
                Client.ConnectedToChannel(channel.Name);
            }
            catch
            {
                Channels.Remove(channel.Name);
            }
        }

        /// <summary>
        /// remove channel
        /// </summary>
        /// <param name="channelName">name of the channel to remove</param>
        public void RemoveChannel(string channelName)
        {
            //check if the speaker is in the channel
            if (!Channels.ContainsKey(channelName))
            {
                Channels.Remove(channelName);
                new Thread(() => RemoveChannelThreaded(channelName)).Start();
            }
        }

        /// <summary>
        /// remove channel (threaded)
        /// </summary>
        /// <param name="strChannel">name of the channel to remove</param>
        private void RemoveChannelThreaded(string strChannel)
        {
            try
            {
                Client.DisconnectedFromChannel(strChannel);
            }
            catch
            {
            }
        }

        #endregion
    }
}