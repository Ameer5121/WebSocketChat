using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WebSocketChat.ViewModels;
using Models;
using Microsoft.AspNetCore.SignalR.Client;
using WebSocketChat.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Input;
using WebSocketChat.Commands;

namespace WebSocketChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private UserModel _currentUser;
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<MessageModel> _messages;
        private string _currentMessage;
        private HubConnection _connection;
        private INetworkService _networkservice;
        public event EventHandler OnDisconnect;
        public ChatViewModel(DataModel data, UserModel currentuser, HubConnection connection, INetworkService networkservice)
        {
            _currentUser = currentuser;
            _users = data.Users;
            _messages = data.Messages;
            _connection = connection;
            _networkservice = networkservice;
            CreateHandlers();
            SendHeartBeat();
        }

        public ICommand Send => new RelayCommand(SendMessage, CanSendMessage);
        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set => SetPropertyValue(ref _users, value);
        }
        public ObservableCollection<MessageModel> Messages
        {
            get => _messages;
            set => SetPropertyValue(ref _messages, value);
        }

        public string CurrentMessage
        {
            get => _currentMessage;
            set => SetPropertyValue(ref _currentMessage, value);
        }

        private bool CanSendMessage()
        {
            return string.IsNullOrEmpty(CurrentMessage) ? false : true;
        }
        private async Task SendMessage()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
            var messagetoSend = new MessageModel(_currentMessage, _currentUser);
            var jsonData = JsonConvert.SerializeObject(messagetoSend);
            try
            {
                await httpClient.PostAsync("/api/chat/PostMessage",
                    new StringContent(jsonData, Encoding.UTF8, "application/json"));

                CurrentMessage = default;
            }
            catch (HttpRequestException)
            {
                Messages.Add(new MessageModel("Could not send message.", new UserModel
                {
                    Name = "System"
                })); ;
            }
        }

        private async Task SendHeartBeat()
        {
            while (true)
            {
                await Task.Delay(2000);
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:5001");
                try
                {
                    var response = await httpClient.GetAsync("api/chat/GetHeartBeat");
                }
                catch (HttpRequestException)
                {
                    //Server is down.
                    _connection.Remove("ReceiveData");
                    OnDisconnect?.Invoke(this, EventArgs.Empty);
                    break;
                }
            }
        }

        private void CreateHandlers()
        {
            _connection.On<DataModel>("ReceiveData", ReceiveData);
        }

        private void ReceiveData(DataModel data)
        {
            if (data.Users.Count != _users.Count)
            {
                Users = data.Users;
            }else if(data.Messages.Count != _messages.Count)
            {
                Messages = data.Messages;
            }               
        }
    }
}
