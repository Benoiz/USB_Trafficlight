using System;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace USB_Trafficlight
{
    public class Duel_Mode
    {
        private readonly IntPtr cwObj = default;
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;

        public Duel_Mode()     //constructor initializes connection to device
        {
            try
            {
                cwObj = CwUSB.FCWInitObject();
                int devCount = CwUSB.FCWOpenCleware(cwObj);
                // Please note that OpenCleware should be called only once in the
                // initialisation of your programm, not every time a function is called

                Console.WriteLine("Found " + devCount + " Cleware devices");
                int devType = CwUSB.FCWGetUSBType(cwObj, 0);
                Console.WriteLine(devType);

                if (devType == (int)CwUSB.USBtype_enum.SWITCH1_DEVICE)
                {
                    //CwUSB.FCWSetLED(cwObj, 8, (int)CwUSB.LED_IDs.LED_1, 1);
                    int state = CwUSB.FCWGetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1); // for debugging
                    Console.WriteLine(state); //0 is off, 1 is on and -1 is a failure
                }

                if (devCount < 1)
                {
                    MessageBox.Show("No device connected!");
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Environment.Exit(1);
            }

            dt.Tick += new EventHandler(dt_tick); //calls delegate
            //ChangeTextboxStatus("Bitte Modus auswählen!"); //null pointer

        }

        public async Task<string> StartDuel(CancellationToken ct, int preparation_time, int pause_time, int shoot_time, int reps)
        {

            if (ct.IsCancellationRequested) // Is cancellation request already true
            {
                ct.ThrowIfCancellationRequested();
            }

            StartWatch(); // Show preparation time

            ChangeTextboxStatus("Vorbereitung");
            await OrangeOn(ct, preparation_time);

            //loop of 5 because of 5 rounds

            for (int i = 0; i < reps; i++)
            {
                ChangeTextboxStatus("Pause " + (i + 1));
                ResetWatch();
                StartWatch();
                await RedOn(ct, pause_time);

                ChangeTextboxStatus("Schuss " + (i + 1));
                ResetWatch();
                StartWatch();
                await GreenOn(ct, shoot_time);
            }


            ChangeTextboxStatus("");
            ResetWatch();
            StartWatch();
            //show red light after the last green phase
            await RedOn(ct, 7); //needed or can it be skipped;

            ResetWatch();
            StartWatch();
            // orange light on for 5 secs to signal ending
            ChangeTextboxStatus("Ende");
            await OrangeOn(ct, 5);

            StopWatch();
            ResetWatch();

            return "Fertig";

        }

        public async Task<string> ResetMode()
        {
            await Task.Run(() =>
            {
                ResetWatch();
                StopWatch();
                int devCount = CwUSB.FCWOpenCleware(cwObj);

                if (devCount >= 1)
                {
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
                }
            });
            return "reset done";
        }

        public async Task CloseConnection()
        {
            await Task.Run(() =>
            {
                StopWatch();
                int devCount = CwUSB.FCWOpenCleware(cwObj);

                if (devCount >= 1)
                {
                    CwUSB.FCWCloseCleware(cwObj);
                    CwUSB.FCWUnInitObject(cwObj);
                }
            });

        }


        // Next methods are single lights turned on for some time and then turned off. argument time in seconds.
        public async Task GreenOn(CancellationToken ct,int time)
        {
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 1);
            await Task.Delay(time * 1000, ct);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
        }

        public async Task OrangeOn(CancellationToken ct, int time)
        {
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 1);
            await Task.Delay(time * 1000, ct);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);
        }

        public async Task RedOn(CancellationToken ct, int time)
        {
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 1);
            await Task.Delay(time * 1000, ct);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);
        }

        void ChangeTextboxZeit(string txt)
        {
            MainWindow.AppWindow.Dispatcher.BeginInvoke(new Action(() => MainWindow.AppWindow.TextBox_Zeit.Text = txt));
        }

        void dt_tick(object sender, EventArgs e) //delegate
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = string.Format("{1:00}:{2:00}",
                ts.Minutes, ts.TotalSeconds, ts.Milliseconds / 10);
                ChangeTextboxZeit(string.Concat(currentTime));
            }
        }

        private void StartWatch()
        {
            sw.Start();
            dt.Start();
        }

        private void StopWatch()
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
        }

        private void ResetWatch()
        {
            sw.Reset();
            ChangeTextboxZeit("00:00");
        }

        private void ChangeTextboxStatus(string text)
        {
            MainWindow.AppWindow.Dispatcher.BeginInvoke(new Action(() => MainWindow.AppWindow.TextBox_Status.Text = text));
        }
    }
}
