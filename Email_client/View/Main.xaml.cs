using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Email_client.IMap;
using Email_client.Model;

namespace Email_client.View
{
    public partial class Main : Window
    {
        ImapControl _imap;

        SMTPWindow _smtpWindow;
        ImprovedDictionary <string,string> comandsForAddingToTheServer;
        ImprovedDictionary<string, string> comandsForDeletingToTheServer;

        private string Login { get; set; }

        private string Password { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();

        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();
        private LoginInfo user;

        public  void CreateSmtpWindowAndConnectToServer(string userName, string password)
        {
            _smtpWindow = new SMTPWindow();
            user = new LoginInfo();
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
           // BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, Messages[0].TextHTML); ;
        }


        public  Main()
        {
            InitializeComponent();
            comandsForAddingToTheServer = new ImprovedDictionary<string, string>();
            comandsForDeletingToTheServer=new ImprovedDictionary<string, string>();
            CreateSmtpWindowAndConnectToServer("login", "password");
        }


        private void ButtonForSendMessage_Click(object sender, RoutedEventArgs e)
        {           
            _smtpWindow=new SMTPWindow();
            _smtpWindow.Login = Login;
            _smtpWindow.Password = Password;
            _smtpWindow.Show();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _imap.GetUids();
            
            Messages.Clear();
            _imap.Logout();
            _imap.Connect(user);
            var col = _imap.UpdateListMessages();
            foreach (var email in col)
            {
                email.TextHTML = "<!DOCTYPE HTML><html><head><meta http-equiv = 'Content-Type' content = 'text/html;charset=UTF-8'></head><body>" + email.TextHTML + "</body></html>";
                Messages.Add(email);               
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
                message.Select = true;
            }
            ShowMessagesDataGrid.ItemsSource = Messages;

        }

        private void checkBoxInColumnCircle_Click(object sender, RoutedEventArgs e)
        {

            var element = ShowMessagesDataGrid.CurrentItem;
            if (element is MessageModel)
            {
               // _imap.RemoveMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
                ((MessageModel) element).RemoveFlag("\\Seen");
                ((MessageModel)element).Color = "Aqua";
                comandsForDeletingToTheServer.Add(((MessageModel)element).Uid, "\\Seen");
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
                //_imap.AddMessageFlags(((MessageModel)element).Uid, ImapMessageFlags.Seen);
                ((MessageModel)element).Color = "White";
                ((MessageModel)element).AddFlag("\\Seen");
                comandsForAddingToTheServer.Add(((MessageModel)element).Uid, "\\Seen");
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
                    comandsForDeletingToTheServer.Add(item.Uid, "\\Seen");
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
                    comandsForAddingToTheServer.Add(message.Uid, "\\Deleted");
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

        private void SendToServerCommands()
        {
            foreach (var command in comandsForAddingToTheServer)
            {
                foreach (var value in command.Value)
                {
                    _imap.ModificateMessageFlagOnTheServer(command.Key, value,'+');
                }       
            }
            foreach (var command in comandsForDeletingToTheServer)
            {
                foreach (var value in command.Value)
                {
                    _imap.ModificateMessageFlagOnTheServer(command.Key, value, '-');
                }
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
           SendToServerCommands();
        }
    }
}