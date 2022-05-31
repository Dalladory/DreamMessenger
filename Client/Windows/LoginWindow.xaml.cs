using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Base.Data.Models;
using MaterialDesignThemes.Wpf;

namespace Client.Windows
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        const int PORT = 8080;
        Socket socket;

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("135.181.63.54"), 8080);

        public LoginWindow()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Bind(endPoint);
            socket.Connect(endPoint);
        }
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper=new PaletteHelper();
        

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

       private void logIn()
        {
            if (!string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrEmpty(pbPassword.Password))
            {
                string serverResult = null;
                try
                {
                    byte[] bufferSend = Encoding.UTF8.GetBytes($"SignIn|{tbLogin.Text}|{pbPassword.Password}");
                    socket.Send(bufferSend);
                    byte[] bufferReceived = new byte[1024];
                    int receiveBytesCount = socket.Receive(bufferReceived);
                    serverResult = Encoding.UTF8.GetString(bufferReceived, 0, receiveBytesCount);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (serverResult is not null && serverResult.StartsWith("true"))
                {
                    //MessageBox.Show(serverResult);
                    //int myId = int.Parse(serverResult.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]);
                    User user = JsonSerializer.Deserialize<User>(serverResult.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]);
                    MainWindow mainWindow = new MainWindow(socket, user);
                    this.Close();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show(serverResult);
                }
            }
            else
            {
                MessageBox.Show("Login or password is wrong");
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            logIn();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow(socket);
            this.Close();
            registrationWindow.Show();

        }

        private void pbPassword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            logIn();
        }
    }
}
