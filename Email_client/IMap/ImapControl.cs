using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Email_client.View;

namespace IMAP
{

    public class ImapControl
    {
        readonly int _port;

        private TcpClient _tcpClient = null;
        private SslStream _ssl = null;

        private StreamReader _sr = null;
        private StreamWriter _sw = null;

        private int bytes = -1;
        private byte[] _buffer;
        private StringBuilder _sb;
        private byte[] _dummy;
        private List<string> uids;
        private List<EmailTemplate> emails;

        public ImapControl(int port)
        {         
            this._port = port;
        }

        public List<EmailTemplate> UpdateListMessages()
        {
            emails.Clear();
            uids = GetUids();
            foreach (var uid in uids)
            {
                string body = ReceiveResponse("$ FETCH " + uid + " body.peek[header]\r\n").Replace("\0", "");
                string header = body;
                if (body == "$ OK Success\r\n" || body == string.Format(" * {0} FETCH(BODY[TEXT] ", uid))
                {
                    body = ReceiveResponse("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                    header = body;
                }
                EmailTemplate email = new EmailTemplate();
                email = GetEmailTemplate(body);
                email.Body = GetBody2(uid);
                email.Flags = GetMessageFlags(uid);
                emails.Add(email);

            }

            return emails;
        }

        public List<EmailTemplate> ListMessages()
        {
            if (emails != null)
                return emails;
            emails = new List<EmailTemplate>();
           // uids = GetUids();
            int i = 0;
            foreach (var uid in uids)
            {
                string body = ReceiveResponse("$ FETCH " + uid + " body.peek[header]\r\n").Replace("\0", "");
                string header = body;
                if (body == "$ OK Success\r\n" || body == string.Format(" * {0} FETCH(BODY[TEXT] ", uid))
                {
                    body = ReceiveResponse("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                    header = body;
                }
                EmailTemplate email = new EmailTemplate();
                email = GetEmailTemplate(body);
                email.TextHTML = GetBodyHTML(uid);
                email.Body = GetBody2(uid);
                email.Flags =GetMessageFlags(uid);
                emails.Add(email);

            }
            return emails;
        }

        public string GetBodyHTML(string uid)
        {
            string body = ReceiveResponse("$ FETCH " + uid + " body.peek[text]\r\n").Replace("\0", "");
            string header = body;
            if (body == "$ OK Success\r\n")
            {
                body = ReceiveResponse("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                header = body;
            }


            Regex regexEncoding = new Regex(@"(?<=\r\nContent-Transfer-Encoding: )([\s\S]*?)(?=(\r\n))");
            Regex regexType = new Regex(@"(?<=\r\nContent-Type: )(.*?)(?=(;))");

            var typeMatch = regexType.Match(header).Value;

            string encoding = "utf-8";

            if (typeMatch == "text/html" || typeMatch == "text/plain")
            {
                var div = body.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                    .Where(n => !n.StartsWith("* ") && !n.StartsWith("$ ") && !(n == ")"));


                encoding = regexEncoding.Match(header).Value;
            }

            int index = body.IndexOf("Content-Type: text/html");
            if (index == -1)
            {
                index = body.IndexOf("Content-Type: text/plain");
            }

            int startIndex;
            try
            {
                startIndex = body.IndexOf("\r\n", index);
            }
            catch (Exception e)
            {
                return "empty";
            }
            //int startIndex = body.IndexOf("\r\n", index);
            int endIndex = body.IndexOf("\r\n--", startIndex);
            var sbstr = body.Substring(startIndex, endIndex - startIndex);
            encoding = regexEncoding.Match(sbstr).Value;
            sbstr = sbstr.Substring(sbstr.IndexOf("\r\n", 2));
            body = sbstr.Substring(sbstr.IndexOf("\r\n") + "\r\n".Length);
            return DecodeBody(body, encoding, "utf-8");
        }


        public bool Connect(LoginInfo user)
        {
            try
            {
                _tcpClient = new TcpClient(user.ImapAddress, _port);
                _ssl = new SslStream(_tcpClient.GetStream());
                _ssl.AuthenticateAsClient(user.ImapAddress);
                string login = ReceiveResponse("$ LOGIN " + user.Username + " " + user.Password + "  \r\n");
            
                if (!login.ToLower().Contains("ok"))
                {
                    return false;
                }
                ReceiveResponse("$ SELECT INBOX\r\n");
                ReceiveResponse("$ STATUS Inbox (MESSAGES)\r\n");
                uids=GetUids();
                emails=ListMessages();
                if (_tcpClient.Connected)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }
        internal EmailTemplate GetMessage(string uid)
        {
            foreach (var email in emails)
            {
                if (email.Uid == uid)
                    return email;
            }

            return null;
        }
        internal EmailTemplate GetMessageFromServer(string uid)
        {
            string text = GetBody2(uid);//у существующего на компе сообщения
            EmailTemplate email = GetEmailTemplate(text);
            email.Uid = uid;
            email.Flags = GetMessageFlags(uid);
            return email;
        }

       
        public List<string> GetMessageFlags(string Uid)
        {
            List<string> answer = new List<string>();

            var flags = ReceiveResponse("$ FETCH " + Uid + " (FLAGS)\r\n");//получать флаги у уже существующих на компе соббщений,т.к запрос=время

            Regex regex = new Regex(@"\\(\w*)");

            var separatedFlags = regex.Matches(flags);

            if (separatedFlags.Count > 0)
            {
                foreach (Match flag in separatedFlags)
                    answer.Add(flag.Value);
                //Console.WriteLine(flag.Value);
            }
            return answer;
        }
        internal void AddMessageFlags(string Uid, ImapMessageFlags flag)//Доработать
        {
            switch (flag)
            {
                case ImapMessageFlags.Seen: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\SEEN)\r\n"); break;
                case ImapMessageFlags.Unseen: ReceiveResponse("$ STORE " + Uid + " -FLAGS (\\SEEN)\r\n"); break;
                case ImapMessageFlags.Answered: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\Answered)\r\n"); break;
                case ImapMessageFlags.Deleted: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\Deleted)\r\n"); break;
                case ImapMessageFlags.Draft: ReceiveResponse("$ STORE " + Uid + " -FLAGS (\\Draft)\r\n"); break;
                default: return;
            }
            ReceiveResponse("$ EXPUNGE" + "\r\n");
        }
 
        public string GetBody2(string uid)
        {
            string body = ReceiveResponse("$ FETCH " + uid + " body.peek[text]\r\n").Replace("\0", "");
            string header = body;
            if (body == "$ OK Success\r\n")
            {
                body = ReceiveResponse("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                header = body;
            }


            Regex regexEncoding = new Regex(@"(?<=\r\nContent-Transfer-Encoding: )([\s\S]*?)(?=(\r\n))");
            Regex regexType = new Regex(@"(?<=\r\nContent-Type: )(.*?)(?=(;))");

            var typeMatch = regexType.Match(header).Value;

            string encoding = "utf-8";

            if (typeMatch == "text/html" || typeMatch == "text/plain")
            {
                var div = body.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //.Where(n => !n.StartsWith("* ")/* && !n.StartsWith("$ ") && !(n == ")")*/);

                string answer = string.Empty;
                answer = div[4];
                if (string.IsNullOrEmpty(answer))
                {
                    answer = div[5];
                    answer = DecodeBody(answer, "base64", "utf-8");
                }
                encoding = regexEncoding.Match(header).Value;
                return answer;
            }

            int index = body.IndexOf("Content-Type: text/html");
            if (index == -1)
            {
                index = body.IndexOf("Content-Type: text/plain");
            }

            int startIndex;
            try
            {
                startIndex = body.IndexOf("\r\n", index);
            }
            catch (Exception e)
            {
                return "empty";
            }
            //int startIndex = body.IndexOf("\r\n", index);
            int endIndex = body.IndexOf("\r\n--", startIndex);
            var sbstr = body.Substring(startIndex, endIndex - startIndex);
            encoding = regexEncoding.Match(sbstr).Value;
            sbstr = sbstr.Substring(sbstr.IndexOf("\r\n", 2));
            body = sbstr.Substring(sbstr.IndexOf("\r\n") + "\r\n".Length);
            return DecodeBody(body, encoding, "utf-8");
        }
        internal void RemoveMessageFlags(string Uid, ImapMessageFlags flag)
        {
            switch (flag)
            {
                case ImapMessageFlags.Seen: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\SEEN)\r\n"); break;
                case ImapMessageFlags.Unseen: ReceiveResponse("$ STORE " + Uid + " -FLAGS (\\SEEN)\r\n"); break;
                case ImapMessageFlags.Answered: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\Answered)\r\n"); break;
                case ImapMessageFlags.Deleted: ReceiveResponse("$ STORE " + Uid + " +FLAGS (\\Deleted)\r\n"); break;
                case ImapMessageFlags.Draft: ReceiveResponse("$ STORE " + Uid + " -FLAGS (\\Draft)\r\n"); break;
                default: return;
            }
            ReceiveResponse("$ EXPUNGE" + "\r\n");

        }

