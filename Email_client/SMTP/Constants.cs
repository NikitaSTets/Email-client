using System;

namespace Email_client.SMTP
{
    public static class Constants
    {
        public static readonly string TelnetEndOfLine = new string(new[] { (char)0x0D, (char)0x0A });

        public static readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1);
    }
}