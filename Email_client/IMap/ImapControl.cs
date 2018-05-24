using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Email_client.Model;

namespace Email_client.IMap
{
    public class ImapControl
    {
        readonly int _port;

        private TcpClient _tcpClient;
        private SslStream _ssl;


        private byte[] _dummy;
        private byte[] _byffer;

        private int _bytes = -1;

        private StringBuilder _sb;

        private List<string> _uids;
        private List<MessageModel> _emails;


        public ImapControl(int port)
        {
            _port = port;
        }


        public bool Connect(LoginInfo user)
        {
            try
            {
                _tcpClient = new TcpClient(user.ImapAddress, _port);
                _ssl = new SslStream(_tcpClient.GetStream());
                _ssl.AuthenticateAsClient(user.ImapAddress);
                string login = SendCommandToImapServer("$ LOGIN " + user.Username + " " + user.Password + "  \r\n");

                if (!login.ToLower().Contains("ok"))
                {
                    return false;
                }
                SendCommandToImapServer("$ SELECT INBOX\r\n");
                SendCommandToImapServer("$ STATUS Inbox (MESSAGES)\r\n");
                SendCommandToImapServer("");
                if (_tcpClient.Connected)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }

        }

        public void Logout()
        {
            SendCommandToImapServer("$ LOGOUT" + "\r\n");
        }

        public void ModificateMessageFlagOnTheServer(string uid, string flag, char sign)
        {
            SendCommandToImapServer("$ STORE " + uid + " " + sign + "FLAGS (" + flag + ")\r\n");
            SendCommandToImapServer("$ EXPUNGE" + "\r\n");
        }

