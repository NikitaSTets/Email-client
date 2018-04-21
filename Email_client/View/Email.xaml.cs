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
            //string Encoding = "ISO-8859-1";
            //WebBroserForShowMessage.Document.Encoding = Encoding;
           

            Messages.Add(new MessageModel("User 1", DateTime.Now, "Hello world"));
            Messages.Add(new MessageModel("User 2", DateTime.Now, "I want to eat"));
            DataContext = this;

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            using (ImapClient imap = new ImapClient("imap.gmail.com"))
            {
                // Connect to mail server
                imap.Connect();

                imap.Authenticate("nikitstets@gmail.com", "StackCorporation");
                imap.SelectInbox();
                IList<ImapMessageInfo> a=imap.ListMessages();

                MailMessage currentMessage;
               
                Encoding utf8 = Encoding.GetEncoding("UTF-8");
                Encoding win1251 = Encoding.GetEncoding("Windows-1251");
                byte[] utf8Bytes;
                byte[] win1251Bytes;
                for (int i = 0; i < a.Count; i++)
                {
                    currentMessage=imap.GetMessage(a[i].Uid);



                    if (currentMessage.BodyHtml != null)
                    { 
                        
                        Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date,currentMessage.BodyHtml));
                    }
                    else
                    {
                        utf8Bytes = win1251.GetBytes(currentMessage.BodyText);
                        win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
                        Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, /*currentMessage.BodyText*/utf8.GetString(win1251Bytes)));
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
    }
}
