using System.IO;

namespace Email_client.SMTP
{
    public class SmtpControlStreamReader : ControlStreamReader
    {
        public SmtpControlStreamReader(Stream stream)
            : base(stream)
        {

        }

        public SmtpServerReply ReadServerReply()
        {
            var reply = ReadServerReplyRaw();
            return new SmtpServerReply((SmtpReplyCode)reply.Code, reply.Raw, reply.Parsed);
        }
    }
}