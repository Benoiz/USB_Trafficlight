using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Threading;

namespace USB_Trafficlight
{
    public class Duel_Mode
    {
        private readonly IntPtr cwObj = default;

        private const int PREPARATION_TIME = 6000;
        private const int AVERTED_TIME = 7000;
        private const int FACED_TIME = 3000;

        public Duel_Mode()     //contructor that is called when class is intantiated
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
        }

         public async Task InitiateDuelMode(CancellationToken ct)
        {
            try
            {   
                ct.Register(() =>
                {
                    try
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        throw new OperationCanceledException(ct);
                    }
                });

                CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 1); //turning the orange light on
                await Task.Delay(PREPARATION_TIME);
                CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);

                //loop of 5 because of 5 rounds

                for (int i = 0; i < 4; i++)
                {
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 1); //turning the red light on
                    await Task.Delay(AVERTED_TIME);
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);

                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 1); //turning the green light on
                    await Task.Delay(FACED_TIME);
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ResetMode()
        {
            int devCount = CwUSB.FCWOpenCleware(cwObj);

            if (devCount >= 1)
            {
                CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);
                CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);
                CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
            }
        }

        public void CloseConnection()
        {
            int devCount = CwUSB.FCWOpenCleware(cwObj);

            if (devCount >= 1)
            {
                CwUSB.FCWCloseCleware(cwObj);
                CwUSB.FCWUnInitObject(cwObj);
            }
        }
    }
}
