using System.IO;

namespace Email_client.SMTP
{
    public interface ITlsProvider
    {
        Stream AuthenticateAsClient(Stream innerStream, string host);
    }
}