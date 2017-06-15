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
    public class GetTIme : RequestType
    {
        override public void ExecuteRequest()
        {
            Console.WriteLine("Text is a get time request");
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            socket.Send(data);
            Console.WriteLine("Time sent to client");
        }
    }
}

