using System;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;

namespace USB_Trafficlight
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //singleton
        public static readonly Duel_Mode CWOBJ = new Duel_Mode();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            var result = await Task.Run(() =>
            {
                CWOBJ.InitiateDuelMode();
                return "done";
            });

            button_start.IsEnabled = true;
            TextBox_Status.Text = result;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
            Environment.Exit(0);
        }

        private async void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            var result = await Task.Run(() =>
            {
                CWOBJ.ResetMode();
                return "reset done";
            });
            TextBox_Status.Text = result;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
        }
    }
}
