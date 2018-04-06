using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImapX;

namespace Email_client.Model
{
    class IMapService
    {
        private static ImapClient client { get; set; }

        public static void Initialize()
        {
            client = new ImapClient("imap.gmail.com", true);

            if (!client.Connect())
            {
                throw new Exception("Error connecting to the client.");
            }
        }


        public static bool Login(string userName, string password)
        {
            return client.Login(userName, password);
        }
        public static void Logout()
        {
            if (client.IsAuthenticated)
            {
                client.Logout();
            }
            MainWindow.LoggedIn = false;
        }
       

    }
}
