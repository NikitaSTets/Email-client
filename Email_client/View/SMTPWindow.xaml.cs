using System;
using System.Windows;
//using System.Net;
//using System.Net.Mail;
using Email_client.View;
using Microsoft.Win32;
using SMTP;

namespace Email_client
{
    public partial class SMTPWindow : Window
    {
        OpenFileDialog ofAttachment;
        string fileName;
        Email emails;


        public SMTPWindow()
        {
            InitializeComponent();
            //emails = new Email();
            //Closing += viewModel.OnWindowClosing;
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ofAttachment = new OpenFileDialog();
                ofAttachment.Filter = "Images(.jpg,.png)|*.png;*.jpg;|Pdf Files|*.pdf";
                Nullable<bool> result = ofAttachment.ShowDialog();
                if (result == true)
                {
                    fileName = ofAttachment.FileName;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //Do you want to send empty message?

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Smtp client details
                //gmail>>smtp server :smtp.gmail.com,port 587,ssl required
                var smtp = new SmtpConnection();
                var userInfo = fromEmailTextBox.Text.Split('@');
                var login = userInfo[0];
                var smtpHost = userInfo[1];
                smtp.Connect("smtp." + smtpHost, 587);


                smtp.ExtendedHello("bla-bla");
                smtp.StartTls(smtpHost);

                smtp.ExtendedHello("bla-bla");
                smtp.AuthPlain(login, fromPasswordTextBox.Password);

                smtp.Mail(login + "@" + smtpHost);
                smtp.Recipient(toEmailTextBox.Text);
                
                smtp.Data(EmailFormatter.GetText(login + "@" + smtpHost, topicTextBox.Text, toEmailTextBox.Text, null, contentOfMessageSubHeaderText.Text));
                //smtp.Data(EmailFormatter.GetHtml(login+smtpHost, "Subject",toEmailTextBox.Text, null,
                //    " < h1>Hello!</h1> <a href='stackoverflow.com'>click!</a>"));

                smtp.Quit();
                //    SmtpClient clientDetails = new SmtpClient();
                //    clientDetails.Port = Convert.ToInt32(clientParametrsPortTextBox.Text.Trim());
                //    clientDetails.Host = clientParametrsSmtpServerTextBox.Text.Trim();
                //    bool? result = clientParametrsProtocolSSICheckBox.IsChecked;
                //    clientDetails.EnableSsl = result.Value;//?????
                //    clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                //    clientDetails.UseDefaultCredentials = false;
                //    //message detail
                //    clientDetails.Credentials = new NetworkCredential(fromEmailTextBox.Text.Trim(), fromPasswordTextBox.Password.Trim());
                //    MailMessage mailDetails = new MailMessage();
                //    mailDetails.From = new MailAddress(fromEmailTextBox.Text.Trim());
                //    mailDetails.To.Add(toEmailTextBox.Text.Trim());
                //    //for multiple reciptionists
                //    //mailDetails.To.Add("another recipient email address")
                //    //for bcc
                //    //mailDetail.Bcc.Add("bcc email address");
                //    mailDetails.Subject = toEmailTextBox.Text.Trim();
                //    mailDetails.Body = contentOfMessageSubHeaderText.Text.Trim();
                //    if (fileName.Length > 0)
                //    {
                //        Attachment attachment = new Attachment(fileName);
                //        mailDetails.Attachments.Add(attachment);
                //    }
                //    clientDetails.Send(mailDetails);
                //    fileName = "";
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetEmailsButton_Click(object sender, RoutedEventArgs e)
        {
            emails.Show();
        }
    }
}
