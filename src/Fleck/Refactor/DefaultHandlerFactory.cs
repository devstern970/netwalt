using System;
using Fleck.Interfaces;

namespace Fleck
{
    public class DefaultHandlerFactory : IHandlerFactory
    {
        public DefaultHandlerFactory()
        {
            RequestParser = new RequestParser();
        }
        
        public IRequestParser RequestParser { get; set; }

        public IHandler BuildHandler(byte[] data, Action<int> close)
        {
            if (!RequestParser.IsComplete(data))
                return null;
            
            var request = RequestParser.Parse(data);
            
            var version = GetVersion(request);
            
            switch (version)
            {
                case "76":
                    return new FakeHandler();
                case "8":
                    return new FakeHandler();
            }
            
            throw new WebSocketException("Unsupported Request");
        }
        
        public static string GetVersion(WebSocketHttpRequest request) 
        {
            string version;
            if (request.Headers.TryGetValue("Sec-WebSocket-Version", out version))
                return version;
                
            if (request.Headers.TryGetValue("Sec-WebSocket-Draft", out version))
                return version;
            
            if (request.Headers.ContainsKey("Sec-WebSocket-Key1"))
                return "76";
            
            return "75";
        }
        
        public class FakeHandler : IHandler 
        {
            public void Run()
            {
                throw new NotImplementedException();
            }

            public void Recieve(System.Collections.Generic.IEnumerable<byte> data)
            {
                throw new NotImplementedException();
            }
        }
    }
    
}

