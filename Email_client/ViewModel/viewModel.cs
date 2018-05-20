using Email_client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Email_client.IMap;

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
            IList <MessageModel> messageInfoCollection= imap.ListMessages();
            for (int i = 0; i < messageInfoCollection.Count; i++)
            {                
                messageInfoCollection[i].TextHTML = "<!DOCTYPE HTML><html><head><meta http-equiv = 'Content-Type' content = 'text/html;charset=UTF-8'></head><body>" + messageInfoCollection[i].TextHTML +"</body></html>";
                     Messages.Add(messageInfoCollection[i]);//что-то мне подсказывает,что там уже есть html
            }
        }
    }
}
