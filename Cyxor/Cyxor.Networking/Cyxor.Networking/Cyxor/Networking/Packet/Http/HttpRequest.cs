using System;
using System.Net;
using System.Text;
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

        public HttpRequest(Connection connection, Serializer serializer, bool pop = true)
        {
            var header = default(string);

            try
            {
                var value = default(byte);
                var stringBuilder = new StringBuilder();
                var httpEndChars = Utilities.Http.HttpHeaderEndChars;

                while (serializer.TryDeserializeByte(out value))
                {
                    var successCount = 0;
                    stringBuilder.Append((char)value);

                    if (stringBuilder.Length >= httpEndChars.Length)
                        if (stringBuilder[stringBuilder.Length - 1] == httpEndChars[httpEndChars.Length - 1])
                            for (var i = 2; i <= httpEndChars.Length; i++)
                            {
                                if (stringBuilder[stringBuilder.Length - i] == httpEndChars[httpEndChars.Length - i])
                                    successCount++;
                                else
                                    break;
                            }

                    if (successCount == 3)
                    {
                        header = stringBuilder.ToString();
                        break;
                    }
                }
            }
            catch { return; }

            var index = Parse(header, serializer);

            if (index != -1 && Result == 0)
            {
                if (pop)
                    if (index == serializer.Int32Length)
                        serializer.Reset();
                    else
                        serializer.Pop(index);
            }
        }

        int Parse(string header, Serializer body)
        {
            var methodToken = header.Substring(0, header.IndexOf(' '));

            foreach (var methodName in Enum.GetNames(typeof(HttpMethod)))
                if (methodName.ToUpperInvariant() == methodToken.ToUpperInvariant())
                {
                    Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), methodName, ignoreCase: true);
                    break;
                }

            if (Method == HttpMethod.None)
                return -1;

            var index = header.IndexOf(Utilities.Http.NewLine);

            if (index == -1)
            {
                Result = -1;
                return index;
            }

            var line = header.Substring(0, index);

            if (!line.Substring(line.LastIndexOf(' ') + 1).StartsWith("HTTP/", StringComparison.OrdinalIgnoreCase))
                return index;

            //if (!header.Contains($"{Utilities.Http.NewLine}{Utilities.Http.NewLine}"))
            //{
            //    Result = -1;
            //    return index;
            //}

            var tokens = line.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            var uriString = tokens[1];
            ProtocolVersion = tokens[2];

            index++;

            while (!header.Substring(++index).StartsWith(Utilities.Http.NewLine))
            {
                var newIndex = header.IndexOf(Utilities.Http.NewLine, ++index) + 1;
                line = header.Substring(index - 1, newIndex - index);
                index = newIndex;
                newIndex = line.IndexOf(HeaderSeparator);
                var key = line.Substring(0, newIndex);
                var value = line.Substring(newIndex + HeaderSeparator.Length);
                Headers.Add(key.Trim().ToLowerInvariant(), value.Trim());
            }

            index += 2;

            if (header.Length != index)
            {
                // TODO: Protocol error
                Result = -1;
                return index;
            }

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

                if (body.Int32Position + ContentLength > body.Length)
                {
                    Result = index + ContentLength;
                    return header.Length;
                }
                else if (ContentLength > 0)
                {
                    Body = body.DeserializeString(byteCount: ContentLength);
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
