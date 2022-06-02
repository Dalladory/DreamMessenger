using Base.Data.Models;
using MaterialDesignThemes.Wpf;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
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

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        Socket socket;
        public RegistrationWindow(Socket socket)
        {

            InitializeComponent();
            this.socket = socket;
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();


        bool ResponceParse(string serverMessage, out string ErrorMessage)
        {
            if (serverMessage is not null)
            {
                if (serverMessage.StartsWith("true"))
                {
                    ErrorMessage = serverMessage;
                    return true;
                }
                else
                {
                    ErrorMessage = serverMessage.Trim('f', 'a', 'l', 's', 'e', '|');
                    return false;
                }
            }
            else
                ErrorMessage = null;
                return false;

        }

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

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text)
                && !string.IsNullOrEmpty(tbSurname.Text)
                && !string.IsNullOrEmpty(tbEmail.Text)
                && !string.IsNullOrEmpty(tbLogin.Text)
                && !string.IsNullOrEmpty(pbPassword_1.Password)
                && pbPassword_1.Password == pbPassword_2.Password)
            {

                User newUser = new User()
                {
                    Name = tbName.Text,
                    Surname = tbSurname.Text,
                    Email = tbEmail.Text,
                    Login = tbLogin.Text,
                    Password = pbPassword_1.Password
                };

                string serverResult = null;
                try
                {
                    string userSerialized = JsonSerializer.Serialize<User>(newUser);

                    byte[] bufferSend = Encoding.UTF8.GetBytes("AddUser|" + userSerialized);
                    socket.Send(bufferSend);
                    byte[] bufferReceived = new byte[1024];
                    int receiveBytesCount = socket.Receive(bufferReceived);
                    serverResult = Encoding.UTF8.GetString(bufferReceived, 0, receiveBytesCount);
                }
                catch (Exception ex)
                {
                    ErrorWindow error = new ErrorWindow(ex.Message);
                    error.Show();
                    return;
                }

                if (ResponceParse(serverResult,out serverResult))
                {
                    int myId = int.Parse(serverResult.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]);
                    newUser.Id = myId;
                    MainWindow mainWindow = new MainWindow(socket, newUser);
                    this.Close();
                    mainWindow.Show();
                }
                else
                {
                    ErrorWindow error = new ErrorWindow(serverResult);
                    error.ShowDialog();
                }

            }
            else
            {
                ErrorWindow error = new ErrorWindow("Some fields is empty","DATA ERROR");
                error.ShowDialog();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
            
        }
    }
}
