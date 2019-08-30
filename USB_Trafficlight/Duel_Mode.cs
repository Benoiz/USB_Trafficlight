using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;

namespace USB_Trafficlight
{
    public class Duel_Mode
    {
        public void InitiateDuelMode()
        {
            try
            {
                IntPtr cwObj = CwUSB.FCWInitObject();
                int devCount = CwUSB.FCWOpenCleware(cwObj);
                // Please note that OpenCleware should be called only once in the
                // initialisation of your programm, not every time a function is called
                Console.WriteLine("Found " + devCount + " Cleware devices");
                int devType = CwUSB.FCWGetUSBType(cwObj, 0);
                Console.WriteLine(devType );

                if (devType == (int)CwUSB.USBtype_enum.SWITCH1_DEVICE)
                {
                    //CwUSB.FCWSetLED(cwObj, 8, (int)CwUSB.LED_IDs.LED_1, 1);
                    int state = CwUSB.FCWGetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1); // for debugging
                    Console.WriteLine(state); //0 is off, 1 is on and -1 is a failure
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 1); //turning the orange light on
                    System.Threading.Thread.Sleep(60000);
                    CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);
                    for (int i = 0; i < 4; i++)
                    {
                        CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 1); //turning the red light on
                        System.Threading.Thread.Sleep(7000);
                        CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);
                        CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 1); //turning the green light on
                        System.Threading.Thread.Sleep(3000);
                        CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
                        
                    }
                }

                if (devCount >= 1)
                {
                    CwUSB.FCWCloseCleware(cwObj); // freeing the object
                    CwUSB.FCWUnInitObject(cwObj);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void ResetMode()
        {
            IntPtr cwObj = CwUSB.FCWInitObject();
            //int devCount = CwUSB.FCWOpenCleware(cwObj);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_0, 0);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_1, 0);
            CwUSB.FCWSetSwitch(cwObj, 0, (int)CwUSB.SWITCH_IDs.SWITCH_2, 0);
        }
    }
}
