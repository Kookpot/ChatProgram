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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using CookComputing.XmlRpc;

namespace ChatProgramServer
{
    /// <summary>
    /// server main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// main loop
        /// </summary>
        /// <param name="args">possible arguments</param>
        static void Main(string[] args)
        {
            //named httpchannel listening on port 5678
            IDictionary dicProps = new Hashtable();
            dicProps["name"] = "MyHttpChannel";
            dicProps["port"] = 5678;
            var objChannel = new HttpChannel(dicProps, new XmlRpcClientFormatterSinkProvider(), new XmlRpcServerFormatterSinkProvider());
            ChannelServices.RegisterChannel(objChannel, false);
            //expose public methods of chatserver as server.rem
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatServer), "server.rem", WellKnownObjectMode.Singleton);
            Console.WriteLine("Press <ENTER> to shutdown");
            ChatServer.CloseAll(Console.ReadLine());
        }
    }
}