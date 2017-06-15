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
    public class RequestType 
    {
        public RequestType(Socket socket, List<Socket> socketList, Request request)
        {
            request.socket = socket;
            request.socketList = socketList;
            request.ExecuteRequest();

        }
    }
}

