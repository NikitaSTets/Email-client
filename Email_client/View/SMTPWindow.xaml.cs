using System;
using System.Windows;
using Email_client.SMTP;

namespace Email_client.View
{
    public partial class SmtpWindow : Window
    {
        private SmtpConnection _smtp;

        internal string Login { get; set; }

        internal string Password { get; set; }

        private string SmtpHost { get; set; }


        public SmtpWindow()
        {
            InitializeComponent();
            _smtp = new SmtpConnection();
            
        }


        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userInfo = Login.Split('@');
                var login = userInfo[0];
                SmtpHost = userInfo[1];
                _smtp.Connect("smtp." + SmtpHost, 587);


                _smtp.ExtendedHello("bla-bla");
                _smtp.StartTls("smtp." + SmtpHost);

                _smtp.ExtendedHello("bla-bla");
                _smtp.AuthPlain(login, Password);

                _smtp.Mail(Login + "@" + SmtpHost);
                _smtp.Recipient(ToEmailTextBox.Text);
                _smtp.Data(EmailFormatter.GetText(Login + "@" + SmtpHost, TopicTextBox.Text, ToEmailTextBox.Text, null, ContentOfMessageSubHeaderText.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Close();
        }

    }
}