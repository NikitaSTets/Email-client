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

        readonly ImprovedDictionary<string, string> _comandsForAddingToTheServer;
        readonly ImprovedDictionary<string, string> _comandsForDeletingToTheServer;

        ImapControl _imap;

        SMTPWindow _smtpWindow;

        private string Login { get; set; }
        private string Password { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();

        private LoginInfo _user;

        public  void CreateSmtpWindowAndConnectToServer(string userName, string password)
        {
            _smtpWindow = new SMTPWindow();
            _user = new LoginInfo();
            _user.ImapAddress = "imap.gmail.com";
            _user.Username = userName;
            _user.Password = password;
            Login = userName;
            Password = password;
            _imap = new ImapControl(993);
            try
            {
                _imap.Connect(_user);

            }
            catch (Exception)
            {
                MessageBox.Show("Error.Connect to server");
            }
            ViewModel.ViewModel.UpdateListOfMessages(Messages, _imap);
            ShowMessagesDataGrid.ItemsSource = Messages;
            SetCheckedForUnreadMessage();
           // BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, Messages[0].TextHTML); ;
        }

        
        public  Main()
        {
            InitializeComponent();
            _comandsForAddingToTheServer = new ImprovedDictionary<string, string>();
            _comandsForDeletingToTheServer=new ImprovedDictionary<string, string>();
            CreateSmtpWindowAndConnectToServer("nikit.stets@gmail.com", "Minsk1.1.ru");
        }


        public void SetCheckedForUnreadMessage()
      {
            for (int i = 0; i <Messages.Count; i++)
            {
                if (!Messages[i].HasFlag("\\Seen"))
                {
                   // ShowMessagesDataGrid.Columns[1].
                   // var findName = ShowMessagesDataGrid.FindName("checkBoxInColumnCircle");
                   // ((CheckBox) findName).IsChecked = true;
                }
            }
          
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
            _imap.Connect(_user);
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
                if (!_comandsForAddingToTheServer.Remove(((MessageModel)element).Uid, "\\Seen"))
                    _comandsForDeletingToTheServer.Add(((MessageModel)element).Uid, "\\Seen");
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
                if (!_comandsForDeletingToTheServer.Remove(((MessageModel)element).Uid, "\\Seen"))               
                     _comandsForAddingToTheServer.Add(((MessageModel)element).Uid, "\\Seen");
                ((MessageModel)element).AddFlag("\\Seen");
               
            }
        }

        private void UnreadMessagesLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedMessages = ShowMessagesDataGrid.Items;
            foreach (MessageModel item in selectedMessages)
            {
                if (item.Select == true)
                {                    
                   if( !_comandsForAddingToTheServer.Remove(item.Uid, "\\Seen"))               
                       _comandsForDeletingToTheServer.Add(item.Uid, "\\Seen");
                   
                    item.RemoveFlag("\\Seen");
                }
            }
        }
        private void ReadMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedMessages = ShowMessagesDataGrid.Items;
            foreach (MessageModel item in selectedMessages)
            {
                if (item.Select == true)
                {
                    if (!_comandsForDeletingToTheServer.Remove(item.Uid, "\\Seen"))
                        _comandsForAddingToTheServer.Add(item.Uid, "\\Seen");
                    item.AddFlag("\\Seen");
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
                    _comandsForAddingToTheServer.Add(message.Uid, "\\Deleted");
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
            foreach (var command in _comandsForAddingToTheServer)
            {
                foreach (var value in command.Value)
                {
                    _imap.ModificateMessageFlagOnTheServer(command.Key, value,'+');
                }       
            }
            foreach (var command in _comandsForDeletingToTheServer)
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