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
            UsersInfo result = null;

            //foreach (var user in _users)
            //{
            //    if (user.Login == LoginTextBox.Text && user.Password == PasswordBox.Password)
            //    {
            //        result = new UsersInfo() {Login = LoginTextBox.Text,Password = PasswordBox.Password};
            //        break;
            //    }
            //}
            //if (result!=null)
            //{
            if (_mainWindow.CreateSmtpWindowAndConnectToServer(LoginTextBox.Text, PasswordBox.Password))
            {
                _mainWindow.Show();
                Close();
            }

            //}
            else

            {
                MessageBox.Show("Login or Password Error");
            }
            //_mainWindow.Show();
        }

    }
}
