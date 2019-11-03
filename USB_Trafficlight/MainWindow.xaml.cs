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
        CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
            button_reset.IsEnabled = false; // reset button is disabled at the beginning
        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            button_reset.IsEnabled = true;
            cts = new CancellationTokenSource(); // support for cancellation
            TextBox_Status.Text = "Go";
            Task result = Task.Run(() => CWOBJ.InitiateDuelMode(cts.Token), cts.Token);

            try
            {
                await result;
                TextBox_Status.Text = "Done";
            }
            catch (Exception)
            {
                TextBox_Status.Text = "Canceled";
            }
            finally
            {
                cts.Dispose(); // free ressources
            }

            button_start.IsEnabled = true;
        }

        private async void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            button_exit.IsEnabled = false;
            await CWOBJ.ResetMode();
            await CWOBJ.CloseConnection();
            Environment.Exit(0);
        }

        private async void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            button_reset.IsEnabled = false;
            if (cts != null)
            {
                cts.Cancel(); // trigger cancellation
            }
            await CWOBJ.ResetMode();
        }

        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            await CWOBJ.ResetMode();
            await CWOBJ.CloseConnection();
        }
    }
}
