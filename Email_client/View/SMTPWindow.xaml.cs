using System;
using System.Windows;
using Microsoft.Win32;
using SMTP;


namespace Email_client
{
    public partial class SMTPWindow : Window
    {
        private SmtpConnection _smtp;

        internal string Login { get; set; }

        internal string Password { get; set; }

        private string _smtpHost { get; set; }


        public SMTPWindow()
        {
            InitializeComponent();
            _smtp = new SmtpConnection();
            
        }


        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Smtp client details
                //gmail>>smtp server :smtp.gmail.com,port 587,ssl required
                var userInfo = Login.Split('@');
                var login = userInfo[0];
                _smtpHost = userInfo[1];
                _smtp.Connect("smtp." + _smtpHost, 587);


                _smtp.ExtendedHello("bla-bla");
                _smtp.StartTls("smtp." + _smtpHost);

                _smtp.ExtendedHello("bla-bla");
                _smtp.AuthPlain(login, Password);

                _smtp.Mail(Login + "@" + _smtpHost);
                _smtp.Recipient(ToEmailTextBox.Text);
                _smtp.Data(EmailFormatter.GetText(Login + "@" + _smtpHost, TopicTextBox.Text, ToEmailTextBox.Text, null, ContentOfMessageSubHeaderText.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Close();
        }

    }
}