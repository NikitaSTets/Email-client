using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using GemBox.Email;
using GemBox.Email.Imap;
//using MailKit.Net.Imap;
//using AE.Net.Mail;
using GemBox.Email.Mime;
using GemBox.Email.Security;
using System.Collections.ObjectModel;
using MailBee.ImapMail;
using Email_client.Model;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Text;

namespace Email_client.View
{
   
    public partial class Email : Window
    {

      public  ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public Email()
        {
            InitializeComponent();
            Messages.Add(new MessageModel("User 1", DateTime.Now, "Hello world"));
            Messages.Add(new MessageModel("User 2", DateTime.Now, "I want to eat"));
            DataContext = this;

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            using (ImapClient imap = new ImapClient("imap.gmail.com"))
            {
                // Connect to mail server
                imap.Connect();

                imap.Authenticate("nikitstets@gmail.com", "StackCorporation");
                imap.SelectInbox();
                IList<ImapMessageInfo> a=imap.ListMessages();

                MailMessage currentMessage;
               
                for (int i = 0; i < a.Count; i++)
                {
                    currentMessage=imap.GetMessage(a[i].Uid);
                    Messages.Add(new MessageModel(currentMessage.From[0].User, currentMessage.Date, currentMessage.BodyText));
                }
                //imap.GetMessage(2);
                //MailMessage message = imap.GetMessage(1);
                //Messages.Add(new MessageModel(message.From[0].ToString(), message.Date, message.BodyText));
                //listBox.Items.Add(message.BodyText);

            }
            
        }


        private void flagTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
        
            Image imgMessage = new Image();
            imgMessage.Visibility = Visibility.Collapsed;
            BitmapImage bi = new BitmapImage();
            //"C:\\Users\\Никита\\Documents\\Visual Studio 2015\\Projects\\Email_client\\Email_client\\bin\\Debug\\Email_client.exe"
             var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
             var foldersProjectToEXE = location.Split('\\');
            StringBuilder  pathToImage=new StringBuilder();
            for (int i = 0; i < foldersProjectToEXE.Length-3; i++)
            {
                pathToImage.Append(foldersProjectToEXE[i]+"\\"+"\\");
            }
            pathToImage.Append("flag(blue).png");
            bi.BeginInit();
            bi.UriSource = new Uri(@pathToImage.ToString(), UriKind.Absolute);
            bi.EndInit();

            imgMessage.Source = bi;


            TextBlock tb = sender as TextBlock;
            InlineUIContainer iuc = new InlineUIContainer(imgMessage);
            tb.Inlines.Add(iuc);
        
        }


    }
}
