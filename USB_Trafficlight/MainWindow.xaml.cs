using System;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace USB_Trafficlight
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Singleton
        public static readonly Duel_Mode CWOBJ = new Duel_Mode();
        // CancellationToken

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            TextBox_Status.Text = "duel mode started";
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            await CWOBJ.InitiateDuelMode();

            button_start.IsEnabled = true;
        }

        private async void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            await CWOBJ.ResetMode();
            await CWOBJ.CloseConnection();
            Environment.Exit(0);
        }

        private async void Button_Reset_Click(object sender, RoutedEventArgs e)
        { 
            await CWOBJ.ResetMode();
        }

        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            await CWOBJ.ResetMode();
            await CWOBJ.CloseConnection();
        }
    }
}
