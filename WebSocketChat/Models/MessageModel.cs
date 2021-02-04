using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketChat.Models
{
    readonly struct MessageModel
    {
        public readonly string Message;
        public readonly UserModel User;

        public MessageModel(string message, UserModel user)
        {
            Message = message;
            User = user;
        }
    }
}
