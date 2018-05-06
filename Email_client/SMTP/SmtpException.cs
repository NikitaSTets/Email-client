using System;

namespace SMTP
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