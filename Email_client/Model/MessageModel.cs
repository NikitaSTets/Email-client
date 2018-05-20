using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows;

namespace Email_client.Model
{
    public class MessageModel:DependencyObject
    {
        private List<string> _flags;
        public List<string> Flags
        {
            get { return _flags;}
            set
            {
                _flags=value;
                if(!HasFlag("\\Seen"))
                    {
                        Color = "Aqua";
                        Unread = true;
                    }
                else
                {
                    Color = "White";
                    Unread = false;
                }
            }

        }

        public bool AddFlag(string flag)
        {
            if (flag == "\\Seen")
            {           
               Color = "White";
               Unread = false;
            }
            if(!HasFlag(flag))
            Flags.Add(flag);
            return true;
        }

        public bool HasFlag(string flag)
        {
            return Flags.Contains(flag);
        }

        public bool RemoveFlag(string flag)
        {
            if (flag == "\\Seen")
            {          
               Color = "Aqua";
               Unread = true;
            }
            return  Flags.Remove(flag);
        }

        public string Subject { get; set; }       
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public string TextHTML { get; set; }
        
        public  bool Unread { get;set;}
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

        public string To { get; internal set; }

        public static readonly DependencyProperty SelectProperty =
            DependencyProperty.Register("Select", typeof(bool), typeof(MessageModel), new UIPropertyMetadata(false));

        public MessageModel(string author,DateTime date,string textHTML,string text,string Uid,List<string> flags)
        {
            Author = author;
            DateTime = date;
            Unread = true;
            TextHTML = textHTML;
            Text = text;
            this.Uid = Uid;
            Color = "White";
            Select = false;
            Flags = flags;
        }

        public MessageModel(string uid)
        {
            Uid = uid;
            Flags = new List<string>();
        }
    }
}
