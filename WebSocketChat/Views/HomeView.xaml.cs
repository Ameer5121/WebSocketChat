using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketChat.Events;
using WebSocketChat.ViewModels;

namespace WebSocketChat.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
            (DataContext as HomeViewModel).OnSuccessfulConnect += ChangeWindow;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
                this.DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangeWindow(object sender, ConnectionEventArgs e)
        {
            ChatView chatView = new ChatView(e.Data);
            chatView.Show();
            this.Hide();
        }
    }
}
