using System;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using WebSocketChat.Commands;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using WebSocketChat.Events;
using Newtonsoft.Json;
using WebSocketChat.Services;

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
            set => _name = value;
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
            return string.IsNullOrEmpty(_name) || _isHosting || _isConnecting ? false : true;
        }
        private bool CanConnectToServer()
        {
            return string.IsNullOrEmpty(_name) || _isConnecting ? false : true;
        }

        private void ConnectToServer()
        {
            Task.Run(async () =>
            {
                LogStatus("Connecting...");
                var isSuccessful = await SendUser(new UserModel { Name = this.Name });
                if (isSuccessful)
                {
                    connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/chathub")
                    .Build();
                    CreateHandlers();
                    await connection.StartAsync();
                }
                else
                {
                    LogStatus("Could not connect to the server!");
                    _isConnecting = false;
                }
            });
        }

        private void HostServer()
        {
            if (IsServerProcessRunning())
            {
                LogStatus("A server process is already running!");
                return;
            }
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

        private bool IsServerProcessRunning()
        {
            var serverProcess = Process.GetProcessesByName("ChattingHub");
            if(serverProcess.Count() > 0)
            {
                return true;
            }
            return false;
        }
        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {
                // Invoke the handler from the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnSuccessfulConnect?.Invoke(this, new ConnectionEventArgs
                    {
                        ChatViewModelContext = new ChatViewModel(data, connection, new NetworkService())
                    });
                });       
            });
        }

        private async Task<bool> SendUser(UserModel user)
        {
            _isConnecting = true;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
            var jsonData = JsonConvert.SerializeObject(user);
            try
            {
                var response = await httpClient.PostAsync("/api/chat/PostUser",
                    new StringContent(jsonData, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                    return true;

            }catch(HttpRequestException e)
            {
                return false;
            }
            return false;
        }

        private async Task LogStatus(string message)
        {
            Status = message;
            await Task.Delay(2000);
            Status = default;
        }

    }
}
