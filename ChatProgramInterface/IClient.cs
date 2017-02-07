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

using CookComputing.XmlRpc;

namespace ChatProgramInterface
{
    /// <summary>
    /// interface for client of chatprogram
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// receive a message from a user on a specific channel
        /// </summary>
        /// <param name="channelName">name of the channel where someone spoke into</param>
        /// <param name="userName">username of speaker</param>
        /// <param name="text">text that was send</param>
        [XmlRpcMethod("ReceiveMessage")]
        void ReceiveMessage(string channelName, string userName, string text);

        /// <summary>
        /// client is connected to a specific channel (and receives acknowledge)
        /// </summary>
        /// <param name="channelName">name of the channel the client is now connected to</param>
        [XmlRpcMethod("ConnectedToChannel")]
        void ConnectedToChannel(string channelName);
        
        /// <summary>
        /// client is disconnected from a specific channel (and receives acknowledge)
        /// </summary>
        /// <param name="channelName"></param>
        [XmlRpcMethod("DisconnectedFromChannel")]
        void DisconnectedFromChannel(string channelName);

        /// <summary>
        /// server went down
        /// </summary>
        /// <param name="message">message of the server why it went down</param>
        [XmlRpcMethod("ServerDown")]
        void ServerDown(string message);
    }
}