using System.Windows;
using Email_client.Model;

namespace Email_client.View
{
    public partial class Authentication : Window
    {
        readonly UnitOfWork _unitOfWork;

        Main _mainWindow;

        dynamic _users;


        public Authentication()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            _mainWindow = new Main();
            _users = _unitOfWork.Users.GetAll();
        }


        private void AuthetnticationButton_Click(object sender, RoutedEventArgs e)
        {
            UsersInfo result=null;

            foreach (var user in _users)
            {
                if (user.Login == LoginTextBox.Text && user.Password == PasswordBox.Password)
                {
                    result = user;
                    break;
                }
            }
            if (result!=null)
            {
                _mainWindow.Show();
                _mainWindow.CreateSmtpWindowAndConnectToServer(result.Login,result.Password);
                this.Close();
            }
        }

    }
}
