using System;

namespace server.Requests
{
    public class RequestTake
    {
        private string request;
        public RequestTake(String request)
        {
            this.request = request;
        }

        public Request ChooseRequest()
        {
            switch(request)
            {
                case "gettime":
                    return new GetTime();
                case "exit":
                    return new Exit();
                default:
                    return new Request();
                    
            }
        }
    }
}

