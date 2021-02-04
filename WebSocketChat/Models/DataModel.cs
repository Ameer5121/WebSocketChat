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
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }

        public DataModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Users = new ObservableCollection<UserModel>();
        }
    }
}
