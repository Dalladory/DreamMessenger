using Base.Data.Models;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        const int PORT = 8080;
        public RegistrationWindow()
        {
            InitializeComponent();
        }


        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(tbName.Text)
                && !string.IsNullOrEmpty(tbSurname.Text)
                && !string.IsNullOrEmpty(tbEmail.Text)
                && !string.IsNullOrEmpty(tbLogin.Text)
                && !string.IsNullOrEmpty(pbPassword_1.Password)
                && pbPassword_1.Password == pbPassword_2.Password)
            {
                //test 
                User newUser= new User()
                {
                    Name = tbName.Text,
                    Surname = tbSurname.Text,
                    Email = tbEmail.Text,
                    Login = tbLogin.Text,
                    Password = pbPassword_1.Password
                };


                /////////////////////////
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();

            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
