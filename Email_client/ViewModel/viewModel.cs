using Email_client.Model;
using GemBox.Email;
using GemBox.Email.Imap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Email_client.ViewModel
{
  public static class ViewModel
    {
        public static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }
        public static void ConnectToServer(ref ImapClient imap, string userName, string password)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            imap = new ImapClient("imap.gmail.com");

            // Connect to mail server
            try
            {
                imap.Connect();
                imap.Authenticate(userName, password);
                imap.SelectInbox();
            }
            catch (Exception)
            {

                MessageBox.Show("Error.Connect to server");
            }
           
        }
        public static void UpdateListOfMessages(ObservableCollection<MessageModel> Messages, ImapClient imap)
        {
            Messages.Clear();
            IList<ImapMessageInfo> messageInfoCollection = imap.ListMessages();
            MailMessage currentMessage;
            string head = "<head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head>";
            string text;
            for (int i = 0; i < messageInfoCollection.Count; i++)
            {
                currentMessage = imap.GetMessage(messageInfoCollection[i].Uid);
                text = currentMessage.BodyHtml;
                if (text == null)
                {
                    text = currentMessage.BodyText;
                }
                currentMessage.BodyHtml = head + text;
                if (currentMessage.BodyHtml != null)
                {
                    Messages.Add(new MessageModel(currentMessage.From[0].User,currentMessage.Date,currentMessage.BodyHtml,currentMessage.BodyText, messageInfoCollection[i].Uid));
                }
                else
                {
                    Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyHtml, currentMessage.BodyText, messageInfoCollection[i].Uid));
                }
            }
        }
    }
}
