using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace server
{
    public static class Request
    { 
        /*
        private static string _request { get; private set;}

        public Request(string request)
        {
            this._request = request;
        }
        */
        public static string  GetDate(Socket socket)
        {
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
            try
            {
                socket.Send(data);
            }
            catch(SocketException ex)
            {
                return "Send data Error: "+ ex.ToString();
            }

            return "Time sent";
        }
    }
}

