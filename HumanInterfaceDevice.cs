//
//
//

namespace Hid
{
    /// <summary>
    /// From USB HID usage tables.
    /// http://www.usb.org/developers/hidpage#HID_Usage
    /// http://www.usb.org/developers/devclass_docs/Hut1_12v2.pdf
    /// </summary>
    public enum UsagePage : ushort
    {
        Undefined = 0,
        GenericDesktopControl,
        SimulationControl,
        VirtualRealityControl,
        SportControl,
        GameControl,
        GenericDeviceControl,
        Keyboard,
        LightEmittingDiode,
        Button,
        Ordinal,
        Telephony,
        Consumer,
        Digitiser,
        PhysicalInterfaceDevice = 0x0f,
        Unicode = 0x10,
        AlphaNumericDisplay = 0x14,
        MedicalInstruments = 0x40,
        MonitorPage0 = 0x80,
        MonitorPage1,
        MonitorPage2,
        MonitorPage3,
        PowerPage0,
        PowerPage1,
        PowerPage2,
        PowerPage3,
        BarCodeScanner = 0x8c,
        Scale,
        MagneticStripeReader,
        ReservedPointOfSale,
        CameraControl,
        Arcade,
        // http://msdn.microsoft.com/en-us/library/windows/desktop/bb417079.aspx
        MceRemote = 0xffbc,
        TerraTecRemote = 0xffcc
    }

    /// <summary>
/// 
/// </summary>
    public enum UsageId: ushort
    {
        MceRemoteUsage = 0x88,
        ConsumerControl = 0x01
    }



    namespace UsageTables
    {
        /// <summary>
        ///
        /// </summary>
        public enum MceButton: ushort
        {
            /// <summary>
            /// Not defined by the Microsoft specs.
            /// </summary>
            Null                    =   0x00,
            GreenStart              =   0x0D,
            ClosedCaptioning        =   0x2B,
            Teletext                =   0x5A,
            TeletextRed             =   0x5B,
            TeletextGreen           =   0x5C,
            TeletextYellow          =   0x5D,
            TeletextBlue            =   0x5E,
            LiveTv                  =   0x25,
            Music                   =   0x47,
            RecordedTv              =   0x48,
            Pictures                =   0x49,
            Videos                  =   0x4A,
            FmRadio                 =   0x50,
            Extras                  =   0x3C,
            ExtrasApp               =   0x3D,
            DvdMenu                 =   0x24,
            DvdAngle                =   0x4B,
            DvdAudio                =   0x4C,
            DvdSubtitle             =   0x4D,
            /// <summary>
            /// First press action: Ejects a DVD drive.
            /// <para />
            /// Second press action: Repeats first press action.
            /// <para />
            /// Notably issued by XBOX360 remote as defined in irplus - Remote Control - Android application.
            /// </summary>
            Eject                   =   0x28,
            DvdTopMenu              =   0x43,
            /// <summary>
            /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
            /// Collection (page 0xFFBC, usage 0x88).
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Visualization' button of HP Windows Media Center Remote (TSGH-IR08).
            /// <para />
            /// According to HP specs it displays visual imagery that is synchronized to the sound of your music tracks.
            /// </summary>
            Ext0                    =   0x32,
            /// <summary>
            /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
            /// Collection (page 0xFFBC, usage 0x88).
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Slide Show' button of HP Windows Media Center Remote (TSGH-IR08).
            /// <para />
            /// According to HP specs it plays a slide show of all the pictures on your hard disk drive.
            /// </summary>
            Ext1                    =   0x33,
            /// <summary>
            /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
            /// Collection (page 0xFFBC, usage 0x88).
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Eject' button of HP Windows Media Center Remote (TSGH-IR08).
            /// Also interpreted as 'Eject' action by SoundGraph iMON Manager in MCE mode (OrigenAE VF310).
            /// </summary>
            Ext2                    =   0x34,
            /// <summary>
            /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
            /// Collection (page 0xFFBC, usage 0x88).
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Input selection' button of HP Windows Media Center Remote (TSGH-IR08).
            /// </summary>
            Ext3                    =   0x35,
            Ext4                    =   0x36,
            Ext5                    =   0x37,
            Ext6                    =   0x38,
            Ext7                    =   0x39,
            Ext8                    =   0x3A,
            Ext9                    =   0x80,
            Ext10                   =   0x81,
            Ext11                   =   0x6F,
            Zoom                    =   0x27,
            ChannelInput            =   0x42,
            SubAudio                =   0x2D,
            Channel10               =   0x3E,
            Channel11               =   0x3F,
            Channel12               =   0x40,
            /// <summary>
            /// First press action: Generates OEM2 HID message in the Media Center Vendor Specific
            /// Collection. This button is intended to control the front panel display of home entertainment
            /// computers. When this button is pressed, the display could be turned on or off, or the display
            /// mode could change.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably issued by XBOX360 remote as defined in irplus - Remote Control - Android application.
            /// </summary>
            Display                 =   0x4F,
            /// <summary>
            /// First press action: To be determined.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// </summary>
            Kiosk                   =   0x6A,
            NetworkSelection        =   0x2C,
            BlueRayTool             =   0x78,
            ChannelInfo             =   0x41,
            VideoSelection          =   0x61
        }

        /// <summary>
        /// Those codes come from experimenting with HP remotes.
        /// </summary>
        public enum HpMceButton: ushort
        {
            /// <summary>
            /// Displays visual imagery that is synchronized to the sound of your music tracks.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Visualization' button of HP Windows Media Center Remote (TSGH-IR08).
            /// <para />
            /// According to HP specs it displays visual imagery that is synchronized to the sound of your music tracks.
            /// </summary>
            Visualization = MceButton.Ext0,
            /// <summary>
            /// Plays a slide show of all the pictures on your hard disk drive.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Slide Show' button of HP Windows Media Center Remote (TSGH-IR08).
            /// <para />
            /// According to HP specs it plays a slide show of all the pictures on your hard disk drive.
            /// </summary>
            SlideShow = MceButton.Ext1,
            /// <summary>
            /// Eject optical drive.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Eject' button of HP Windows Media Center Remote (TSGH-IR08).
            /// Also interpreted as 'Eject' action by SoundGraph iMON Manager in MCE mode (OrigenAE VF310).
            /// </summary>
            Eject = MceButton.Ext2,
            /// <summary>
            /// Not sure what this should do.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Input selection' button of HP Windows Media Center Remote (TSGH-IR08).
            /// </summary>
            InputSelection = MceButton.Ext3,
        }

        /// <summary>
        /// Usage Table for Consumer Controls
        /// 0x0C 0X01
        /// </summary>
        public enum ConsumerControl: ushort
        {
            /// <summary>
            /// Alternative code for properties.
            /// Also supported by Windows Media Center.
            /// </summary>
            MceProperties = 0x000A,
            /// <summary>
            /// Alternative code for program guide.
            /// Also supported by Windows Media Center.
            /// </summary>
            MceProgramGuide = 0x0012,
            /// <summary>
            /// Sent by MCE remotes.
            /// </summary>
            MediaSelectProgramGuide = 0x008D,
            /// <summary>
            /// Sent by MCE remotes.
            /// </summary>
            AppCtrlPrint = 0x0208,
            /// <summary>
            /// Sent by MCE remotes from the 'I' Informations or More Info buttons.
            /// </summary>
            AppCtrlProperties = 0x0209            
        }
    }
}