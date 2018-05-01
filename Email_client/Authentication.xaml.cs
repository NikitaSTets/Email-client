using System.Windows;
using Email_client.Model;
using System.Linq;

namespace Email_client.View
{
    public partial class Authentication : Window
    {
        UnitOfWork unitOfWork;
        Main mainWindow;
        public Authentication()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            mainWindow = new Main();
        }

        private void AuthetnticationButton_Click(object sender, RoutedEventArgs e)
        {
           // unitOfWork.Users.Create(new UsersInfo() {Login="aaa@mail.ru", Password= "dasd" });
           // unitOfWork.Save();
            var users = unitOfWork.Users.GetAll();
            UsersInfo result=null;
            foreach (var user in users)
            {
                if (user.Login == LoginTextBox.Text && user.Password == passwordBox.Password)
                {
                    result = user;
                    break;
                }
            }
            //var user = from currentUser in unitOfWork.Users.GetAll()
            //           where currentUser.Login == LoginTextBox.Text && currentUser.Password == passwordBox.Password
            //           select currentUser;

            if (result!=null)
            {
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
