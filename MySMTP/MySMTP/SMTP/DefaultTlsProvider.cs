using System;
using System.IO;
using System.Net.Security;

namespace SMTP
{
    public class DefaultTlsProvider : ITlsProvider
    {
        public Stream AuthenticateAsClient(Stream innerStream, string host)
        {
            var ssl = new SslStream(innerStream, true);
            //ssl.AuthenticateAsClient();
            ssl.AuthenticateAsClient(host);
            //ssl.AuthenticateAsClient(host, null, System.Security.Authentication.SslProtocols.Ssl2,true);//true->false

            return ssl;
        }
    }
}