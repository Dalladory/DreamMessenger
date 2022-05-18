using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Client.Windows
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        const int PORT = 8080;
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ipaddress = null;
        
        public LoginWindow()
        {
            InitializeComponent();
            IPAddress.TryParse("135.181.63.54", out ipaddress);
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrEmpty(pbPassword.Password))
            {

                try
                {

                    client.Connect(ipaddress, PORT);
                    byte[] bufferSend = Encoding.UTF8.GetBytes($"SignIn|{tbLogin.Text}|{pbPassword.Password}");
                    client.Send(bufferSend);
                    byte[] bufferRecived = new byte[1024];
                    int recive = client.Receive(bufferRecived);
                    string resivedData = Encoding.UTF8.GetString(bufferRecived, 0, recive);
                    if (resivedData.Contains("true"))
                    {
                        MainWindow mainWindow = new MainWindow(resivedData, client);
                        this.Close();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show(resivedData, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (client != null)
                    {
                        if (client.Connected)
                        {
                            client.Shutdown(SocketShutdown.Both);
                        }
                        client.Close();
                        client.Dispose();
                    }
                }


            }
            else
            {
                MessageBox.Show("Error");
            }

        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            this.Close();
            registrationWindow.Show();

        }
    }
}
