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
        GenericDesktopControls,
        SimulationControls,
        VirtualRealityControls,
        SportControls,
        GameControls,
        GenericDeviceControls,
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
        WindowsMediaCenterRemoteControl = 0xffbc,
        TerraTecRemote = 0xffcc
    }

    public enum UsageCollectionGenericDesktop : ushort
    {      
        Pointer = 0x01,
        Mouse = 0x02,
        Joystick = 0x04,
        GamePad = 0x05,
        Keyboard = 0x06,
        KeyPad = 0x07,
        MultiAxisController = 0x08,
        TabletPCSystemControls = 0x09,
        SystemControl = 0x80
    }

    public enum UsageCollectionConsumer : ushort
    {
        ConsumerControl = 0x01,
        NumericKeyPad = 0x02,
        ProgrammableButtons = 0x03,
        Microphone = 0x04,
        Headphone = 0x05,
        GraphicEqualizer = 0x06,
        FunctionButtons = 0x36,
        Selection = 0x80,
        MediaSelection = 0x0087,
        SelectDisc = 0x00BA,
        PlaybackSpeed = 0x00F1,
        Proximity = 0x0109,
        SpeakerSystem = 0x0160,
        ChannelLeft = 0x0161,
        ChannelRight = 0x0162,
        ChannelCenter = 0x0163,
        ChannelFront = 0x0164,
        ChannelCenterFront = 0x0165,
        ChannelSide = 0x0166,
        ChannelSurrond = 0x0167,
        ChannelLowFrequencyEnhancement = 0x0168,
        ChannelTop = 0x0169,
        ChannelUnknown = 0x016A,
        ApplicationLaunchButtons = 0x016A,
        GenericGuiApplicationControls = 0x0200,
    }

    
    public enum UsageCollectionWindowsMediaCenter: ushort
    {
        WindowsMediaCenterRemoteControl = 0x88        
    }



    namespace UsageTables
    {
        /// <summary>
        ///
        /// </summary>
        public enum WindowsMediaCenterRemoteControl: ushort
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
        public enum HpWindowsMediaCenterRemoteControl : ushort
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
            Visualization = WindowsMediaCenterRemoteControl.Ext0,
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
            SlideShow = WindowsMediaCenterRemoteControl.Ext1,
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
            HpEject = WindowsMediaCenterRemoteControl.Ext2,
            /// <summary>
            /// Not sure what this should do.
            /// <para />
            /// Second press action: Repeats message.
            /// <para />
            /// Auto-repeat: No
            /// <para />
            /// Notably sent by the 'Input selection' button of HP Windows Media Center Remote (TSGH-IR08).
            /// </summary>
            InputSelection = WindowsMediaCenterRemoteControl.Ext3,
        }

        /// <summary>
        /// Usage Table for Consumer Controls
        /// 0x0C 0X01
        /// </summary>
        public enum ConsumerControl: ushort
        {
            Null = 0x0000,           
            //           
            Channel = 0x0086,
            MediaSelectComputer = 0x0088,
            MediaSelectTV = 0x0089,
            MediaSelectWWW = 0x008A,
            MediaSelectDVD = 0x008B,
            MediaSelectTelephone = 0x008C,
            MediaSelectProgramGuide = 0x008D,
            MediaSelectVideoPhone = 0x008E,
            MediaSelectGames = 0x008F,
            MediaSelectMessages = 0x0090,
            MediaSelectCD = 0x0091,
            MediaSelectVCR = 0x0092,
            MediaSelectTuner = 0x0093,
            Quit = 0x0094,
            Help = 0x0095,
            MediaSelectTape = 0x0096,
            MediaSelectCable = 0x0097,
            MediaSelectSatellite = 0x0098,
            MediaSelectSecurity = 0x0099,
            MediaSelectHome = 0x009A,
            MediaSelectCall = 0x009B,
            ChannelIncrement = 0x009C,
            ChannelDecrement = 0x009D,
            MediaSelectSAP = 0x009E,
            //
            Play = 0x00B0,
            Pause = 0x00B1,
            Record = 0x00B2,
            FastForward = 0x00B3,
            Rewind = 0x00B4,
            ScanNextTrack = 0x00B5,
            ScanPreviousTrack = 0x00B6,
            Stop = 0x00B7,
            Eject = 0x00B8,
            RandomPlay = 0x00B9,            
            EnterDisc = 0x00BB,
            Repeat = 0x00BC,
            Tracking = 0x00BD,
            TrackNormal = 0x00BE,
            SlowTracking = 0x00BF,
            FrameForward = 0x00C0,
            FrameBack = 0x00C1,
            Mark = 0x00C2,
            ClearMark = 0x00C3,
            RepeatFromMark = 0x00C4,
            ReturnToMark = 0x00C5,
            SearchMarkForward = 0x00C6,
            SearchMarkBackwards = 0x00C7,
            CounterReset = 0x00C8,
            ShowCounter = 0x00C9,
            TrackingIncrement = 0x00CA,
            TrackingDecrement = 0x00CB,
            StopEject = 0x00CC,
            PlayPause = 0x00CD,
            PlaySkip = 0x00CE,

            /// <summary>
            /// Audio controls
            /// </summary>
            Volume = 0x00E0,
            Balance = 0x00E1,
            Mute = 0x00E2,
            Bass = 0x00E3,
            Treble = 0x00E4,
            BassBoost = 0x00E5,
            SurroundMode = 0x00E6,
            Loudness = 0x00E7,
            MPX = 0x00E8,
            VolumeIncrement = 0x00E9,
            VolumeDecrement = 0x00EA,

            //Generic GUI Application Controls
            //GenericGUIApplicationControls = 0x0200,
            AppCtrlNew = 0x0201,
            AppCtrlOpen = 0x0202,
            AppCtrlClose = 0x0203,
            AppCtrlExit = 0x0204,
            AppCtrlMaximize = 0x0205,
            AppCtrlMinimize = 0x0206,
            AppCtrlSave = 0x0207,
            AppCtrlPrint = 0x0208,
            AppCtrlProperties = 0x0209,
            AppCtrlUndo = 0x021A,
            AppCtrlCopy = 0x021B,
            AppCtrlCut = 0x021C,
            AppCtrlPaste = 0x021D,
            AppCtrlSelectAll = 0x021E,
            AppCtrlFind = 0x021F,
            AppCtrlFindAndReplace = 0x0220,
            AppCtrlSearch = 0x0221,
            AppCtrlGoTo = 0x0222,
            AppCtrlHome = 0x0223,
            AppCtrlBack = 0x0224,
            AppCtrlForward = 0x0225,
            AppCtrlStop = 0x0226,
            AppCtrlRefresh = 0x0227,
            AppCtrlPreviousLink = 0x0228,
            AppCtrlNextLink = 0x0229,
            AppCtrlBookmarks = 0x022A,
            AppCtrlHistory = 0x022B,
            AppCtrlSubscriptions = 0x022C,
            AppCtrlZoomIn = 0x022D,
            AppCtrlZoomOut = 0x022E,
            AppCtrlZoom = 0x022F,
            AppCtrlFullScreenView = 0x0230,
            AppCtrlNormalView = 0x0231,
            AppCtrlViewToggle = 0x0232,
            AppCtrlScrollUp = 0x0233,
            AppCtrlScrollDown = 0x0234,
            AppCtrlScroll = 0x0235,
            AppCtrlPanLeft = 0x0236,
            AppCtrlPanRight = 0x0237,
            AppCtrlPan = 0x0238,
            AppCtrlNewWindow = 0x0239,
            AppCtrlTileHorizontally = 0x023A,
            AppCtrlTileVertically = 0x023B,
            AppCtrlFormat = 0x023C,
            AppCtrlEdit = 0x023D,
        }
    }
}