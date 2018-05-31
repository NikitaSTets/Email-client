using System.Net.Sockets;

namespace Email_client.SMTP
{
    public abstract class ProxyBase
    {

        public int ConnectTimeout { get; set; } = -1;
        public int ReadTimeout { get; set; } = -1;
        public int WriteTimeout { get; set; } = -1;

        public abstract TcpClient CreateConnection(string hostname, int port, TcpClient tcp);

        public virtual TcpClient CreateConnection(string hostname, int port)
        {
            return CreateConnection(hostname, port, null);
        }
    }
}