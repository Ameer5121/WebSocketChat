using System;
using System.Windows;
using WebSocketChat.ViewModels;
using Models;


namespace WebSocketChat.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        public ChatView(DataModel data)
        {
            InitializeComponent();
            DataContext = new ChatViewModel(data);
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
                this.DragMove();
        }

        private void HeaderSite_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
