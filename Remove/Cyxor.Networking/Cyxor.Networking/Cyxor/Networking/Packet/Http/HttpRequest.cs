using System;
using System.Net;
using System.Collections.Generic;

namespace Cyxor.Networking
{
    using Serialization;

    public enum HttpMethod
    {
        None,
        Get,
        Head,
        Put,
        Post,
        Patch,
        Trace,
        Delete,
        Connect,
        Options,
    }

    public class HttpRequest
    {
        static string HeaderSeparator { get; } = ": ";
        static char[] SpaceSeparator { get; } = new char[] { ' ' };

        static string HttpHeaderHost { get; } = "Host".ToLowerInvariant();
        static string HttpHeaderCors { get; } = "Host".ToLowerInvariant();
        static string HttpHeaderOrigin { get; } = "Origin".ToLowerInvariant();
        static string HttpHeaderAuthorization { get; } = "Authorization".ToLowerInvariant();
        static string HttpHeaderContentLength { get; } = "Content-Length".ToLowerInvariant();
        static string HttpHeaderContentLocation { get; set; } = "Content-Location".ToLowerInvariant();

        Dictionary<string, string> Headers = new Dictionary<string, string>();
        public string GetHeaderValue(string header) => Headers.ContainsKey(header) ? Headers[header] : null;

        public Uri Uri { get; private set; }
        public string Api { get; private set; }
        public string Body { get; private set; }
        public int? Result { get; private set; }
        public string Origin { get; private set; }
        public bool CorsHeader { get; private set; }
        public string HostHeader { get; private set; }
        public int ContentLength { get; private set; }
        public HttpMethod Method { get; private set; }
        public string Authorization { get; private set; }
        public string ProtocolVersion { get; private set; }
        public NetworkCredential Credentials { get; private set; }

        public HttpRequest(string content) => Parse(content);

        public HttpRequest(Connection connection, Serializer serializer, bool pop = true)
        {
            var content = default(string);

            try { content = serializer.ToString(); }
            catch { return; }

            var index = Parse(content);

            if (index != -1 && Result == 0)
            {
                if (pop)
                    if (index == serializer.Length)
                        serializer.Reset();
                    else
                        serializer.Pop(index);
            }
        }

        int Parse(string content)
        {
            var methodToken = content.Substring(0, content.IndexOf(' '));

            foreach (var methodName in Enum.GetNames(typeof(HttpMethod)))
                if (methodName.ToUpperInvariant() == methodToken.ToUpperInvariant())
                {
                    Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), methodName, ignoreCase: true);
                    break;
                }

            if (Method == HttpMethod.None)
                return -1;

            var index = content.IndexOf(Utilities.Http.NewLine);

            if (index == -1)
            {
                Result = -1;
                return index;
            }

            var line = content.Substring(0, index);

            if (!line.Substring(line.LastIndexOf(' ') + 1).StartsWith("HTTP/", StringComparison.OrdinalIgnoreCase))
                return index;

            if (!content.Contains($"{Utilities.Http.NewLine}{Utilities.Http.NewLine}"))
            {
                Result = -1;
                return index;
            }

            var tokens = line.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            var uriString = tokens[1];
            ProtocolVersion = tokens[2];

            index++;

            while (!content.Substring(++index).StartsWith(Utilities.Http.NewLine))
            {
                var newIndex = content.IndexOf(Utilities.Http.NewLine, ++index) + 1;
                line = content.Substring(index - 1, newIndex - index);
                index = newIndex;
                newIndex = line.IndexOf(HeaderSeparator);
                var key = line.Substring(0, newIndex);
                var value = line.Substring(newIndex + HeaderSeparator.Length);
                Headers.Add(key.Trim().ToLowerInvariant(), value.Trim());
            }

            index += 2;

            if (!Headers.ContainsKey(HttpHeaderHost))
                Uri = new Uri(uriString);
            else
            {
                HostHeader = Headers[HttpHeaderHost];
                Uri = new Uri(new Uri($"http://{HostHeader}"), uriString);
            }

            Api = Uri.UnescapeDataString(Uri.PathAndQuery.Substring(1));

            if (Headers.ContainsKey(HttpHeaderContentLocation))
                Api += Headers[HttpHeaderContentLocation];

            if (Headers.ContainsKey(HttpHeaderContentLength))
            {
                ContentLength = int.Parse(Headers[HttpHeaderContentLength]);

                if (content.Length < index + ContentLength)
                {
                    Result = index + ContentLength;
                    return content.Length;
                }
                else if (ContentLength > 0)
                {
                    Body = content.Substring(index, ContentLength);
                    Api += " " + Body;
                    index += ContentLength;
                }
            }

            if (Headers.ContainsKey(HttpHeaderOrigin))
                Origin = Headers[HttpHeaderOrigin];

            if (Headers.ContainsKey(HttpHeaderAuthorization))
            {

                Authorization = Headers[HttpHeaderAuthorization];
                //Credentials = new NetworkCredential()
            }

            //System.Net.CredentialCache.

            Result = 0;
            return index;
        }
    }
}
