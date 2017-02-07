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

namespace ChatProgramServer
{
    /// <summary>
    /// manages the speakers and channels
    /// </summary>
    public class ChatManager
    {
        #region Properties

        // instance
        private static ChatManager _instance;
        //all connected speakers
        public Dictionary<string, Speaker> Speakers { get; set; }
        //channels
        public Dictionary<string, Channel> Channels { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        private ChatManager()
        {
            Speakers = new Dictionary<string, Speaker>();
            Channels = new Dictionary<string, Channel>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// get instance (singleton)
        /// </summary>
        /// <returns>Chatmanager instance of singleton</returns>
        public static ChatManager GetInstance()
        {
            return _instance ?? (_instance = new ChatManager());
        }

        #endregion
    }
}