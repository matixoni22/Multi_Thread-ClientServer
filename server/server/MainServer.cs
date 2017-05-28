using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data;


namespace server
{
    class MainServer 
    {
        private const int portNumber = 2000 ;
        //określenie rodzaju gniazda 
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //lista klientów
        private static List<Socket> clientSockets = new List<Socket>();
        //bufor
        private const int bufferSize = 2048;
        private static readonly byte[] buffer =  new byte[bufferSize];

        private static string[] request = { "get time", "exit", "send string", "request list" };
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
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, portNumber));
                serverSocket.Listen(1);
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

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
            foreach(Socket socket in clientSockets)
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
            Socket socket = serverSocket.EndAccept(asyncResult);
            clientSockets.Add(socket);
            socket.BeginReceive(buffer,0,buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

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
                clientSockets.Remove(current);
                return;

            }

            byte[] dataBuf = new byte[received];
            Array.Copy(buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Text received:" + text);

            //Get time
            if (text.ToLower() == request[0])
            {
                Console.WriteLine(Request.GetDate(current));
            }
            //exit
            else if (text.ToLower() == request[1])
            {
                Console.WriteLine(Request.Exit(current, clientSockets));
            }
            //request list
            else if (text.ToLower() == request[3] )
            {
                Console.WriteLine(Request.ListingRequest(current, request));
            }
            //Unknow
            else
            {
                Console.WriteLine("Unknow request");
                byte[] data = Encoding.ASCII.GetBytes("Unknow request");
                current.Send(data);
                Console.WriteLine("Warning sent");
            }
            current.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, current);
        }
    }

}