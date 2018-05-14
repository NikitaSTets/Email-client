using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP
{
   public class LoginInfo
    {
        public LoginInfo()
        {
                
        }
        
        public string ImapAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
