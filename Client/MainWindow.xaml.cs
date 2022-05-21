using Base.Data.Models;
using Client.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
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
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Chat> chats = new ObservableCollection<Chat>();
        Socket socket;
        int myUserId;

        Task receiveTask;

        User my;

        string responceStr = "";

        public MainWindow(Socket socket, User my)
        {
            InitializeComponent();
            MessagesZoneUpdate(Visibility.Hidden);
            this.my = my;
            this.myUserId = my.Id;
            this.socket = socket;
            
            StartChatPanel.Visibility = Visibility.Hidden;

            receiveTask = Task.Run(async () => Receive(socket, chats, out responceStr));

            string strChats = "";
            try
            {
                SendTextToServer("GetUserChats|" + myUserId, out strChats);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            //MessageBox.Show(strChats);
            if (strChats.StartsWith("true"))
            {
                strChats = strChats.Replace("true|", "");
                //ChatList.ItemsSource = JsonSerializer.Deserialize<ObservableCollection<Chat>>(strChats);
                List<Chat> receiveChats = JsonSerializer.Deserialize<List<Chat>>(strChats);

                receiveChats = receiveChats.Where(c => c.Messages.Count > 0).OrderByDescending(c => c.Messages.Last().SendDate).ToList();
                

                foreach (Chat ch in receiveChats)
                {
                    //MessageBox.Show(ch.Messages.Last().SendDate.ToString("g"));
                    chats.Add(ch);
                }
            }
          
            ChatList.ItemsSource = chats;

        }

        private int SendTextToServer(string text, out string responce)
        {
            int bytesCount = 0;
            responce = "";
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(text));
                
                receiveTask.Wait();
                responce = responceStr;
                
                responceStr = "";

                receiveTask = Task.Run(async () => Receive(socket, chats, out responceStr));
            }
            catch (Exception ex)
            {
                MessageBox.Show("1 " + ex.Message);   
            }
            return bytesCount;
            
        }

        private void Receive(Socket _socket, ObservableCollection<Chat> _chats, out string responce)
        {
            string result = "";
            ObservableCollection<Chat> chts = _chats;
            int receiveBytesCount;
            while (true)
            {
                byte[] buffer = new byte[50000];
                try
                {
                    //_asyncResult = socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, null, null);
                    //_socket.EndReceive(_asyncResult);
                    receiveBytesCount = _socket.Receive(buffer);
                    if(receiveBytesCount == 0)
                    {
                        continue;
                    }
                    result = Encoding.UTF8.GetString(buffer, 0, receiveBytesCount);
                    string[] command = result.Split("|", 2, StringSplitOptions.RemoveEmptyEntries);
                    switch (command[0])
                    {
                        case "AddMessage":
                            {
                                Message message = JsonSerializer.Deserialize<Message>(command[1]);
                                //Chat chat = chts.FirstOrDefault(c => c.Id == message.ChatId);
                                //if (chat != null)
                                //{

                                //    chat.Messages.Add(message);
                                //}
                                AddMessageToChat(message);
                                break;
                            }
                        case "AddChat":
                            {
                                Chat chat = JsonSerializer.Deserialize<Chat>(command[1]);
                                //chts.Add(chat);
                                AddChat(chat);
                                break;
                            }
                            default:
                            {
                                responce = result;
                                return;
                            }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Receive " + ex.Message);
                    //continue;
                }
               
            }        
        }

        private void AddMessageToChat(Message message)
        {
            MessagesList.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                Chat chat = chats.FirstOrDefault(c => c.Id == message.ChatId);
                ObservableCollection<Message> messages = chat.Messages;
                MessagesList.ItemsSource = null;
                messages.Add(message);
                MessagesList.ItemsSource = messages;

                int selectedIndex = ChatList.SelectedIndex;
                ChatList.ItemsSource = null;
                chats.OrderByDescending(c => c.Messages.Last().SendDate);
                ChatList.ItemsSource = chats;
                ChatList.SelectedIndex = selectedIndex;
                MessagesList.ScrollIntoView(message);
            }));
        }

        private void AddChat(Chat chat)
        {
            ChatList.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                chats.Insert(0, chat);
            }));
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show(SearchTb.Text);
            MessagesZoneUpdate(Visibility.Hidden);
            StartChatPanel.Visibility= Visibility.Hidden;
            if (UsersList.Visibility != Visibility.Visible)
            { 
                UsersList.Visibility = Visibility.Visible; 
            }

            if (string.IsNullOrEmpty(SearchTb.Text))
            {
                UsersList.ItemsSource = null;
                UsersList.Visibility = Visibility.Hidden;
                return;
            }

            string usersResponce = "";
            try
            {
                SendTextToServer("SearchUsers|" + SearchTb.Text, out usersResponce);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(usersResponce);
            
            if(usersResponce.StartsWith("true|"))
            {
                
                UsersList.ItemsSource = JsonSerializer.Deserialize<List<User>>(usersResponce.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }

        private void MessagesZoneUpdate(Visibility visibility)
        {
            MessagesList.Visibility = visibility;
            MessageTb.Visibility = visibility;
            MsgSendBtn.Visibility = visibility;
            CompanionProfileBtn.Visibility = visibility;
        }

        private void ChatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessagesZoneUpdate(Visibility.Visible);
            if (ChatList.SelectedItem == null) return;
            MessagesList.ItemsSource = ((Chat)ChatList.SelectedItem).Messages;
            if(((Chat)ChatList.SelectedItem).Messages.Count > 0)
            {
                MessagesList.ScrollIntoView(((Chat)ChatList.SelectedItem).Messages.Last());
            }
            CompanionProfileBtn.Content = ((Chat)ChatList.SelectedItem).Companion.FullName;
        }

        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersList.SelectedItem == null) return;

            Chat chat = chats.FirstOrDefault(c => c.CreatorId == ((User)UsersList.SelectedItem).Id || c.CompanionId == ((User)UsersList.SelectedItem).Id);
            if(chat == null)
            {
                MessagesZoneUpdate(Visibility.Hidden);

                if (UsersList.SelectedItem != null)
                {
                    StartChatPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    StartChatPanel.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                StartChatPanel.Visibility = Visibility.Hidden;
                MessagesZoneUpdate(Visibility.Visible);

                MessagesList.ItemsSource = chat.Messages;
                CompanionProfileBtn.Content = chat.Companion.FullName;
            }
            
            
        }

        private void YStartChatBtn_Click(object sender, RoutedEventArgs e)
        {
            StartChatPanel.Visibility = Visibility.Hidden;
            //MessageBox.Show(((User)UsersList.SelectedItem).Id.ToString());
            

            Chat chat = new Chat()
            {
                CreatorId = myUserId,
                Creator = my,
                CompanionId = ((User)UsersList.SelectedItem).Id,
                Companion = (User)UsersList.SelectedItem
            };


            string serializedChat = JsonSerializer.Serialize<Chat>(chat);
            string result = "";
            try
            {
                SendTextToServer("CreateChat|" + serializedChat, out result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if(result.StartsWith("true"))
            {
                chat.Id = int.Parse(result.Replace("true|", ""));
                //chat.Creator = my;
                //chat.Companion = (User)UsersList.SelectedItem;
                AddChat(chat);
                //chats.Add(chat);
                SearchTb.Text = "";
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void NStartChatBtn_Click(object sender, RoutedEventArgs e)
        {
            StartChatPanel.Visibility = Visibility.Hidden;
        }

        private void CompanionProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            //new CompanionProfileWindow();
        }

        private void MsgSendBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(MessageTb.Text))
            {
                return;
            }

            Message message = new Message()
            {
                ChatId = ((Chat)ChatList.SelectedItem).Id,
                CreatorId = myUserId,
                CompanionId = ((Chat)ChatList.SelectedItem).Companion.Id,
                Text = MessageTb.Text,
                SendDate = DateTime.Now
            };

            //byte[] buffer = Encoding.UTF8.GetBytes("AddMessage|" + JsonSerializer.Serialize<Message>(message));

            //int receivedBytesCount = socket.Send(buffer);
            string result = "";
            try
            {
                SendTextToServer("AddMessage|" + JsonSerializer.Serialize<Message>(message), out result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            AddMessageToChat(message);

            //ObservableCollection<Message> messages = ((Chat)ChatList.SelectedItem).Messages;
            //MessagesList.ItemsSource = null;
            //messages.Add(message);
            //MessagesList.ItemsSource = messages;

            //((Chat)ChatList.SelectedItem).Messages.Add(message);
            //int selectedIndex = ChatList.SelectedIndex;
            //ChatList.ItemsSource = null;
            
            //ChatList.ItemsSource = chats;
            //ChatList.SelectedIndex = selectedIndex;
            
            MessageTb.Text = "";
            

        }
    }
}
