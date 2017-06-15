using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public class RequestType
    {
        public Socket socket{ get; set;}
        public List<Socket> socketList{ get; set;}


        public virtual void ExecuteRequest()
        {
            Console.WriteLine("Unknow request");
            byte[] data = Encoding.ASCII.GetBytes("Unknow request");
            socket.Send(data);
            Console.WriteLine("Warning sent");
        }

    }
}

