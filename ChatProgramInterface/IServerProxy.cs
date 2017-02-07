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
    /// server proxy for functions that IServer exposes
    /// We expose this at localhost on port 5678 as server.rem
    /// </summary>
    [XmlRpcUrl("http://localhost:5678/server.rem")] 
    public interface IServerProxy : IServer, IXmlRpcProxy
    {
    }
}