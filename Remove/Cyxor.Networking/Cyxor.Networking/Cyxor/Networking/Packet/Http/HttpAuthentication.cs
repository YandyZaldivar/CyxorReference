using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyxor.Networking
{
    class HttpAuthentication
    {
        public string UserId { get; private set; }
        public string Schema { get; private set; }
        public string Token { get; private set; }

        public HttpAuthentication(string credentials)
        {
            credentials = credentials.Trim();
            var index = credentials.IndexOf(' ');

            Schema = credentials.Substring(0, index);
            Token = credentials.Substring(index).TrimStart();

            switch (Schema.ToLowerInvariant())
            {
                case "basic":



                break;

                //case "digest": break;
                //case "bearer": break;

                default: throw new InvalidOperationException($"Not supported '{Schema}' authentication schema.");
            }
        }

        void ParseBasic(string basicCredentials)
        {

        }
    }
}
