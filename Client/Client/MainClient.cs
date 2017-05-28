using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Client
{
    class MainClient
    {
        private static readonly Socket clientSocket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int port = 2000;


        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        static void Main()
        {
            Console.Title = "Client";
            ConnectToServer();
            RequestLoop();
            Exit();

        }
        /// <summary>
        /// Connects to server.
        /// </summary>
        public static void ConnectToServer()
        {
            int attempts = 0;
            IPAddress ipAddress = IPAddress.Parse("192.168.56.101");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts.ToString());
                    clientSocket.Connect(endPoint);
                }
                catch(SocketException ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }

            Console.WriteLine("Connected to server");
        }
        /// <summary>
        /// Requests the loop.
        /// </summary>
        public static void RequestLoop()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnet client>");

            while (true)
            {
                SendRequest();
                ReceiveRespond();

            }
        }
        /// <summary>
        /// Exit this instance.
        /// </summary>
        public static void Exit()
        {
            SendString("exit");
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Environment.Exit(0);
        }
        /// <summary>
        /// Sends the request.
        /// </summary>
        public static void SendRequest()
        {
            Console.WriteLine("Send a request: ");
            string request = Console.ReadLine();
            SendString(request);

            if(request.ToLower() == "exit")
            {
                Exit();
            }
        }
        /// <summary>
        /// Receives the respond.
        /// </summary>
        public static void ReceiveRespond()
        {
            var buffer = new byte[2048];
            int received = clientSocket.Receive(buffer, SocketFlags.None);

            if(received == 0)
            {
                return;
            }

            var data = new byte[received];
            Array.Copy(buffer, data, received);

            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine(text);

        }
        /// <summary>
        /// Sends the string.
        /// </summary>
        /// <param name="request">Request.</param>
        public static void SendString(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

        }
    }
}
