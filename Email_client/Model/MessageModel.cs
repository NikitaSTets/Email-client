using System;
using System.Collections.Generic;
using System.Windows;

namespace Email_client.Model
{
    public class MessageModel:DependencyObject
    {
        public List<string> Flags { get; set;}

        public bool AddFlag(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                return false;
            if (Flags == null)
                Flags = new List<string>();
            Flags.Add(flag);
            return true;
        }

        public bool HasFlag(string flag)
        {
            return Flags.Contains(flag);
        }

        public bool RemoveFlag(string flag)
        {
          return  Flags.Remove(flag);
        }

        public string Subject { get; set; }       
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public string TextHTML { get; set; }
       // public string Color { get; set; }
        public string Uid { get; }
        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(MessageModel), new UIPropertyMetadata(""));
        public bool Select
        {
            get { return (bool)GetValue(SelectProperty); }
            set { SetValue(SelectProperty, value); }
        }
        public static readonly DependencyProperty SelectProperty =
            DependencyProperty.Register("Select", typeof(bool), typeof(MessageModel), new UIPropertyMetadata(false));
        // public bool Select { get; set; }
        public MessageModel(string author,DateTime date,string textHTML,string text,string Uid,List<string> flags)
        {
            Author = author;
            DateTime = date;
            TextHTML = textHTML;
            Text = text;
            this.Uid = Uid;
            Color = "White";
            Select = false;
            Flags = flags;
        }
    }
}
