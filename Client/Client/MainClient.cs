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
        private static bool token = false;
        private static string login;
        private static string password;


        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        static void Main()
        {
            Console.Title = "Client";
            while(!ConnectToServer())
            {
                ConnectToServer();
            }
            RequestLoop();
            Exit();

        }
        /// <summary>
        /// Connects to server.
        /// </summary>
        public static bool ConnectToServer()
        {
           
            Console.WriteLine("Set ip of server:");
            string ip =  Console.ReadLine();
            Console.WriteLine("Set port:");
            string portString = Console.ReadLine();

            IPAddress ipAddress = IPAddress.Parse(ip);
            int port;
            int.TryParse(portString, out port);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
       
            clientSocket.SendTimeout = 10000;

            while (!clientSocket.Connected)
            {
                try{
                    clientSocket.Connect(endPoint);
                   
               }
                catch(SocketException){
                    Console.WriteLine("Cannot connect to server. Verify your connection and server address");
                    return false;
                }
            }
            return true;
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
            if(token == false)
            {
                
                Console.WriteLine("Login:");
                login = Console.ReadLine();
                Console.WriteLine("Password:");
                password = Console.ReadLine();
                SendString(password +"|"+ login);
            }
            else{
                Console.WriteLine("Send a request: ");
                string request = Console.ReadLine();
                SendString(request);

                if(request.ToLower() == "exit")
                {
                    Exit();
                }
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
            if(token == false)
            {
                if(text == "auth approve")
                {
                    token = true;
                    Console.WriteLine("Authentication approve");
                }
                else{
                    Console.WriteLine("Authentication false");

                }
            }
            else
            {
                Console.WriteLine(text);
            }
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
