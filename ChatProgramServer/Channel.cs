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
using System.Linq;

namespace ChatProgramServer
{
    /// <summary>
    /// channel
    /// </summary>
    public class Channel
    {
        #region Properties

        //name of the channel
        public string Name { get; set; }
        //speakers/users in channel
        public Dictionary<string, Speaker> Speakers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of the channel</param>
        public Channel(string name)
        {
            Name = name;
            Speakers = new Dictionary<string, Speaker>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// speak
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text to speak</param>
        public void Speak(string userName, string text)
        {
            foreach (var key in Speakers.Keys.Where(strKey => !strKey.Equals(userName)))
            {
                Speakers[key].Speak(Name, userName, text);
            }
        }

        /// <summary>
        /// speak except
        /// </summary>
        /// <param name="userName">name of the user</param>
        /// <param name="text">text to speak</param>
        /// <param name="except">name of the sender who shouldn't receive the message</param>
        public void SpeakExcept(string userName, string text, string except)
        {
            foreach (var key in Speakers.Keys.Where(strKey => !strKey.Equals(except)))
            {
                Speakers[key].Speak(Name, userName, text);
            }
        }

        #endregion
    }
}