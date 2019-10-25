using System;
using System.Runtime.InteropServices;

//This class comes from the cleware homepage. It imports the needed "USBAccess.dll"

namespace USB_Trafficlight
{
    class CwUSB
    {
        public enum SWITCH_IDs : int
        {
            SWITCH_0 = 0x10, SWITCH_1 = 0x11, SWITCH_2 = 0x12, SWITCH_3 = 0x13,
            SWITCH_4 = 0x14, SWITCH_5 = 0x15, SWITCH_6 = 0x16, SWITCH_7 = 0x17,
            SWITCH_8 = 0x18, SWITCH_9 = 0x19, SWITCH_10 = 0x1a, SWITCH_11 = 0x1b,
            SWITCH_12 = 0x1c, SWITCH_13 = 0x1d, SWITCH_14 = 0x1e, SWITCH_15 = 0x1f
        };
        public enum LED_IDs { LED_0 = 0, LED_1 = 1, LED_2 = 2, LED_3 = 3 };
        public enum USBtype_enum : int
        {
            ILLEGAL_DEVICE = 0,
            LED_DEVICE = 0x01,
            POWER_DEVICE = 0x02,
            DISPLAY_DEVICE = 0x03,
            WATCHDOG_DEVICE = 0x05,
            AUTORESET_DEVICE = 0x06,
            WATCHDOGXP_DEVICE = 0x07,
            SWITCH1_DEVICE = 0x08, SWITCH2_DEVICE = 0x09, SWITCH3_DEVICE = 0x0a,
            SWITCH4_DEVICE = 0x0b, SWITCH5_DEVICE = 0x0c, SWITCH6_DEVICE = 0x0d,
            SWITCH7_DEVICE = 0x0e, SWITCH8_DEVICE = 0x0f,
            TEMPERATURE_DEVICE = 0x10, TEMPERATURE2_DEVICE = 0x11,
            TEMPERATURE5_DEVICE = 0x15,
            HUMIDITY1_DEVICE = 0x20, HUMIDITY2_DEVICE = 0x21,
            SWITCHX_DEVICE = 0x28, // new switch 3,4,8
            CONTACT00_DEVICE = 0x30, CONTACT01_DEVICE = 0x31,
            ADC0800_DEVICE = 0x50, ADC0801_DEVICE = 0x51
        };
        [DllImport(@"USBaccess.DLL")]
        public static extern IntPtr FCWInitObject();
        [DllImport(@"USBaccess.DLL")]
        public static extern void FCWUnInitObject(IntPtr cwHdl);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWOpenCleware(IntPtr cwHdl);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWCloseCleware(IntPtr cwHdl);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWGetUSBType(IntPtr cwHdl, int devNum);
        [DllImport(@"USBaccess.DLL")]
        public static extern float FCWDGetTemperature(IntPtr cwHdl, int devNum);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWSetLED(IntPtr cwHdl, int devNum, int Led, int value);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWSetSwitch(IntPtr cwHdl, int dvNum, int Switch, int On);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWGetSwitch(IntPtr cwHdl, int devNum, int Switch);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWSelectADC(IntPtr cwHdl, int devNum, int subDevice);
        [DllImport(@"USBaccess.DLL")]
        public static extern float FCWGetADC(IntPtr cwHdl, int dNum, int sNum, int subDev);
        [DllImport(@"USBaccess.DLL")]
        public static extern int FCWStartDevice(IntPtr cwHdl, int devNum);
    }
}
