using System.Collections.Generic;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using Email_client.ViewModel;
using Email_client.View;
using System;

namespace Email_client
{

    public partial class Main : Window
    {
        ImapClient imap;
        SMTPWindow smtpWindow;
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();
        public Main()
        {
            InitializeComponent();
            smtpWindow = new SMTPWindow();
            ViewModel.ViewModel.ConnectToServer(ref imap, "nikitstets@gmail.com", "StackCorporation");
            ViewModel.ViewModel.UpdateListOfMessages(Messages, imap);
            ShowMessagesDataGrid.ItemsSource = Messages;
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
            ViewModel.ViewModel.UpdateListOfMessages(Messages, imap);
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
            //foreach (var item in Messages)
            //{
            //    item.Select = true;
            //}
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
                foreach (var flag in imap.GetMessageFlags(message.Uid))
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
                foreach (var flag in imap.GetMessageFlags(message.Uid))
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
    }
}
