using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public struct MessageModel
    {
        private string _message;
        private UserModel _user;

       public string Message
       {
            get => _message;
            set => _message = value;
       }
       public UserModel User
       {
            get => _user;
            set => _user = value;
       }
    }
}
