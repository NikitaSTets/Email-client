using System;


namespace Email_client.Model
{
    public class MessageModel
    {
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public string Uid { get; }
        public MessageModel(string author,DateTime date,string text,string Uid)
        {
            Author = author;
            DateTime = date;
            Text = text;
            this.Uid = Uid;
            Color = "White";
        }
    }
}
