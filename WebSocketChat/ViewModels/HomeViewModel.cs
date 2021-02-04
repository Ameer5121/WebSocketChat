using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketChat.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using WebSocketChat.Commands;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebSocketChat.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private bool _ishosting;
        private string _name;
        private string _status;


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

        public string Status
        {
            get => _status;
            set => SetPropertyValue(ref _name, value);       
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

        private async Task ConnectToServer()
        {
           var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chathub")
                .Build();
            Status = LogStatus("Connecting...");
            await Task.WhenAny(connection.StartAsync(), Task.Delay(2000));
            if (connection.ConnectionId == null)
            {
                Status = LogStatus("Could not connect to the server!");
                await Task.Delay(1500);
                Status = default;
            }
            else
            {
                //connection.On<>
            }
        }

        private void HostServer()
        {
            var server = GetServer();
            server.Start();
            _ishosting = true;
            
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

        private string LogStatus(string message)
        {
            return null;
        }
    }
}
