using Email_client.Model;
//using GemBox.Email;
//using GemBox.Email.Imap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using IMAP;

namespace Email_client.ViewModel
{
    public static class ViewModel
    {
        public static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }
        public static void ConnectToServer(ref ImapControl imap, string userName, string password)
        {
            LoginInfo user = new LoginInfo();
            user.ImapAddress = "imap.gmail.com";
            user.Username = userName;
            user.Password = password;
            imap = new ImapControl(993);                
            try
            {
                imap.Connect(user);
                
            }
            catch (Exception)
            { 
                MessageBox.Show("Error.Connect to server");
            }
           
        }
        public static void UpdateListOfMessages(ObservableCollection<MessageModel> Messages, ImapControl imap)
        {
            Messages.Clear();
            IList <EmailTemplate> messageInfoCollection= imap.ListMessages();
            EmailTemplate currentMessage;
            for (int i = 0; i < messageInfoCollection.Count; i++)
            {
                currentMessage = messageInfoCollection[i];//imap.GetMessage(messageInfoCollection[i].Uid);
                Messages.Add(new MessageModel(currentMessage.From,DateTime.Now,currentMessage.Body,currentMessage.Body,currentMessage.Uid,currentMessage.Flags));
            }
        }
    }
}
