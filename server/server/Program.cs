using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace server
{
    class MainServer
    {
        //port number
        private const int _portNumber = 2000 ;
        //określenie rodzaju gniazda 
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //lista klientów
        private static List<Socket> _clientSockets = new List<Socket>();
        //bufor
        private const int _bufferSize = 2048;
        private static readonly byte[] _buffer =  new byte[_bufferSize];

        private static string[] _request = { "get time", "exit", "send string", "request list" };
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        static void Main()
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
            CloseAllSocket();
            
        }
        /// <summary>
        /// Setups the server.
        /// </summary>
        private static void SetupServer()
        { 
            Console.WriteLine("Setting server...");
            try{
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _portNumber));
                _serverSocket.Listen(1);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                Console.WriteLine("Server setup succesful complete");
            }
            catch(SocketException ex)
            {
                Console.WriteLine("ERROR: "+ ex);
            }

        }
        /// <summary>
        /// Closes all socket.
        /// </summary>
        private static void CloseAllSocket()
        {
            foreach(Socket socket in _clientSockets)
            {
                Console.WriteLine("Closeing all socket");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        /// <summary>
        /// Accepts the callback.
        /// </summary>
        /// <param name="asyncResult">Async result.</param>
        private static void AcceptCallback (IAsyncResult asyncResult)
        {
            Socket socket = _serverSocket.EndAccept(asyncResult);
            _clientSockets.Add(socket);
            socket.BeginReceive(_buffer,0,_buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }
        /// <summary>
        /// Receives the callback.
        /// </summary>
        /// <param name="asyncResult">Async result.</param>
        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket current = (Socket)asyncResult.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(asyncResult); 
            }
            catch (SocketException)
            {
                Console.WriteLine("Client force to close");
                current.Close();
                _clientSockets.Remove(current);
                return;

            }

            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Text received:" + text);

            //Get time
            if (text.ToLower() == _request[0])
            {
                Console.WriteLine(Request.GetDate(current));
            }
            //exit
            else if (text.ToLower() == _request[1])
            {
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                _clientSockets.Remove(current);
                Console.WriteLine("client disconnected");
                return;
            }
            //request list
            else if (text.ToLower() == _request[3] )
            {
                //Console.WriteLine("Request list send");
                foreach(string value in _request)
                {
                    byte[] data = Encoding.ASCII.GetBytes(value); //poprawić pojawianie się listy w szeregu
                    current.Send(data);
                }
                Console.WriteLine("Request list sent");

            }
            //Unknow
            else
            {
                Console.WriteLine("Unknow request");
                byte[] data = Encoding.ASCII.GetBytes("Unknow request");
                current.Send(data);
                Console.WriteLine("Warning sent");
            }

            current.BeginReceive(_buffer, 0, _bufferSize, SocketFlags.None, ReceiveCallback, current);
      
        }
    }

}