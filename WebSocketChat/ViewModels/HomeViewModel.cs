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
using System.Net;
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
        private string _ipAddress;
        private ConnectionType _selectedconnectionType = ConnectionType.Internal;
        private HubConnection connection;
        private UserModel _currentUser;
        public EventHandler<ConnectionEventArgs> OnSuccessfulConnect;
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

        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetPropertyValue(ref _isConnecting, value);
        }

        public ConnectionType CurrentConnectionType
        {
            get => _selectedconnectionType;
            set => _selectedconnectionType = value;
        }
        public string CurrentIPAddress
        {
            get => _ipAddress;
            set => SetPropertyValue(ref _ipAddress, value);
        }

        public Array ConnectionTypes { get; } = Enum.GetValues(typeof(ConnectionType));
        public ICommand Host => new RelayCommand(HostServer, CanHostServer);
        public ICommand Connect => new RelayCommand(ConnectToServer, CanConnectToServer);


        private bool CanHostServer()
        {       
            return string.IsNullOrEmpty(_name) || _isHosting || _isConnecting ? false : true; 
        }
        private bool CanConnectToServer()
        {
            if (CurrentConnectionType == ConnectionType.External)
                return string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(CurrentIPAddress) || _isConnecting ? false : true;

            //Disable the connect button if _isHosting is true since it automatically connects.
            return string.IsNullOrEmpty(_name) || _isConnecting || _isHosting ? false : true;
        }
        private void ConnectToServer()
        {
            Task.Run(async () =>
            {
                LogStatus("Connecting...");
                var isSuccessful = await SendUser(_currentUser = new UserModel 
                { 
                    Name = this.Name,
                    EndPoint = CurrentIPAddress                   
                },CurrentConnectionType);

                if (isSuccessful)
                {
                   if(CurrentConnectionType == ConnectionType.External)
                   {
                      connection = new HubConnectionBuilder()
                      .WithUrl($"http://{CurrentIPAddress}:5001/chathub")
                      .Build();                    
                   }
                   else
                   {
                      connection = new HubConnectionBuilder()
                      .WithUrl("http://localhost:5001/chathub")
                      .Build();
                   }
                   CreateHandlers();
                   await connection.StartAsync();
                }
                else
                {
                    LogStatus("Could not connect to the server!");
                    IsConnecting = false;
                }
            });
        }

        private async Task HostServer()
        {
            await Task.Run(() =>
            {
                if (IsServerProcessRunning())
                {
                    LogStatus("A server process is already running!");
                    return;
                }
                var server = GetServer();
                if (server == null)
                {
                    LogStatus("Could not find the server process!");
                    return;
                }
                server.Start();
                _isHosting = true;
                ConnectToServer();
            });
        }

        private Process GetServer()
        {
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo();
            process.StartInfo = processInfo;
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),
            "ChattingHub.exe", SearchOption.AllDirectories);
            if (files.Count() == 0)
            {
                return null;
            }
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
                        ChatViewModelContext = new ChatViewModel(data, _currentUser, connection, new NetworkService())
                    });
                });
                connection.Remove("Connected");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">The user to send to the server</param>
        /// <param name="isExternal">Determines whether the connection is external or internal</param>
        /// <returns></returns>
        private async Task<bool> SendUser(UserModel user, ConnectionType connectionType)
        {
            IsConnecting = true;
            var httpClient = new HttpClient();
            if (connectionType == ConnectionType.External)
            {
                var isSuccessful = IPAddress.TryParse(CurrentIPAddress, out _);
                if (!isSuccessful)
                {
                    return false;
                }              
                httpClient.BaseAddress = new Uri($"http://{CurrentIPAddress}:5001");
            }
            else
            {
                httpClient.BaseAddress = new Uri("http://localhost:5001");
            }
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

        public enum ConnectionType
        {
            Internal,
            External,
        }
    }
}
