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
    public class Request 
    {
        RequestType requestType;

        public Request(string request, Socket socket, List<Socket> socketList)
        {
            switch(request)
            {
                case "gettime":
                    requestType = new GetTIme();
                    requestType.socket = socket;
                    requestType.ExecuteRequest();
                    break;
                case "exit":
                    requestType = new Exit();
                    requestType.ExecuteRequest();
                    break;
                default:
                    requestType.ExecuteRequest();
                    break;
                    
            }

        }
    }
}

