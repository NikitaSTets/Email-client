namespace Email_client.SMTP
{
    public static class SmtpCommands
    {
        public const string Ehlo = "EHLO";//приветствие с сервером
        public const string Mail = "MAIL";//отправитель
        public const string Rcpt = "RCPT";//получатель
        public const string Data = "DATA";//текст
        public const string Quit = "QUIT";//окончание диалога с сервером
        public const string Starttls = "STARTTLS";//передача данных по защищенному каналу с использованием сертификатов
        public const string Auth = "AUTH";//аутентификация
    }
}