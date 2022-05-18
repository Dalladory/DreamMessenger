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
        const int PORT = 8080;
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ipaddress = null;
        public RegistrationWindow()
        {
            InitializeComponent();
            IPAddress.TryParse("135.181.63.54", out ipaddress);
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();


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
                //test 
                User newUser = new User()
                {
                    Name = tbName.Text,
                    Surname = tbSurname.Text,
                    Email = tbEmail.Text,
                    Login = tbLogin.Text,
                    Password = pbPassword_1.Password
                };

                try
                {

                    client.Connect(ipaddress, PORT);
                    string usertSerialized = JsonSerializer.Serialize(newUser);

                    byte[] bufferSend = Encoding.UTF8.GetBytes("AddUser|" + usertSerialized);
                    client.Send(bufferSend);
                    byte[] bufferRecived = new byte[1024];
                    int recive = client.Receive(bufferRecived);
                    string resivedData = Encoding.UTF8.GetString(bufferRecived, 0, recive);
                    MessageBox.Show(resivedData);
                    MainWindow mainWindow = new MainWindow(resivedData, client);
                    this.Close();
                    mainWindow.Show();

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

                /////////////////////////


            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
