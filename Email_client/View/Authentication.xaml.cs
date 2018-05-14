using System.Collections.Generic;
using System.Windows;
using Email_client.Model;
using System.Linq;

namespace Email_client.View
{
    public partial class Authentication : Window
    {
        UnitOfWork unitOfWork;
        Main mainWindow;
        dynamic users;
        public Authentication()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            mainWindow = new Main();
            users = unitOfWork.Users.GetAll();
        }

        private void AuthetnticationButton_Click(object sender, RoutedEventArgs e)
        {
           // unitOfWork.Users.Create(new UsersInfo() {Login="aaa@mail.ru", Password= "dasd" });
           // unitOfWork.Save();
           
            UsersInfo result=null;
            foreach (var user in users)
            {
                if (user.Login == LoginTextBox.Text && user.Password == passwordBox.Password)
                {
                    result = user;
                    break;
                }
            }
            if (result!=null)
            {
                mainWindow.Show();
                mainWindow.CreateSMTPWIndowAndConnectToServer(result.Login,result.Password);
                this.Close();
            }
        }
    }
}
