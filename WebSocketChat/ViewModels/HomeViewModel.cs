using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketChat.Models;
using System.Windows.Input;
using WebSocketChat.Commands;

namespace WebSocketChat.ViewModels
{
    class HomeViewModel
    {
        private bool _ishosting;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                _name = value;
            }
        }

        public ICommand Host => new RelayCommand(HostServer, CanHostServer);
        public ICommand Connect => new RelayCommand(ConnectToServer, CanConnectToServer);


        private bool CanHostServer()
        {
            return _name == null || _ishosting ? false : true;
        }
        private bool CanConnectToServer()
        {
            return _name == null ? false : true;
        }

        private void ConnectToServer()
        {

        }

        private void HostServer()
        {
            var server = GetServer();
            server.Start();
        }

        private Process GetServer()
        {
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo();
            process.StartInfo = processInfo;
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),          
            "ChattingHub.exe", SearchOption.AllDirectories);;
            processInfo.FileName = files[0];
            process.StartInfo = processInfo;
            return process;
        }
    }
}
