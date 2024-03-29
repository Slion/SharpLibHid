//
// Copyright (C) 2014-2015 St�phane Lenclud.
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
using System.Runtime.InteropServices;
using System.Diagnostics;
using SharpLib.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        //TODO: Consider using only the VirtualKey as key since we now resolve left and right modifiers internally
        Dictionary<ulong, Event> iKeyDownEvents;
        RAWINPUTDEVICE[] iRawInputDevices;
        Dictionary<IntPtr, Device.Input> iDevices = new Dictionary<IntPtr, Device.Input>();

        public bool IsRegistered { get; private set; }
        public bool ManageRepeats { get; private set; }
        public int RepeatDelayInMs { get; private set; }
        public int RepeatSpeedInMs { get; private set; }

        /// <summary>
        /// Provide a collection of keyboard keys currently pushed down.
        /// </summary>
        public Dictionary<ulong, Event> KeyDownEvents {
            get { return iKeyDownEvents;}
        }

        /// <summary>
        /// Create an HID handler.
        /// Registers the provided input devices.
        /// </summary>
        /// <param name="aRawInputDevices">List of HID devices to receive input from.</param>
        /// <param name="aManageRepeats">Specify if we want to our handler to manage event repeats for our application.</param>
        /// <param name="aRepeatDelayInMs">The initial delay in milliseconds before an event starts repeating.</param>
        /// <param name="aRepeatSpeedInMs">The delay in milliseconds between an event repeat notification past the first one.</param>
        public Handler(RAWINPUTDEVICE[] aRawInputDevices, bool aManageRepeats = false, int aRepeatDelayInMs = -1, int aRepeatSpeedInMs = -1)
        {
            //Initialize
            Init(aManageRepeats, aRepeatDelayInMs, aRepeatSpeedInMs);
            //Register for WM_INPUT
            iRawInputDevices = aRawInputDevices;
            IsRegistered = Function.RegisterRawInputDevices(iRawInputDevices, (uint)iRawInputDevices.Length, (uint)Marshal.SizeOf(iRawInputDevices[0]));
        }

        /// <summary>
        /// Create an HID handler without registering for WM_INPUT messages.
        /// This is useful in a context where we already registered for WM_INPUT messages through another SharpLib.Hid.Handler instance or otherwise. 
        /// One can then still use this Handler to parse incoming WM_INPUT messages.
        /// </summary>
        /// <param name="aManageRepeats">Specify if we want to our handler to manage event repeats for our application.</param>
        /// <param name="aRepeatDelayInMs">The initial delay in milliseconds before an event starts repeating.</param>
        /// <param name="aRepeatSpeedInMs">The delay in milliseconds between an event repeat notification past the first one.</param>
        public Handler(bool aManageRepeats = false, int aRepeatDelayInMs = -1, int aRepeatSpeedInMs = -1)
        {
            Init(aManageRepeats, aRepeatDelayInMs, aRepeatSpeedInMs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aManageRepeats"></param>
        /// <param name="aRepeatDelayInMs"></param>
        /// <param name="aRepeatSpeedInMs"></param>
        private void Init(bool aManageRepeats, int aRepeatDelayInMs, int aRepeatSpeedInMs)
        {
            iRawInputDevices = new RAWINPUTDEVICE[0];
            iHidEvents = new List<Event>();
            iKeyDownEvents = new Dictionary<ulong, Event>();
            IsRegistered = false;
            ManageRepeats = aManageRepeats;
            RepeatDelayInMs = aRepeatDelayInMs;
            RepeatSpeedInMs = aRepeatSpeedInMs;
        }

        /// <summary>
        /// Will de-register devices.
        /// </summary>
        public void Dispose()
        {
            //Free up cached devices if any
            foreach (Device.Input d in iDevices.Values)
            {
                d.Dispose();
            }

            iDevices.Clear();


            if (!IsRegistered)
            {
                return;
            }

            //Setup device removal
            for (int i=0; i<iRawInputDevices.Length; i++)
            {
                iRawInputDevices[i].dwFlags = RawInputDeviceFlags.RIDEV_REMOVE;
                iRawInputDevices[i].hwndTarget = IntPtr.Zero;
            }

            //De-register
            Function.RegisterRawInputDevices(iRawInputDevices, (uint)iRawInputDevices.Length, (uint)Marshal.SizeOf(iRawInputDevices[0]));
            IsRegistered = false;
        }


        /// <summary>
        /// Enable caching of HID devices.
        /// In fact our work on Oculus Rift S has shown creating our device is rather slow.
        /// At least in respect to Oculus Rift S high frequency HID events.
        /// </summary>
        /// <param name="aHandle">Handle of the HID device we need to fetch</param>
        /// <returns></returns>
        public Device.Input FetchDevice(IntPtr aHandle)
        {
            Device.Input device = null;

            // Try fetch matching device from our cache
            if (!iDevices.TryGetValue(aHandle, out device))
            {   
                // Device not found in our cache, just create it then
                device = new Device.Input(aHandle);
                // Then make sure we cache it
                iDevices.Add(aHandle, device);
            }

            return device;
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

            Event hidEvent = new Event(this, aMessage, OnHidEventRepeat, ManageRepeats, RepeatDelayInMs, RepeatSpeedInMs);
            //hidEvent.DebugWrite();

            if (!hidEvent.IsValid /*|| !hidEvent.IsGeneric*/)
            {
                Debug.WriteLine("Skipping HID message.");
                return;
            }

            //We want to repeat only a single event at a time.
            //Any other event will interrupt the current repeat.
            if (ManageRepeats && hidEvent.IsGeneric)
            {
                //Discard all outstanding repeats, though we should only ever have only one
                for (int i = (iHidEvents.Count - 1); i >= 0; i--)
                {
                        iHidEvents[i].Dispose();
                        iHidEvents.RemoveAt(i);
                }
                //Add our newly created event to our repeat list
                //TODO: instead of a list we could now have a single event since we only support one repeat at a time
                iHidEvents.Add(hidEvent);
            }
            else if (hidEvent.IsKeyboard)
            {
                hidEvent.HasModifierShift = !hidEvent.IsModifier && HasModifierShift;
                hidEvent.HasModifierControl = !hidEvent.IsModifier && HasModifierControl;
                hidEvent.HasModifierAlt = !hidEvent.IsModifier && HasModifierAlt;
                hidEvent.HasModifierWindows = !hidEvent.IsModifier && HasModifierWindows;

                //We are dealing with a keyboard event 
                if (hidEvent.IsButtonDown)
                {
                    Event previous;
                    if (iKeyDownEvents.TryGetValue(hidEvent.KeyId, out previous))
                    {
                        // This key is already pushed down
                        // Increment our repeat count
                        hidEvent.RepeatCount = previous.RepeatCount+1;
                        // Discard our previous instance
                        previous.Dispose();           
                    }

                    //Add or update our key in our dictionary
                    iKeyDownEvents[hidEvent.KeyId] = hidEvent;
                }
                else if (hidEvent.IsButtonUp)
                {
                    Event previous;
                    if (iKeyDownEvents.TryGetValue(hidEvent.KeyId, out previous))
                    {
                        // Key was released just make sure we track that by removing it
                        previous.Dispose();
                        iKeyDownEvents.Remove(hidEvent.KeyId);
                    }                    
                }
            }

            // Broadcast our events
            // Filter out keyboard repeats unless otherwise specified.
            // Only keyboard repeats coming from drivers come through here.
            if (ManageRepeats || !hidEvent.IsRepeat)
            {
                OnHidEvent(this, hidEvent);
            }            
        }

        public void OnHidEventRepeat(Event aHidEvent)
        {
            // Generic HID repeats generated using timer go through here.  
            // Broadcast our events
            OnHidEvent(this, aHidEvent);    
        }

        /// <summary>
        /// Checks if SHIFT modifier is enabled.
        /// </summary>
        public bool HasModifierShift
        {
            get
            {
                return iKeyDownEvents.Values.Any(hidEvent => hidEvent.IsModifierShift);
            }
        }

        /// <summary>
        /// Checks if CTRL modifier is enabled.
        /// </summary>
        public bool HasModifierControl
        {
            get
            {
                return iKeyDownEvents.Values.Any(hidEvent => hidEvent.IsModifierControl);
            }
        }

        /// <summary>
        /// Checks if ALT modifier is enabled.
        /// </summary>
        public bool HasModifierAlt
        {
            get
            {
                return iKeyDownEvents.Values.Any(hidEvent => hidEvent.IsModifierAlt);
            }
        }

        /// <summary>
        /// Checks if WINDOWS modifier is enabled.
        /// </summary>
        public bool HasModifierWindows
        {
            get
            {
                return iKeyDownEvents.Values.Any(hidEvent => hidEvent.IsModifierWindows);
            }
        }


    }

}