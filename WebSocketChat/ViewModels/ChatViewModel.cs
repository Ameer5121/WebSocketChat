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

namespace WebSocketChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<MessageModel> _messages;
        private HubConnection _connection;
        private INetworkService _networkservice;
        public event EventHandler OnDisconnect;
        public ChatViewModel(DataModel data, HubConnection connection, INetworkService networkservice)
        {
            _users = data.Users;
            _messages = data.Messages;
            _connection = connection;
            _networkservice = networkservice;
            SendHeartBeat();
        }

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

        private async Task SendHeartBeat()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
            Task<HttpResponseMessage> getHeartBeatTask = httpClient.GetAsync("api/chat/GetHeartBeat");
            var completedTask = await Task.WhenAny(getHeartBeatTask, Task.Delay(5000));
            if (completedTask == getHeartBeatTask)
            {
                await Task.Delay(5000);
                SendHeartBeat();
            }
            else
            {
                OnDisconnect?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
