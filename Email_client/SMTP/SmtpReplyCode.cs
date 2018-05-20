namespace Email_client.SMTP
{
    public enum SmtpReplyCode
    {
        /// <summary>
        /// Service ready
        /// </summary>
        ServiceReady = 220,

        /// <summary>
        /// Authentication Succeeded
        /// </summary>
        AuthSucceded = 235,

        /// <summary>
        /// Requested mail action okay, completed
        /// </summary>
        Ok = 250,

        /// <summary>
        /// Start mail input
        /// </summary>
        StartInput = 354,

        /// <summary>
        /// Authentication credentials invalid
        /// </summary>
        InvalidCredentials = 535,

    }
}