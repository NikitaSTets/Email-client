namespace Email_client.SMTP
{
    public static class SmtpCommands
    {

        public const string Ehlo = "EHLO";

        public const string Mail = "MAIL";
        public const string Rcpt = "RCPT";
        public const string Data = "DATA";

        public const string Quit = "QUIT";

        public const string Starttls = "STARTTLS";
        public const string Auth = "AUTH";
    }
}