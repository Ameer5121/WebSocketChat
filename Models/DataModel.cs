using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Models
{
    public static class DataModel
    {
        public static ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public static ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();

    }
}
