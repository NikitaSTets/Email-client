using System;

namespace Email_client.SMTP
{
    public class SmtpException : Exception
    {
        public SmtpException()
        {

        }

        public SmtpException(string message) : base(message)
        {

        }
    }
}