using System;
using System.Collections.Generic;
using System.Windows;
//using GemBox.Email;
//using GemBox.Email.Imap;
using System.Collections.ObjectModel;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using IMAP;

namespace Email_client.View
{

    public partial class Email : Window
    {
        ImapControl imap;
        LoginInfo user = new LoginInfo();
        IList<CheckBox> checkBoxs = new List<CheckBox>();
        public  ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        private void ConnectToServer(string userName,string password)
        {
            user.ImapAddress = "imap.gmail.com";
            user.Username = "nikit.stets@gmail.com";//=userName;
            user.Password = "StackCorporation";//=password;
            imap = new ImapControl(993);
            imap.Connect(user);
        }
        private void SetGreyColor()
        {
            IList<EmailTemplate> messagesInfo = imap.ListMessages();
            bool isUnread;
            int i = 0;
            IList<string> listOfUidUnreadMessages = new List<string>();
            foreach (var item in messagesInfo)
            {
                isUnread = true;
                foreach (var item1 in item.Flags)
                {
                    if (item1 == "\\Seen")
                    {
                        isUnread = false;
                        break;
                    }
                }
                if (isUnread)
                {
                    Messages[i].Color = "Aqua";               
                }
                i++;
            }

        }
        private void UpdateListOfMessages()
        {

            Messages.Clear();
            IList<EmailTemplate> messageInfoCollection = imap.ListMessages();
            EmailTemplate currentMessage;
            string text;
            for (int i = 0; i < messageInfoCollection.Count; i++)
            {
                currentMessage = messageInfoCollection[i];
                text = currentMessage.Body;
                Messages.Add(new MessageModel(currentMessage.From,DateTime.Now,text,text,currentMessage.Uid,currentMessage.Flags));
            }
        }
        public Email()
        {

            InitializeComponent();
            DataContext = this;
            ConnectToServer("userName", "password");
            UpdateListOfMessages();
            SetGreyColor();
        }     
        private void flagTextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }      
        private void WebBrowser_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {             
                IList<EmailTemplate> messagesInfo = imap.ListMessages();                           
                var selectedMessages = listBoxOfMessages.SelectedItems;
                var arrayMessageForDelete = new List<int>();
                foreach (MessageModel message in selectedMessages)
                {
                    for (int i = 0; i < Messages.Count; i++)
                    {
                        if (Messages[i].Author == message.Author && Messages[i].DateTime == message.DateTime && Messages[i].Text == message.Text)
                        {
                            imap.DeleteMessage(messagesInfo[i].Uid);
                            arrayMessageForDelete.Add(i);
                        }
                    }
                }
            UpdateListOfMessages();
            }  
        private void CircleCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var container = FindParentOfType<ListBoxItem>(checkBox);


            var messages=listBoxOfMessages.SelectedItems;
            if (container != null)
            {
                int i = 0;
                while (i<messages.Count)
                {
                    imap.RemoveMessageFlags(((MessageModel)messages[i]).Uid, ImapMessageFlags.Seen);
                    i++;
                }
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

            }
        }    
    }
    
    
}
