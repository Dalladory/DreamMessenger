using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Chat> chats = new ObservableCollection<Chat>();
        public MainWindow()
        {
            InitializeComponent();

            ChatList.ItemsSource = chats;

            Chat chat = new Chat();
            
            List<Message> messages = new List<Message>();

            messages.Add(new Message()
            {
                Text = "BlaBla Message"
            });

            chat.Messages = messages;
            chat.User = new User()
            {
                Name = "Bob",
                Surname = "Bobovich"
            };
            chats.Add(chat);
            
        }
    }
}
