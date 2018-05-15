using System.Windows;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Input;
using Email_client.View;
using System;
using System.Runtime.Remoting.Messaging;
using IMAP;

namespace Email_client
{

    public partial class Main : Window
    {
        ImapControl imap;
        SMTPWindow smtpWindow;
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();

        public void CreateSMTPWIndowAndConnectToServer(string userName, string password)
        {
            smtpWindow = new SMTPWindow();
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

            //Messages = imap.ListMessages();
            //ViewModel.ViewModel.ConnectToServer(ref imap, "nikitstets@gmail.com", "StackCorporation");
            ViewModel.ViewModel.UpdateListOfMessages(Messages, imap);
            ShowMessagesDataGrid.ItemsSource = Messages;
        }

        public Main()
        {
            InitializeComponent();
            CreateSMTPWIndowAndConnectToServer("nikit.stets@gmail.com", "Minsk1.1.ru");//after test will kick
                                                                                       // smtpWindow = new SMTPWindow();
                                                                                       //ViewModel.ViewModel.ConnectToServer(ref imap, "nikitstets@gmail.com", "StackCorporation");
                                                                                       //ViewModel.ViewModel.UpdateListOfMessages(Messages, imap);
                                                                                       //ShowMessagesDataGrid.ItemsSource = Messages;
        }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void comboBox1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ButtonForSendMessage_Click(object sender, RoutedEventArgs e)
        {
            smtpWindow.Show();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //ViewModel.ViewModel.UpdateListOfMessages(Messages, imap);
            imap.GetUids();
            Messages.Clear();
            foreach (var email in imap.UpdateListMessages())
            {
                Messages.Add(new MessageModel(email.From,DateTime.Now,email.Body,email.Body,email.Uid,email.Flags));
            }

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Messages)
            {
                item.Select = true;
            }
        }
        private void UnheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Messages)
            {
                item.Select = false;
            }
        }
        private void AllLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowMessagesDataGrid.ItemsSource = Messages;
            foreach (var item in Messages)
            {
                item.Select = true;
            }
        }
        private void checkBoxInColumnCircle_Click(object sender, RoutedEventArgs e)
        {

            var element = ShowMessagesDataGrid.CurrentItem;
            if (element is MessageModel)
            {
                imap.RemoveMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
                ((MessageModel)element).Color = "Aqua";
            }


        }
        private void checkBoxInColumn_Click(object sender, RoutedEventArgs e)
        {

            var currentItem = ShowMessagesDataGrid.CurrentItem as MessageModel;
            if (currentItem != null)
            {
                currentItem.Select = ((CheckBox)sender).IsChecked.Value;
            }

        }

        private void checkBoxInColumnCircle_Unchecked(object sender, RoutedEventArgs e)
        {

            var element = ShowMessagesDataGrid.CurrentItem;
            if (element is MessageModel)
            {
                imap.AddMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
                ((MessageModel)element).Color = "White";
            }
        }

        private void UnreadMessagesLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedMessages = ShowMessagesDataGrid.Items;
            foreach (MessageModel item in selectedMessages)
            {
                if (item.Select == true)
                    imap.RemoveMessageFlags(item.Uid, ImapMessageFlags.Seen);
            }
        }

        private void NoOneLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in Messages)
            {
                item.Select = false;
            }
        }

        private void ReadMessageLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            bool isRead;
            foreach (var message in Messages)
            {
                isRead = false;
                foreach (var flag in message.Flags)
                {
                    if (flag == "\\Seen")
                    {
                        isRead = true;
                    }
                }
                if (isRead)
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void UnreadMessageLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {

            MessagesTemp.Clear();
            bool isRead;
            foreach (var message in Messages)
            {
                isRead = false;
                foreach (var flag in message.Flags)
                {
                    if (flag == "\\Seen")
                    {
                        isRead = true;
                    }
                }
                if (!isRead)
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void FlaggedMessageLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            bool isFlagged;
            foreach (var message in Messages)
            {
                isFlagged = false;
                foreach (var flag in message.Flags)
                {
                    if (flag == "\\Flagged")
                    {
                        isFlagged = true;
                    }
                }
                if (isFlagged)
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void UnFlaggedMessageLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            bool isFlagged;
            foreach (var message in Messages)
            {
                isFlagged = false;
                foreach (var flag in message.Flags)
                {
                    if (flag == "\\Flagged")
                    {
                        isFlagged = true;
                    }
                }
                if (!isFlagged)
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void Incoming_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowMessagesDataGrid.ItemsSource = Messages;
        }

        private void DeleteMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)//добавить флаг,что удалено,а потом отправлять запросы на сервер,что удалено и не отображать сообщения с флагом deleted
        {

        }

        private void SentedMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            ShowMessagesDataGrid.ItemsSource = null;
        }

        private void DraftsMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            bool isFlagged;
            foreach (var message in Messages)
            {
                isFlagged = false;
                foreach (var flag in message.Flags)
                {
                    if (flag == "\\Drafted")
                    {
                        isFlagged = true;
                    }
                }
                if (!isFlagged)
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }
    }
}
