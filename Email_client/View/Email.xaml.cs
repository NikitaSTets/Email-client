using System;
using System.Collections.Generic;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Email_client.View
{

    public partial class Email : Window
    {
        ImapClient imap;
        IList<CheckBox> checkBoxs = new List<CheckBox>();
        public  ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        private void ConnectToServer(string userName,string password)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            imap = new ImapClient("imap.gmail.com");
            
                // Connect to mail server
                imap.Connect();
                imap.Authenticate(userName,password);
                imap.SelectInbox();
            PrintToGreyNewMessages();          
        }
        private void PrintToGreyNewMessages()
        {

            IList<ImapMessageInfo> messagesInfo = imap.ListMessages();
            bool isUnread;
            IList<string> listOfUidUnreadMessages = new List<string>();
            foreach (var item in messagesInfo)
            {
                isUnread = true;
                foreach (var item1 in item.Flags)
                {
                    if (item1 == "\\Seen")
                    {
                        isUnread = false;
                    }
                }
                if (!isUnread)
                {
                    listOfUidUnreadMessages.Add(item.Uid);

                 //   imap.AddMessageFlags(item.Uid, ImapMessageFlags.);
                }
              
            }
            foreach (var message in Messages)
            {
                foreach (var unreadMessage in  listOfUidUnreadMessages)
                {
                    //if (messag==)
                    //{ }
                }
            } 

        }
        private void UpdateListOfMessages()
        {
            Messages.Clear();
            IList<ImapMessageInfo> messageInfoCollection = imap.ListMessages();
            MailMessage currentMessage;
            string head = "<head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head>";
            string text;
            for (int i = 0; i < messageInfoCollection.Count; i++)
            {
                currentMessage = imap.GetMessage(messageInfoCollection[i].Uid);
                text = currentMessage.BodyHtml;
                if (text == null)
                {
                    text = currentMessage.BodyText;
                }
                currentMessage.BodyHtml = head + text;
                if (currentMessage.BodyHtml != null)
                {
                    Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyHtml));
                }
                else
                {
                    Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyText));
                }
            }
        }
        public Email()
        {
            InitializeComponent();
            DataContext = this;
            ConnectToServer("nikitstets@gmail.com", "StackCorporation");
            UpdateListOfMessages();
        }     
        private void flagTextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }      
        private void WebBrowser_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
              
                IList<ImapMessageInfo> messagesInfo = imap.ListMessages();                           
                var selectedMessages = listBoxOfMessages.SelectedItems;
                var arrayMessageForDelete = new List<int>();
                foreach (MessageModel message in selectedMessages)
                {
                    for (int i = 0; i < Messages.Count; i++)
                    {
                        if (Messages[i].Author == message.Author && Messages[i].DateTime == message.DateTime && Messages[i].Text == message.Text)
                        {
                            imap.DeleteMessage(messagesInfo[i].Uid, true);
                            arrayMessageForDelete.Add(i);
                        }
                    }
                }
            UpdateListOfMessages();
            }  
         //unread
        private void CircleCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var container = FindParentOfType<ListBoxItem>(checkBox);
            if (container != null)
                container.IsSelected = checkBox.IsChecked.Value;
            var listMessages = imap.ListMessages();//make more global

            for (int i = 0; i < listMessages.Count; i++)//Не все!!!!
            {
                imap.AddMessageFlags(listMessages[i].Uid, ImapMessageFlags.Seen);
            }
        }
        private void FlagCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            //v 
        }
        private void SelectMessage_Click(object sender, RoutedEventArgs e)
        {       
            var checkBox = (CheckBox)sender;
            var container = FindParentOfType<ListBoxItem>(checkBox);
            if(container!=null)
            container.IsSelected = checkBox.IsChecked.Value;
        }
        static private T FindParentOfType<T>(FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {

                if (element is T)
                {
                    return (T)element;
                }
                element = (FrameworkElement)VisualTreeHelper.GetParent(element);
            }
            return null;
        }
        static private T FindChildOfType<T>(FrameworkElement element) where T : FrameworkElement
        {
            
            while (element != null)
            {

                if (element is T)
                {
                    return (T)element;
                }
                element = (FrameworkElement)VisualTreeHelper.GetChild(element,0);
            }
            return null;
        }
        static public void EnumVisual(Visual myVisual)
        {
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                // Retrieve child visual at specified index value.
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);

                if (childVisual is CheckBox)
                {
                    ((CheckBox)childVisual).IsChecked = false;
                }
                // Do processing of the child visual object.

                // Enumerate children of the child visual object.
                EnumVisual(childVisual);
            }
            
        }
        private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {

                // ListBox item clicked - do some cool things here
               
             //  EnumVisual(item);
                
                
               // VisualTreeHelper.GetChild(sender);
            }
        }    
    }
    
    
}
