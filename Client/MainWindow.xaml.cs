using Base.Data.Models;
using Client.Windows;
using MaterialDesignThemes.Wpf;
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
        User currentCompanion;
        Chat currentChat;
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
                new ErrorWindow(ex.Message).ShowDialog();
            }

            if (strChats.StartsWith("true"))
            {
                strChats = strChats.Replace("true|", "");
                List<Chat> receiveChats = JsonSerializer.Deserialize<List<Chat>>(strChats);

                List<Chat> newChatsList = receiveChats.Where(c => c.Messages.Count > 0).OrderByDescending(c => c.Messages.Last().SendDate).ToList();
                newChatsList.AddRange(receiveChats.Where((c) => c.Messages.Count == 0));
                foreach (Chat ch in newChatsList)
                {
                    chats.Add(ch);
                }
            }

            ChatList.ItemsSource = chats;
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
                new ErrorWindow(ex.Message).ShowDialog();
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
                    receiveBytesCount = _socket.Receive(buffer);
                    if (receiveBytesCount == 0)
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

                                AddMessageToChat(message);
                                break;
                            }
                        case "AddChat":
                            {
                                Chat chat = JsonSerializer.Deserialize<Chat>(command[1]);
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
                catch (Exception)
                {
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

                //int selectedIndex = ChatList.SelectedIndex;
                ChatList.ItemsSource = null;
                chats.OrderByDescending(c => c.Messages.Last().SendDate);
                ChatList.ItemsSource = chats;
                ChatList.SelectedItem = currentChat;
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
            MessagesZoneUpdate(Visibility.Hidden);
            StartChatPanel.Visibility = Visibility.Hidden;
            ChatList.Visibility = Visibility.Hidden;
            if (UsersList.Visibility != Visibility.Visible)
            {
                UsersList.Visibility = Visibility.Visible;
            }

            if (string.IsNullOrEmpty(SearchTb.Text))
            {
                UsersList.ItemsSource = null;
                UsersList.Visibility = Visibility.Hidden;
                ChatList.Visibility = Visibility.Visible;
                return;
            }

            string usersResponce = "";
            try
            {
                SendTextToServer("SearchUsers|" + SearchTb.Text, out usersResponce);
            }
            catch (Exception ex)
            {
                new ErrorWindow(ex.Message).ShowDialog();
            }

            if (usersResponce.StartsWith("true|"))
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
            if(ChatList.SelectedItem == null)
            {
                return;
            }
            currentChat = (Chat)ChatList.SelectedItem;
            currentCompanion = ((Chat)ChatList.SelectedItem).Companion;
            
            //if (currentChat == null) return;
            MessagesList.ItemsSource = currentChat.Messages;
            if (currentChat.Messages.Count > 0)
            {
                MessagesList.ScrollIntoView(currentChat.Messages.Last());
            }
            CompanionProfileBtn.Content = currentChat.Companion.FullName;
        }

        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersList.SelectedItem == null) return;
            currentCompanion = (User)UsersList.SelectedItem;

            Chat chat = chats.FirstOrDefault(c => c.CreatorId == currentCompanion.Id || c.CompanionId == currentCompanion.Id);
            
            if (chat == null)
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
                currentChat = chat;
                StartChatPanel.Visibility = Visibility.Hidden;
                MessagesZoneUpdate(Visibility.Visible);

                MessagesList.ItemsSource = chat.Messages;
                CompanionProfileBtn.Content = currentCompanion.FullName;
            }


        }

        private void YStartChatBtn_Click(object sender, RoutedEventArgs e)
        {
            StartChatPanel.Visibility = Visibility.Hidden;
            currentCompanion = (User)UsersList.SelectedItem;

            Chat chat = new Chat()
            {
                CreatorId = myUserId,
                Creator = my,
                CompanionId = currentCompanion.Id,
                Companion = currentCompanion
            };


            string serializedChat = JsonSerializer.Serialize<Chat>(chat);
            string result = "";
            try
            {
                SendTextToServer("CreateChat|" + serializedChat, out result);
            }
            catch (Exception)
            {
            }

            if (result.StartsWith("true"))
            {
                chat.Id = int.Parse(result.Replace("true|", ""));
                AddChat(chat);
                currentChat = chat;
                SearchTb.Text = "";
            }
            else
            {
                string message = "";
                ResponseParser.Parse(result, out message);
                new ErrorWindow(message).ShowDialog();
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

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(MessageTb.Text)|| MessageTb.Text=="")
            {
                return;
            }

            Message message = new Message()
            {
                ChatId = currentChat.Id,
                CreatorId = myUserId,
                CompanionId = currentCompanion.Id,
                Text = MessageTb.Text,
                SendDate = DateTime.Now
            };

            string result = "";
            try
            {
                SendTextToServer("AddMessage|" + JsonSerializer.Serialize<Message>(message), out result);
            }
            catch (Exception ex)
            {
                new ErrorWindow(ex.Message).ShowDialog();
                return;
            }

            string responseMessage = "";
            if(ResponseParser.Parse(result, out responseMessage))
            {
                AddMessageToChat(message);
                MessageTb.Text = "";
            }
            else
            {
                new ErrorWindow(responseMessage).ShowDialog();
            }
        }

        private void MsgSendBtn_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageTb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            //if (e.Key == Key.Enter)
            //{
            //    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            //    {
            //        MessageBox.Show("Control + Enter pressed");
            //    }
            //    else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            //    {
            //        MessageBox.Show("Shift + Enter pressed");
            //    }
            //}
            e.Handled = true;
            SendMessage();

        }
    }
}
