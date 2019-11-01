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
        CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            TextBox_Status.Text = "duel mode started";

            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                await CWOBJ.InitiateDuelMode(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                TextBox_Status.Text = "Task canceled!";
            }
            catch (Exception)
            {
                TextBox_Status.Text = "Error occurred while in duel mode!";
            }
            button_start.IsEnabled = true;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
            Environment.Exit(0);
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {   try
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            CWOBJ.ResetMode();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
        }
    }
}
