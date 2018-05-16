﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP
{
   public class EmailTemplate
    {
        public List<string> Flags;

        public bool AddFlag(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                return false;
            if(Flags==null)
                Flags=new List<string>();
            Flags.Add(flag);
            return true;
        }

        public string TextHTML { get; set; }
        public string Mailbox { get; set; }
        public string Uid { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return From +" " +Body;
        }
    }
}
