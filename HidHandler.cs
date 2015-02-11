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
    public class HidHandler
    {
        public delegate void HidEventHandler(object aSender, HidEvent aHidEvent);
        public event HidEventHandler OnHidEvent;
        List<HidEvent> iHidEvents;


        public bool IsRegistered { get; private set; }

        public HidHandler(RAWINPUTDEVICE[] aRawInputDevices)
        {
            iHidEvents=new List<HidEvent>();
            IsRegistered = Function.RegisterRawInputDevices(aRawInputDevices, (uint)aRawInputDevices.Length, (uint)Marshal.SizeOf(aRawInputDevices[0]));
        }

        public void ProcessInput(Message aMessage)
        {
            Hid.HidEvent hidEvent = new Hid.HidEvent(aMessage, OnHidEventRepeat);
            hidEvent.DebugWrite();

            if (!hidEvent.IsValid || !hidEvent.IsGeneric)
            {
                Debug.WriteLine("Skipping HID message.");
                return;
            }

            //
            if (hidEvent.IsButtonUp)
            {
                //This is a key up event
                //We need to discard any events belonging to the same page and collection
                for (int i = (iHidEvents.Count-1); i >= 0; i--)
                {
                    if (iHidEvents[i].UsageId == hidEvent.UsageId)
                    {
                        iHidEvents[i].Dispose();
                        iHidEvents.RemoveAt(i);
                    }
                }
            }
            else
            {
                //Keep that event until we get a key up message
                iHidEvents.Add(hidEvent);
            }

            //Broadcast our events
            OnHidEvent(this, hidEvent);    
        }

        public void OnHidEventRepeat(HidEvent aHidEvent)
        {
            //Broadcast our events
            OnHidEvent(this, aHidEvent);    
        }

    }

}