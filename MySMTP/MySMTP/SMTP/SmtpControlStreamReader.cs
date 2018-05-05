using System;
using System.Diagnostics;
using System.IO;

namespace SMTP
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