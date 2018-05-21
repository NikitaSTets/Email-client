using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using  System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Email_client.IMap;
using Email_client.Model;

namespace Email_client.View
{
    public partial class Main : Window
    {

        readonly ImprovedDictionary<string, string> _comandsForAddingToTheServer;
        readonly ImprovedDictionary<string, string> _comandsForDeletingToTheServer;

        ImapControl _imap;

        SmtpWindow _smtpWindow;

        private string Login { get; set; }
        private string Password { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> MessagesTemp { get; set; } = new ObservableCollection<MessageModel>();

        private LoginInfo _user;

        Timer _timer;

        private TimerCallback _tm;


        public Main()
        {
            InitializeComponent();
            _comandsForAddingToTheServer = new ImprovedDictionary<string, string>();
            _comandsForDeletingToTheServer = new ImprovedDictionary<string, string>();
            CreateSmtpWindowAndConnectToServer("login", "password");
            int num = 0;
            _tm = new TimerCallback(SendToServerCommands);
            _timer = new Timer(_tm, num, 60000, 60000);
        }


        public void CreateSmtpWindowAndConnectToServer(string userName, string password)
        {
            _smtpWindow = new SmtpWindow();
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
            ShowMessagesDataGrid.ItemsSource = Messages.OrderByDescending(m => m.Unread);
            SetCheckedForUnreadMessage();
        }


        private void ButtonForSendMessage_Click(object sender, RoutedEventArgs e)
        {
            _smtpWindow = new SmtpWindow();
            _smtpWindow.Login = Login;
            _smtpWindow.Password = Password;
            _smtpWindow.Show();
        }

        ObservableCollection<MessageModel> UpdateMessages()
        {
            var message = new ObservableCollection<MessageModel>();
            _imap.GetUids();

            _imap.Logout();
            _imap.Connect(_user);

            return _imap.UpdateListMessages();;
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            var newMessages = await Task.Run(() => UpdateMessages());
            var messages = new ObservableCollection<MessageModel>();
            foreach (var message in newMessages)
            {
                messages.Add(new MessageModel(message.Author, message.DateTime, message.TextHtml, message.Text, message.Uid, message.Flags));
            }

            Messages = messages;
            ShowMessagesDataGrid.ItemsSource = Messages.OrderByDescending(s => s.Unread);
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
                if (!message.HasFlag("\\Deleted"))
                    MessagesTemp.Add(message);
                message.Select = true;
            }
            ShowMessagesDataGrid.ItemsSource = Messages.OrderByDescending(m => m.Unread);

        }

        private void checkBoxInColumnCircle_Click(object sender, RoutedEventArgs e)
        {

            var element = ShowMessagesDataGrid.CurrentItem;
            if (element is MessageModel)
            {
                ((MessageModel)element).RemoveFlag("\\Seen");
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
                    if (!_comandsForAddingToTheServer.Remove(item.Uid, "\\Seen"))
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
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
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
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
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
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
        }

        private void Incoming_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (!message.HasFlag("\\Deleted"))
                    MessagesTemp.Add(message);
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
        }

        private void DeleteMessagesLabel_OnMouseDown(object sender, MouseButtonEventArgs e)//добавить флаг,что удалено,а потом отправлять запросы на сервер,что удалено и не отображать сообщения с флагом deleted
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (message.Select)
                {
                    message.AddFlag("\\Deleted");
                    if (!_comandsForDeletingToTheServer.Remove("\\Deleted"))
                        _comandsForAddingToTheServer.Add(message.Uid, "\\Deleted");
                    continue;
                }
                MessagesTemp.Add(message);
            }
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
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
            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
        }

        private void DataGridLabelShowCurrentMessageMessage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var content = ((Label)sender).Content;
            foreach (var message in Messages)
            {
                if (content == message.Text)
                {
                    BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, message.TextHtml);
                    break;
                }
            }
        }

        private void DraftedMessagesLabel_OnMouse(object sender, MouseButtonEventArgs e)
        {
            MessagesTemp.Clear();
            foreach (var message in Messages)
            {
                if (message.HasFlag("\\Drafted"))
                    MessagesTemp.Add(message);
            }

            ShowMessagesDataGrid.ItemsSource = MessagesTemp.OrderByDescending(m => m.Unread);
        }
       
        private async void SendToServerCommands(object obj)
        {
            var commandForDeleting = _comandsForDeletingToTheServer.GetCopy();
            var commandForAdding = _comandsForAddingToTheServer.GetCopy();
            _comandsForAddingToTheServer.RemoveAll();
            _comandsForDeletingToTheServer.RemoveAll();
            await Task.Run(() => {
                foreach (var key in commandForAdding.Keys)
            {
                foreach (var value in commandForAdding[key])
                {
                    _imap.ModificateMessageFlagOnTheServer(key, value, '+');                     
                }
            }
            foreach (var key in commandForDeleting.Keys)
            {
                foreach (var value in commandForDeleting[key])
                {
                    _imap.ModificateMessageFlagOnTheServer(key, value, '-'); 
                }
            }
            });
        }

    }
}
