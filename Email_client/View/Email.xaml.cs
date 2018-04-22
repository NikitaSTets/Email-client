using System;
using System.Collections.Generic;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Text;
using System.IO;
using System.Windows.Interactivity;
namespace Email_client.View
{

    public partial class Email : Window
    {

      public  ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
     
        public Email()
        {
            InitializeComponent();
            DataContext = this;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            using (ImapClient imap = new ImapClient("imap.gmail.com"))
            {
                // Connect to mail server
                imap.Connect();

                imap.Authenticate("login", "password");
                imap.SelectInbox();
                
                IList<ImapMessageInfo> messageInfoCollection=imap.ListMessages();

                MailMessage currentMessage;
 
                string head = "<head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head>";
                string text;
                for (int i = 0; i < messageInfoCollection.Count; i++)
                {
                    currentMessage=imap.GetMessage(messageInfoCollection[i].Uid);
                    text = currentMessage.BodyHtml;
                    if (text == null)
                    {
                        text = currentMessage.BodyText;
                    }
                    currentMessage.BodyHtml = head+text;
                    if (currentMessage.BodyHtml != null)
                    {
                        Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyHtml));
                    }
                    else
                    {
                        Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyText));
                    }
                }
              

            }
            
        }

     
        private void flagTextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WebBrowser_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (ImapClient imap = new ImapClient("imap.gmail.com"))
            {
                // Connect to mail server
                imap.Connect();

                imap.Authenticate("login", "password");
                imap.SelectInbox();
                IList<ImapMessageInfo> messagesInfo = imap.ListMessages();
                var selectedMessages = listBoxOfMessages.SelectedItems;
                
                foreach (MessageModel message in selectedMessages)
                {
                    for (int i = 0; i < Messages.Count; i++)
                    {
                        if (Messages[i] == message)
                        {
                            //imap.DeleteMessage(messagesInfo[i].Uid, true);
                            //Messages.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
