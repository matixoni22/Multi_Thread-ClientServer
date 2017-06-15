using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data;


namespace server.Requests
{
    public class Exit : RequestType
    {
        override public void ExecuteRequest()
        {
            base.socket.Shutdown(SocketShutdown.Both);
            base.socket.Close();
            base.socketList.Remove(base.socket);
            Console.WriteLine("Client disconnected");
        }

    }
}

