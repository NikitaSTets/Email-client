using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_client.Model
{
   public  class MessageModel
    {
       public  string  Author { get; set; }
       public DateTime DateTime { get; set; }
       public string Text { get; set; }
       public MessageModel(string author,DateTime date,string text)
        {
            Author = author;
            DateTime = date;
            Text = text;
        }
    }
}