        public List<MessageModel> UpdateListMessages()
        {
            _emails.Clear();
            _uids = GetUids();
            foreach (var uid in _uids)
            {
                string body = SendCommandToImapServer("$ FETCH " + uid + " body.peek[header]\r\n").Replace("\0", "");
                if (body == "$ OK Success\r\n" || body == string.Format(" * {0} FETCH(BODY[TEXT] ", uid))
                {
                    body = SendCommandToImapServer("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                }

                var email = GetEmailTemplate(body, uid);
                email.Text = GetBody(uid);
                email.TextHtml = GetBodyHtml(uid);
                email.Flags = GetMessageFlags(uid);
                _emails.Add(email);
            }

            return _emails;
        }
        public List<MessageModel> GetListOfMessages()
        {
            if (_emails != null)//проверка,на то получали ли мы сообщения с почты
                return _emails;
            _emails = new List<MessageModel>();//если нет,то инициализируем список 

            if (_uids == null) //проверка,получали ли мы uid ,если нет то получаем 
                GetUids();

            foreach (var uid in _uids)//получаем сообщения с почты 
            {
                string body = SendCommandToImapServer("$ FETCH " + uid + " body.peek[header]\r\n").Replace("\0", "");//получаем тело ответа с сервера
                if (body == "$ OK Success\r\n" || body == $" * {uid} FETCH(BODY[TEXT] ")//Если не получили,то чего ожидали используем другой вариант запроса
                {
                    body = SendCommandToImapServer("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
                }

                var email = GetEmailTemplate(body, uid);
                email.TextHtml = GetBodyHtml(uid);//полуаем текст в виде HTML
                email.Text = GetBody(uid);//получем тело в виде простого текста
                email.Flags = GetMessageFlags(uid);//получаем список флагов сообщения
                _emails.Add(email);//добовляем в список сообщение
            }

            return _emails;
        }

        public string GetBodyHtml(string uid)
        {
            string body = SendCommandToImapServer("$ FETCH " + uid + " body.peek[text]\r\n").Replace("\0", "");
            if (body == "$ OK Success\r\n")
            {
                body = SendCommandToImapServer("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
            }

            Regex regexEncoding = new Regex(@"(?<=\r\nContent-Transfer-Encoding: )([\s\S]*?)(?=(\r\n))");

            var encoding = "utf-8";

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
            catch (Exception)
            {
                return "empty";
            }
            int endIndex = body.IndexOf("\r\n--", startIndex);
            var sbstr = body.Substring(startIndex, endIndex - startIndex);
            encoding = regexEncoding.Match(sbstr).Value;
            if (sbstr.Length > sbstr.IndexOf("\r\n", 2))
                sbstr = sbstr.Substring(sbstr.IndexOf("\r\n", 2));
            body = sbstr.Substring(sbstr.IndexOf("\r\n") + "\r\n".Length);
            return "<!DOCTYPE HTML><html><head><meta http-equiv = 'Content-Type' content = 'text/html;charset=UTF-8'></head><body>" + DecodeBody(body, encoding, "utf-8") + "</body></html>";
        }

        public string GetBody(string uid)
        {
            string body = SendCommandToImapServer("$ FETCH " + uid + " body.peek[text]\r\n").Replace("\0", "");
            if (body == "$ OK Success\r\n")
            {
                body = SendCommandToImapServer("$ FETCH " + uid + " (BODY[])\r\n").Replace("\0", "");
            }
            Regex regexEncoding = new Regex(@"(?<=\r\nContent-Transfer-Encoding: )([\s\S]*?)(?=(\r\n))");
            Regex regexType = new Regex(@"(?<=\r\nContent-Type: )(.*?)(?=(;))");

            var typeMatch = regexType.Match(body).Value;
            string encoding;

            if (typeMatch == "text/html" || typeMatch == "text/plain")
            {
                var div = body.Split(new[] { "\r\n" }, StringSplitOptions.None);
                var answer = div[4];
                if (string.IsNullOrEmpty(answer))
                {
                    answer = div[5];
                    answer = DecodeBody(answer, "base64", "utf-8");
                }

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
            catch (Exception)
            {
                return "empty";
            }
            int endIndex = body.IndexOf("\r\n--", startIndex);
            var sbstr = body.Substring(startIndex, endIndex - startIndex);
            encoding = regexEncoding.Match(sbstr).Value;
            sbstr = sbstr.Substring(sbstr.IndexOf("\r\n", 2));
            body = sbstr.Substring(sbstr.IndexOf("\r\n") + "\r\n".Length);

            return DecodeBody(body, encoding, "utf-8");
        }

        public string SendCommandToImapServer(string command)
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
                _buffer = new byte[2048];
                _tcpClient.ReceiveTimeout = 2000000;

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
                    if (LookForEndOfMessage(str))
                        break;
                }

                return _sb.ToString();
            }
            catch (Exception)
            {
                return "WAS ERROR";
            }
        }

        public List<string> GetMessageFlags(string uid)
        {
            List<string> answer = new List<string>();
            var flags = SendCommandToImapServer("$ FETCH " + uid + " (FLAGS)\r\n");
            Regex regex = new Regex(@"\\(\w*)");
            var separatedFlags = regex.Matches(flags);

            if (separatedFlags.Count > 0)
            {
                foreach (Match flag in separatedFlags)
                    answer.Add(flag.Value);
            }

            return answer;
        }

        public List<string> GetUids()
        {
            _uids = new List<string>();
            var uidswithSearch = SendCommandToImapServer("$ uid search all\r\n");
            Regex regex = new Regex(@"( SEARCH )((\d*) )*(\d*)");
            var answer = regex.Match(uidswithSearch).Value.Split(' ');


            for (int j = 1; j < answer.Length - 1; j++)
            {
                _uids.Add(j.ToString());
            }
            return _uids;
        }

        public MessageModel GetEmailTemplate(string text, string uid)
        {
            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            MessageModel email = new MessageModel(uid);
            if (lines.Length > 5)
                email.Author = lines[4];
            foreach (var line in lines)
            {
                if (line.StartsWith("From"))
                {
                    var senders = regex.Matches(line);
                    if (senders.Count > 0)
                    {
                        foreach (Match sender in senders)
                            email.Author = sender.Value;
                    }
                }

                if (line.StartsWith("Date:"))
                {
                    var date = line.Replace("Date: ", string.Empty);
                    if (date.Contains("("))
                    {
                        date = date.Substring(0, date.Length - 5);
                    }

                    email.DateTime = Convert.ToDateTime(date);
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

            return email;
        }

        private static string DecodeBody(string body, string encoding, string charset)
        {
            switch (encoding)
            {
                case "quoted-printable":
                    return MessageDecoder.DecodeQp(body, "utf-8");
                case "base64":
                    var bytes = Convert.FromBase64String(body);
                    return Encoding.GetEncoding(charset).GetString(bytes);
                default:
                    return body;
            }
        }

        private bool LookForEndOfMessage(string str)
        {
            return str.Contains("$ OK")
                   || str.Contains("$ NO")
                   || str.Contains("$ BAD")
                   || str.Contains("Gimap")
                   || str.Contains("* BAD");
        }
    }
}