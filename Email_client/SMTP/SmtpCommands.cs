namespace SMTP
{
    public static class SmtpCommands
    {
        /// <summary>
        /// Extended Hello
        /// </summary>
        public const string EHLO = "EHLO";

        public const string MAIL = "MAIL";
        public const string RCPT = "RCPT";
        public const string DATA = "DATA";

        public const string QUIT = "QUIT";

        public const string STARTTLS = "STARTTLS";
        public const string AUTH = "AUTH";
    }
}