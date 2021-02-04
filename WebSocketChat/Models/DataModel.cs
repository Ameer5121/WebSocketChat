using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace WebSocketChat.Models
{
    public class DataModel
    {
        public readonly ObservableCollection<MessageModel> Messages;
        public readonly ObservableCollection<UserModel> Users;

        public DataModel(ObservableCollection<MessageModel> messages, ObservableCollection<UserModel> users)
        {
            Messages = messages;
            Users = users;
        }
    }
}
