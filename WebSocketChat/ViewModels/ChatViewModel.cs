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
            CreateHandlers();
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
