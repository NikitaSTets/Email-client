using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_client.Model
{
    public class MessageModel
    {
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        //// override object.Equals
        //public override bool Equals(object obj)
        //{
        //    var message = (MessageModel)obj;
        //    if (message == null || GetType() != obj.GetType())
        //    {
        //        return false;
        //    }           
        //    return Author==message.Author && DateTime==message.DateTime && Text==message.Text;
        //}

        //// override object.GetHashCode
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        public MessageModel(string author,DateTime date,string text)
        {
            Author = author;
            DateTime = date;
            Text = text;
            Color = "Aqua";
        }
    }
}
