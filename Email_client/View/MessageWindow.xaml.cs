using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Email_client.Model;

namespace Email_client.View
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        

        public void SetHTML(string html)
        {
            BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, html);
        }
        public MessageWindow()
        {
            InitializeComponent();
          //  BrowserBehavior.SetBody(WebBrowserForShowingCurrentMessage, HTML);
        }
    }
}
