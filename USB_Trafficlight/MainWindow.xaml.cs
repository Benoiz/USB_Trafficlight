using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

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

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            CWOBJ.InitiateDuelMode();
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            //proper exit needed
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
            Environment.Exit(0);
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            CWOBJ.ResetMode();
        }

        private void MainWindow_Closing(object sender, RoutedEventArgs e)
        {
            CWOBJ.ResetMode();
            CWOBJ.CloseConnection();
        }
    }
}
