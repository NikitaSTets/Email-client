using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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