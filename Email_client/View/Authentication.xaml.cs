using System.Windows;
using Email_client.Model;

namespace Email_client.View
{
    public partial class Authentication : Window
    {
        readonly UnitOfWork unitOfWork;

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
            UsersInfo result=null;

            foreach (var user in users)
            {
                if (user.Login == LoginTextBox.Text && user.Password == PasswordBox.Password)
                {
                    result = user;
                    break;
                }
            }
            if (result!=null)
            {
                mainWindow.Show();
                mainWindow.CreateSmtpWindowAndConnectToServer(result.Login,result.Password);
                this.Close();
            }
        }

    }
}
