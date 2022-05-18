using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MainWindow(string text, Socket client)
        {
            InitializeComponent();
            string[] words = text.Split(new char[] { '|' });
            int index = Convert.ToInt32(words[1]);
            ChatList.ItemsSource = chats;
            try
            {
                byte[] bufferSend = Encoding.UTF8.GetBytes($"GetChats|{index}");
                client.Send(bufferSend);
                byte[] bufferRecived = new byte[1024];
                int recive = client.Receive(bufferRecived);
                string resivedData = Encoding.UTF8.GetString(bufferRecived, 0, recive);
                var cathes= JsonSerializer.Deserialize<List<Chat>>(resivedData);
                foreach (var item in cathes)
                {
                    chats.Add(item);
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
    
    }
}
