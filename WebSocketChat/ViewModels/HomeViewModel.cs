using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using WebSocketChat.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using WebSocketChat.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using WebSocketChat.Events;
using Newtonsoft.Json;

namespace WebSocketChat.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private bool _isHosting;
        private bool _isConnecting = false;
        private string _name;
        private string _status;
        public EventHandler<ConnectionEventArgs> OnSuccessfulConnect;
        private HubConnection connection;

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
            set => SetPropertyValue(ref _status, value);       
        }

        public ICommand Host => new RelayCommand(HostServer, CanHostServer);
        public ICommand Connect => new RelayCommand(ConnectToServer, CanConnectToServer);


        private bool CanHostServer()
        {
            return _name == null || _isHosting ? false : true;
        }
        private bool CanConnectToServer()
        {
            return _name == null || _isConnecting ? false : true;
        }

        private async Task ConnectToServer()
        {
            Status = LogStatus("Connecting...");
            var result = await SendUser(new UserModel { Name = this.Name });
            if (result)
            {
                connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chathub")
                .Build();
                CreateHandlers();
                await connection.StartAsync();
            }
            else
            {
                Status = LogStatus("Could not connect to the server!");
                await Task.Delay(1500);
                Status = default;
            }
        }

        private void HostServer()
        {
            var server = GetServer();
            server.Start();
            _isHosting = true;       
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
           return Status = message;
        }

        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {
                OnSuccessfulConnect?.Invoke(this, new ConnectionEventArgs { Data = data });
            });
        }

        private async Task<bool> SendUser(UserModel user)
        {
            _isConnecting = true;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44358");
            var jsonData = JsonConvert.SerializeObject(user);
            var result = await httpClient.PostAsync("/api/chat/PostUser", 
                new StringContent(jsonData, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