        private static string DecodeBody(string body, string encoding, string charset)
        {
            switch (encoding)
            {
                case "quoted-printable":
                    return MessageDecoder.DecodeQP(body, "utf-8");
                case "base64":
                    var bytes = Convert.FromBase64String(body);
                    return Encoding.GetEncoding(charset).GetString(bytes);
                default:
                    return body;
            }
        }
        public void CreateNewFloder(string folderName)
        {
            ReceiveResponse(@"$ CREATE " + folderName + "\r\n");
        }

        public void DeleteFolder(string folderName)   //WARNING dangerous method. Use with caution.
        {
            ReceiveResponse(@"$ DELETE " + folderName + "\r\n");
        }

        public List<string> GetFolderList()
        {

            List<string> foldersList = new List<string>();
            string folders = ReceiveResponse("$ LIST " + "\"\"" + " \"*\"" + "\r\n");
            string[] lines = folders.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string pattern = @"\s([a-zA-Z-\[\]\""]+)$";
            Regex regex = new Regex(pattern);

            foreach (var line in lines)
            {
                if (!line.Contains("\\HasChildren"))
                {
                    var match = regex.Match(line);
                    if (match.Value != string.Empty)
                        foldersList.Add(match.Value.Replace(@"""", "").Remove(0, 1));
                }
            }
            return foldersList;
        }

        public List<string> GetUids()
        {
            uids = new List<string>();
            var uidswithSearch = ReceiveResponse("$ uid search all\r\n");
            Regex regex1 =new Regex(@"(MESSAGES \d*)");
            var  str = regex1.Match(uidswithSearch);
            if (str != null)
            {
                int length = Convert.ToInt32(str.Value.Split(' ')[1]);
                for (int j = 1; j <length+1; j++)
                {
                   

                    this.uids.Add(j.ToString());
                }
        
                    ReceiveResponse("");
                return uids;
            }

            if (string.IsNullOrEmpty(str.Value))
            {


                Regex regex = new Regex(@"( SEARCH )((\d*) )*(\d*)"); //$ uid fetch uid bodystructure body[text]
                string uids = regex.Match(uidswithSearch).Value;
                string[] lines = uids.Split(' ');
                Regex regexNum = new Regex(@"\d+");

                var res = new List<string>();
                int i = 0;
                foreach (var uid in lines)
                {
                    var match = regexNum.Match(uid);
                    if (match.Value != "" && match.Value != "SEARCH")
                    {
                        i++;
                        res.Add(i.ToString());
                    }
                }
            }

            return uids;
        }

        public List<EmailTemplate> SelectFolder(string folderName)
        {
            var uids = GetUids();
            var emails = new List<EmailTemplate>();
            foreach (var uid in uids)
            {
                string text = ReceiveResponse("$ FETCH " + uid + " body.peek[text]\r\n").Replace("\0", "");
                if (text == "$ OK Success\r\n")
                {
                    text = ReceiveResponse("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                }

                //  string text=GetBody2(line);
                EmailTemplate email = GetEmailTemplate(text);
                email.Uid = uid;
                email.Mailbox = folderName;
                email.Flags = GetMessageFlags(uid);
                email.Body = GetBody2(uid); //нужно сразу иметь все uid,body,flags
                emails.Add(email);
                //var message = MimeMessage.Load(new MemoryStream(Encoding.ASCII.GetBytes(text)));
            }

            return emails;

        }

        public EmailTemplate GetEmailTemplate(string text)
        {
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //Console.WriteLine(text);
            Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            EmailTemplate email = new EmailTemplate();
            if (lines.Length > 5)
                email.From = lines[4];
            foreach (var line in lines)
            {
                if (line.StartsWith("From"))
                {


                    var Senders = regex.Matches(line);

                    if (Senders.Count > 0)
                    {
                        foreach (Match Sender in Senders)
                            email.From = Sender.Value;
                        //Console.WriteLine(flag.Value);
                    }

                }
                if (line.StartsWith("To"))
                {
                    email.To = line.Replace("To: ", string.Empty);
                }
                if (line.StartsWith("Subject"))
                {
                    email.Subject = line.Replace("Subject: ", string.Empty);
                }
            }
            //email.From = 


            return email;
        }

      
        public string GetMailCount(string folderName)
        {
            string selectInfo = ReceiveResponse("$ SELECT " + folderName + "\r\n");

            string pattern = @"\d+( EXISTS){1}";
            Regex regex = new Regex(pattern);
            var match = regex.Match(selectInfo);
            return match.Value.Replace("MESSAGES ", string.Empty).Replace(" EXISTS", string.Empty);
        }

        public void Logout()
        {
            ReceiveResponse("$ LOGOUT" + "\r\n");
        }

        public void DeleteMessage(string Uid)
        {

            ReceiveResponse("$ STORE " + Uid + " -FLAGS (\\SEEN)\r\n");
            ReceiveResponse("$ EXPUNGE" + "\r\n");

        }


        public string ReceiveResponse(string command)
        {
            _sb = new StringBuilder();
            try
            {
                if (command != "")
                {
                    if (_tcpClient.Connected)
                    {
                        _dummy = Encoding.ASCII.GetBytes(command);
                        _ssl.Write(_dummy, 0, _dummy.Length);
                    }
                    else
                    {
                        throw new ApplicationException("TCP CONNECTION DISCONNECTED");
                    }
                }
                // _ssl.Flush();

                _buffer = new byte[2048];
                _tcpClient.ReceiveTimeout = 2000000;

                Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                while (true)
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead = _ssl.Read(buffer, 0, 2048);
                    if (bytesRead == 0)
                    {
                        throw new EndOfStreamException("Error while reading");
                    }
                    string str = Encoding.UTF8.GetString(buffer).Replace("\0", string.Empty);
                    _sb.Append(str);
                    if (SearchEndOfMessage(str))
                        break;
                }
                string text = _sb.ToString().Replace("\0", string.Empty);
                //Console.WriteLine(text);

                return _sb.ToString();
                //sw.WriteLine(sb.ToString());

            }
            catch (Exception ex)
            {
                //  throw new ApplicationException(ex.Message);
                return "WAS ERROR";
            }
        }
        private bool SearchEndOfMessage(string str)
        {
            return str.Contains("$ OK")
                   || str.Contains("$ NO")
                   || str.Contains("$ BAD")
                   || str.Contains("Gimap")
                   || str.Contains("* BAD");
        }


        private static string DecodeQuotedPrintable(string input, string bodycharset)
        {
            var i = 0;
            var output = new List<byte>();
            while (i < input.Length)
            {
                if (input[i] == '=' && input[i + 1] == '\r' && input[i + 2] == '\n')
                {
                    //Skip
                    i += 3;
                }
                else if (input[i] == '=')
                {
                    string sHex = input;
                    sHex = sHex.Substring(i + 1, 2);
                    try
                    {
                        int hex = Convert.ToInt32(sHex, 16);
                        byte b = Convert.ToByte(hex);
                        output.Add(b);

                    }
                    catch (Exception e)
                    {

                    }
                    i += 3;
                }
                else
                {
                    output.Add((byte)input[i]);
                    i++;
                }
            }


            if (String.IsNullOrEmpty(bodycharset))
                return Encoding.UTF8.GetString(output.ToArray());
            else
            {
                if (String.Compare(bodycharset, "ISO-2022-JP", true) == 0)
                    return Encoding.GetEncoding("Shift_JIS").GetString(output.ToArray());
                else
                    return Encoding.GetEncoding(bodycharset).GetString(output.ToArray());
            }

        }


    }

}

