using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace server
{
    static class Request
    {
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

        public static string Exit(Socket socket, List<Socket> socketList)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socketList.Remove(socket);
            }
            catch(SocketException ex)
            {
                return "Disconnect Error:" + ex.ToString();
            }

            return "client disconnected";
        }

        public static string ListingRequest(Socket socket, string[] request)
        {
            //Console.WriteLine("Request list send");
            try
            {
                foreach(string value in request)
                {
                    byte[] data = Encoding.ASCII.GetBytes(value); //poprawić pojawianie się listy w szeregu
                    socket.Send(data);
                }   
            }
            catch(SocketException ex)
            {
                return "Request list Error: " + ex.ToString();
            }
           
            return "Request list Send!";
        }
    }
}

