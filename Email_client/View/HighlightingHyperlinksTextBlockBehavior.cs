using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;

 namespace Email_client.View
{
    public class HighlightingHyperlinksTextBlockBehavior : Behavior<TextBlock>
    {
        public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register(nameof(Text), typeof(string),
          typeof(HighlightingHyperlinksTextBlockBehavior), new PropertyMetadata("", TextPropertyChangedCallback));
        // Это свойство зависимости, в которое будем помещать входную строку
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Этот метод будет вызываться при изменении свойства Text
        static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (HighlightingHyperlinksTextBlockBehavior)d;
            var text = (string)e.NewValue ?? "";
            if (behavior.AssociatedObject == null) return;
            // Преобразуем символы \n\r в реальные переводы строк
            text = Regex.Unescape(text);
            // Шаблон для Url
            var pattern = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
            // Ищем все Url во входной строке и собираем из них Hyperlink'и
            var hlinkParts =
                Regex.Matches(text, pattern)
                     .OfType<Match>()
                     .Select(m => m.Value)
                     .Select(s => new Hyperlink(new Run(s)))
                     .ToArray();
            // Остальные куски текста (которые не входят в Url) просто помещаем в Run'ы
            var runParts =
                Regex.Split(text, pattern, RegexOptions.ExplicitCapture)
                     .Select(s => new Run(s))
                     .ToArray();
            // Очищаем у связанного TextBox коллекцию Inline'ов
            behavior.AssociatedObject.Inlines.Clear();
            for (int i = 0; i < hlinkParts.Length + runParts.Length; ++i)
                // И добавляем в нее по очереди Run и Hyperlink
                behavior.AssociatedObject.Inlines.Add(i % 2 == 0 ? runParts[i / 2] : (Inline)hlinkParts[i / 2]);
        }
    }
}
