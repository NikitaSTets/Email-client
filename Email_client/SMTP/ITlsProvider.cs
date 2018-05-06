using System.IO;

namespace SMTP
{
    public interface ITlsProvider
    {
        Stream AuthenticateAsClient(Stream innerStream, string host);
    }
}