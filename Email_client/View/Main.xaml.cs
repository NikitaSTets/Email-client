using System.Windows;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Input;
using Email_client.View;
using System;
using IMAP;

namespace Email_client
{
    public partial class Main : Window
    {
        ImapControl _imap;

        SMTPWindow _smtpWindow;

        private string Login { get; set; }

        private string Password { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();

        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();


        public void CreateSmtpWindowAndConnectToServer(string userName, string password)
        {
            _smtpWindow = new SMTPWindow();
            LoginInfo user = new LoginInfo();
            user.ImapAddress = "imap.gmail.com";
            user.Username = userName;
            user.Password = password;
            Login = userName;
            Password = password;
            _imap = new ImapControl(993);
            try
            {
                _imap.Connect(user);

            }
            catch (Exception)
            {
                MessageBox.Show("Error.Connect to server");
            }
            ViewModel.ViewModel.UpdateListOfMessages(Messages, _imap);
            ShowMessagesDataGrid.ItemsSource = Messages;
            BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, Messages[0].TextHTML); ;
        }


        public Main()
        {
            InitializeComponent();
            CreateSmtpWindowAndConnectToServer("login", "password");
        }


        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonForSendMessage_Click(object sender, RoutedEventArgs e)
        {
            _smtpWindow.Login = Login;
            _smtpWindow.Password = Password;
            _smtpWindow.Show();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _imap.GetUids();
            Messages.Clear();
            foreach (var email in _imap.UpdateListMessages())
            {
                email.TextHTML = "< meta http - equiv = 'Content-Type' content = 'text/html;charset=UTF-8'>" + email.TextHTML;
                Messages.Add(new MessageModel(email.From, DateTime.Now, email.TextHTML, email.Body, email.Uid, email.Flags));
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
            
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if(!message.HasFlag("\\Deleted"))
                   MessagesTemp.Add(message);
            }
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
                _imap.RemoveMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
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
                _imap.AddMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
                ((MessageModel)element).Color = "White";
            }
        }

        private void UnreadMessagesLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedMessages = ShowMessagesDataGrid.Items;
            foreach (MessageModel item in selectedMessages)
            {
                if (item.Select == true)
                {                    
                    item.RemoveFlag("\\Seen");
                }
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
            foreach (var message in Messages)
            {
                if (message.HasFlag("\\Seen"))
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void UnreadMessageLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            { 
                
                if (!message.HasFlag("\\Seen"))
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void FlaggedMessageLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {

                if (message.HasFlag("\\Flagged"))
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void UnFlaggedMessageLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            { 
                
                if (!message.HasFlag("\\Flagged"))
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void Incoming_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (!message.HasFlag("\\Deleted"))
                    MessagesTemp.Add(message);
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private  void DeleteMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)//добавить флаг,что удалено,а потом отправлять запросы на сервер,что удалено и не отображать сообщения с флагом deleted
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (message.Select)
                {
                    message.AddFlag("\\Deleted");
                    continue;
                }
                MessagesTemp.Add(message);
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void SentedMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowMessagesDataGrid.ItemsSource = null;
        }

        private void DraftsMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (message.HasFlag("\\Drafted"))
                {
                    MessagesTemp.Add(message);
                }
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }

        private void DataGridLabelShowCurrentMessageMessage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var content = ((Label)sender).Content;
            foreach (var message in Messages)
            {
                if (content == message.Text)
                {
                    BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, message.TextHTML);
                    break;
                }

            }
        }

        private void DraftedMessagesLabel_OnMouse(object sender, MouseButtonEventArgs e)
        {
           MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if(message.HasFlag("\\Drafted"))
                    MessagesTemp.Add(message);
            }

            ShowMessagesDataGrid.ItemsSource = MessagesTemp;
        }
    }
}