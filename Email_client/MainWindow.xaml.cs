
using System.Windows;
using System.Windows.Controls;
using Email_client.Map;



namespace Email_client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Frame MainFrame { get; set; }
        public static bool LoggedIn { get; set; }
        public MainWindow()
        {
            
            InitializeComponent();
            MainFrame = mainFrame;          
            //MainFrame.Content = new LoginPage();        
            IMapService.Initialize();
        }
    }
}
