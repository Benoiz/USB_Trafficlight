using System;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls;

namespace USB_Trafficlight
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Singleton
        public static readonly Duel_Mode CWOBJ = new Duel_Mode();
        public static MainWindow AppWindow;
        // CancellationToken
        CancellationTokenSource cts;
        public const Task result = default;
        

        public MainWindow()
        {
            InitializeComponent();
            button_reset.IsEnabled = false; // reset button if disabled at the beginning
            AppWindow = this;
        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            Auswahl.IsEnabled = false;
            button_reset.IsEnabled = true;

            Task result;
            int switchCase = Auswahl.SelectedIndex;
            cts = new CancellationTokenSource(); // support for cancellation
            TextBox_Status.Text = "Go";
            switch (switchCase)
            {
                case 0:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 3, 5));
                    break;
                case 1:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 150, 1));
                        break;
                case 2:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 50, 1));
                        break;
                case 3:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 20, 1));
                        break;
                case 4:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 10, 1));
                        break;
                case 5:
                    result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 8, 1));
                        break;
                case 6:
                     result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 6, 1));
                        break;
                case 7:
                     result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 4, 1));
                        break;
                case 8:
                     result = Task.Run(() => CWOBJ.StartDuel(cts.Token, 60, 7, 300, 1));
                        break;
                    default:
                    result = default;
                    break;
            }

            try
            {
                await result;

                TextBox_Status.Text = "Fertig";
            }
            catch (Exception)
            {
                TextBox_Status.Text = "Abgebrochen";
            }
            finally
            {
                cts.Dispose(); // free ressources
            }

            button_reset.IsEnabled = false;
            button_start.IsEnabled = true;
            Auswahl.IsEnabled = true;
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

        public void SetTextbox(string value)
        {
            TextBox_Status.Text = value;
        }

        public string GetTextbox()
        {
            return TextBox_Status.Text;
        }

        private void Auswahl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

    }
}
