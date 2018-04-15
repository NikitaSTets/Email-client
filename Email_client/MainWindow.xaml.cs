using System;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Mail;
using Email_client.View;
using Microsoft.Win32;

namespace Email_client
{ 
    public partial class MainWindow : Window
    {
        OpenFileDialog ofAttachment;
        string fileName;
 
        public MainWindow()
        {
            InitializeComponent();        
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                ofAttachment = new OpenFileDialog();
                ofAttachment.Filter = "Images(.jpg,.png)|*.png;*.jpg;|Pdf Files|*.pdf";
                Nullable<bool> result = ofAttachment.ShowDialog();
                if (result==true)
                {
                    fileName = ofAttachment.FileName;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Smtp client details
                //gmail>>smtp server :smtp.gmail.com,port 587,ssl required
                SmtpClient clientDetails = new SmtpClient();
                clientDetails.Port = Convert.ToInt32(clientParametrsPortTextBox.Text.Trim());
                clientDetails.Host = clientParametrsSmtpServerTextBox.Text.Trim();
                Nullable<bool> result = clientParametrsProtocolSSICheckBox.IsChecked;
                clientDetails.EnableSsl = result.Value;//?????
                clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                clientDetails.UseDefaultCredentials = false;
                //message detail
                clientDetails.Credentials = new NetworkCredential(fromEmailTextBox.Text.Trim(), fromPasswordTextBox.Password.Trim());
                MailMessage mailDetails = new MailMessage();
                mailDetails.From = new MailAddress(fromEmailTextBox.Text.Trim());
                mailDetails.To.Add(toEmailTextBox.Text.Trim());
                //for multiple reciptionists
                //mailDetails.To.Add("another recipient email address")
                //for bcc
                //mailDetail.Bcc.Add("bcc email address");
                mailDetails.Subject = toEmailTextBox.Text.Trim();
                mailDetails.Body = contentOfMessageSubHeaderText.Text.Trim();
                if (fileName.Length > 0)
                {
                    Attachment attachment = new Attachment(fileName);
                    mailDetails.Attachments.Add(attachment);
                }
                clientDetails.Send(mailDetails);
                fileName = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetEmailsButton_Click(object sender, RoutedEventArgs e)
        {
            Email emails = new Email();
            emails.Show();
        }
    }
}
