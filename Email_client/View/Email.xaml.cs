using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
using GemBox.Email.Mime;
using GemBox.Email.Security;
using System.Collections.ObjectModel;
using Email_client.Model;

namespace Email_client.View
{
   
    public partial class Email : Window
    {

      public  ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public Email()
        {
            InitializeComponent();
            Messages.Add(new MessageModel("User 1", DateTime.Now, "Hello world"));
            Messages.Add(new MessageModel("User 2", DateTime.Now, "I want to eat"));
            DataContext = this;

            //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            //using (ImapClient imap = new ImapClient("imap.gmail.com"))
            //{
            //    // Connect to mail server
            //    imap.Connect();
            //    Console.WriteLine("Connected.");

            //    // Authenticate with specified username and password 
            //    // (ImapClient will use strongest possible authentication mechanism)
            //    imap.Authenticate("<userName> ", "<password>");
            //    imap.SelectInbox();
            //    IList<ImapMessageInfo> infoList = imap.ListMessages();
            //    foreach (var info in infoList)
            //    {
            //        listBox.Items.Add("{0} "+ info.Number+" - [{1}] "+info.Uid+"- {2} Byte(s) " + info.Size);
            //    }
                
            //    imap.GetMessage(2);
            //    MailMessage message = imap.GetMessage(1);
            //    listBox.Items.Add(message.BodyText);

           // }
        }

    
    }
}
