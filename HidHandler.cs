using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Win32;
using System.Collections.Generic;


namespace Hid
{


    /// <summary>
    /// Our HID handler manages raw input registrations, processes WM_INPUT messages and broadcasts HID events in return.
    /// </summary>
    class HidHandler
    {
        public delegate void HidEventHandler(object aSender, HidEvent aHidEvent);
        public event HidEventHandler OnHidEvent;

        public bool IsRegistered { get; private set; }

        public HidHandler(RAWINPUTDEVICE[] aRawInputDevices)
        {
            IsRegistered = Function.RegisterRawInputDevices(aRawInputDevices, (uint)aRawInputDevices.Length, (uint)Marshal.SizeOf(aRawInputDevices[0]));
        }


        public void ProcessInput(Message aMessage)
        {
            Hid.HidEvent hidEvent = new Hid.HidEvent(aMessage);
            hidEvent.DebugWrite();

            if (!hidEvent.IsValid || !hidEvent.IsGeneric)
            {
                Debug.WriteLine("Skipping HID message.");
                return;
            }

            //Broadcast our events
            OnHidEvent(this, hidEvent);    
        }

    }

}