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
       // override object.Equals
        public override bool Equals(object obj)
        {
            obj = obj as MessageModel;
            if (obj == null)
                return false;
            return Author == ((MessageModel)obj).Author && ((MessageModel)obj).DateTime == DateTime && ((MessageModel)obj).Text == Text;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            //throw new NotImplementedException();
            return base.GetHashCode();
        }
        public MessageModel(string author,DateTime date,string text)
        {
            Author = author;
            DateTime = date;
            Text = text;
        }
    }
}
