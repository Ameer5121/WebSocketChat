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

namespace WebSocketChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<MessageModel> _messages;
        private HubConnection _connection;
        private INetworkService _networkservice;
        public ChatViewModel(DataModel data, HubConnection connection, INetworkService networkservice)
        {
            _users = data.Users;
            _messages = data.Messages;
            _connection = connection;
            _networkservice = networkservice;
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
    }
}
