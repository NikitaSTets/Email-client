using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Email_client.View
{
    public static class WebBrowserExtentions
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.RegisterAttached("Document", typeof(string), typeof(WebBrowserExtentions), new UIPropertyMetadata(null, DocumentPropertyChanged));

        public static string GetDocument(DependencyObject element)
        {
            return (string)element.GetValue(DocumentProperty);
        }

        public static void SetDocument(DependencyObject element, string value)
        {
            element.SetValue(DocumentProperty, value);
        }

        public static void DocumentPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = target as WebBrowser;
            if (browser != null)
            {
                string document = e.NewValue as string;
                if(document!=null)
                browser.NavigateToString(document);
            }
        }
    }
}
