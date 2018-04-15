﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
using GemBox.Email.Mime;
using GemBox.Email.Security;


namespace Email_client.View
{
   
    public partial class Email : Window
    {
      
        public Email()
        {
            InitializeComponent();
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            using (ImapClient imap = new ImapClient("imap.gmail.com"))
            {
                // Connect to mail server
                imap.Connect();
                Console.WriteLine("Connected.");

                // Authenticate with specified username and password 
                // (ImapClient will use strongest possible authentication mechanism)
                imap.Authenticate("<userName> ", "<password>");
                imap.SelectInbox();
                IList<ImapMessageInfo> infoList = imap.ListMessages();
                foreach (var info in infoList)
                {
                    listBox.Items.Add("{0} "+ info.Number+" - [{1}] "+info.Uid+"- {2} Byte(s) " + info.Size);
                }
                
                imap.GetMessage(2);
                MailMessage message = imap.GetMessage(1);
                listBox.Items.Add(message.BodyText);

            }
        }

    
    }
}