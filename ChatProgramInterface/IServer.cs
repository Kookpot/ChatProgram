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
    /// interface for server
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// a clients wants to connect to the server and register itself
        /// </summary>
        /// <param name="userName">name of the user who wants to connect</param>
        /// <param name="server">IP of the client</param>
        /// <param name="port">Port of the client</param>
        /// <returns>true if connected, false otherwise</returns>
        [XmlRpcMethod("Connect")]
        bool Connect(string userName, string server, int port);

        /// <summary>
        /// a client disconnects from the server
        /// </summary>
        /// <param name="userName">name of the user</param>
        [XmlRpcMethod("Disconnect")]
        void Disconnect(string userName);

        /// <summary>
        /// a client changes (or tries to) his/her username
        /// </summary>
        /// <param name="userName">old name of the user</param>
        /// <param name="newUserName">new name of the user</param>
        /// <returns>true if username was changed, false otherwise</returns>
        [XmlRpcMethod("ChangeUserName")]
        bool ChangeUserName(string userName, string newUserName);

        /// <summary>
        /// a client asks if a username is available to take
        /// </summary>
        /// <param name="userName">name to check</param>
        /// <returns>true if the username is available, false otherwise</returns>
        [XmlRpcMethod("IsUserNameOk")]
        bool IsUserNameOk(string userName);

        /// <summary>
        /// a client wishes to disconnect from a certain channel
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="channelName">name of the channel</param>
        /// <returns>true if the client is disconnected, false otherwise</returns>
        [XmlRpcMethod("DisconnectChannel")]
        bool DisconnectChannel(string userName, string channelName);

        /// <summary>
        /// a client wants to connect to a channel
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="channelName">name of the channel</param>
        [XmlRpcMethod("ConnectChannel")]
        void ConnectChannel(string userName, string channelName);

        /// <summary>
        /// a client wishes to 'speak' something in a channel
        /// </summary>
        /// <param name="channelName">name of the channel</param>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text to 'speak'</param>
        [XmlRpcMethod("SpeakChannel")]
        void SpeakChannel(string channelName, string userName, string text);
    }
}