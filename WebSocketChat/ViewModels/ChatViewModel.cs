using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WebSocketChat.ViewModels;
using Models;

namespace WebSocketChat.ViewModels
{
    class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<MessageModel> _messages;

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
        public ChatViewModel(DataModel data)
        {
            _users = data.Users;
            _messages = data.Messages;
        }


    }
}
