using System.Windows;
using System.Windows.Controls;

namespace Email_client.Model
{
    public static class BrowserBehavior
    {

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.RegisterAttached("Body", typeof(string), typeof(BrowserBehavior),
                new PropertyMetadata(OnChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(BodyProperty, body);
        }

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((WebBrowser)d).NavigateToString((string)e.NewValue);

    }
}
