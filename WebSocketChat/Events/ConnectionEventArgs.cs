using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WebSocketChat.ViewModels;

namespace WebSocketChat.Events
{
    class ConnectionEventArgs : EventArgs
    {
        public ChatViewModel ChatViewModelContext;
    }
}
