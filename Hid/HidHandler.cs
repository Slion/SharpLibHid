//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpLibHid.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//


using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using SharpLib.Win32;
using System.Collections.Generic;


namespace SharpLib.Hid
{
    /// <summary>
    /// Our HID handler manages raw input registrations, processes WM_INPUT messages and broadcasts HID events in return.
    /// </summary>
    public class Handler : IDisposable
    {
        public delegate void HidEventHandler(object aSender, Event aHidEvent);
        public event HidEventHandler OnHidEvent;
        List<Event> iHidEvents;
        RAWINPUTDEVICE[] iRawInputDevices;


        public bool IsRegistered { get; private set; }
        public bool ManageRepeats { get; private set; }

        public Handler(RAWINPUTDEVICE[] aRawInputDevices, bool aManageRepeats=false)
        {
            iRawInputDevices = aRawInputDevices;
            iHidEvents = new List<Event>();
            IsRegistered = Function.RegisterRawInputDevices(iRawInputDevices, (uint)iRawInputDevices.Length, (uint)Marshal.SizeOf(iRawInputDevices[0]));
            ManageRepeats = aManageRepeats;
        }

        /// <summary>
        /// Will de-register devices.
        /// </summary>
        public void Dispose()
        {
            //Setup device removal
            for (int i=0; i<iRawInputDevices.Length; i++)
            {
                iRawInputDevices[i].dwFlags = Const.RIDEV_REMOVE;
                iRawInputDevices[i].hwndTarget = IntPtr.Zero;
            }

            //De-register
            Function.RegisterRawInputDevices(iRawInputDevices, (uint)iRawInputDevices.Length, (uint)Marshal.SizeOf(iRawInputDevices[0]));
            IsRegistered = false;
        }

        /// <summary>
        /// Process a WM_INPUT message.
        /// </summary>
        /// <param name="aMessage"></param>
        public void ProcessInput(ref Message aMessage)
        {
            if (aMessage.Msg != Const.WM_INPUT)
            {
                //We only process WM_INPUT messages
                return;
            }

            Event hidEvent = new Event(aMessage, OnHidEventRepeat, ManageRepeats);
            hidEvent.DebugWrite();

            if (!hidEvent.IsValid || !hidEvent.IsGeneric)
            {
                Debug.WriteLine("Skipping HID message.");
                return;
            }

            //
            if (ManageRepeats)
            {
                if (hidEvent.IsButtonUp)
                {
                    //This is a key up event
                    //We need to discard any events belonging to the same page and collection
                    for (int i = (iHidEvents.Count - 1); i >= 0; i--)
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
            }

            //Broadcast our events
            OnHidEvent(this, hidEvent);    
        }

        public void OnHidEventRepeat(Event aHidEvent)
        {
            //Broadcast our events
            OnHidEvent(this, aHidEvent);    
        }

    }

}