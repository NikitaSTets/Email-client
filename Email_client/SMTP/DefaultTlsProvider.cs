using System.IO;
using System.Net.Security;

namespace Email_client.SMTP
{
    public class DefaultTlsProvider : ITlsProvider
    {
        public Stream AuthenticateAsClient(Stream innerStream, string host)
        {
            var ssl = new SslStream(innerStream, true);
            ssl.AuthenticateAsClient(host);
        
            return ssl;
        }
    }
}