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
        public enum ConsumerControl : ushort
        {
            Null = 0x0000,
            //
            ConsumerControl = 0x01,
            NumericKeyPad = 0x02,
            ProgrammableButtons = 0x03,
            Microphone = 0x04,
            Headphone = 0x05,
            GraphicEqualizer = 0x06,
            Plus10 = 0x20,
            Plus100 = 0x21,
            AmPm = 0x22,
            Power = 0x30,
            Reset = 0x31,
            Sleep = 0x32,
            SleepAfter = 0x33,
            SleepMode = 0x34,
            Illumination = 0x35,
            FunctionButtons = 0x36,
            Menu = 0x40,
            MenuPick = 0x41,
            MenuUp = 0x42,
            MenuDown = 0x43,
            MenuLeft = 0x44,
            MenuRight = 0x45,
            MenuEscape = 0x46,
            MenuValueIncrease = 0x47,
            MenuValueDecrease = 0x48,
            DataOnScreen = 0x60,
            ClosedCaption = 0x61,
            ClosedCaptionSelect = 0x62,
            VcrTv = 0x63,
            BroadcastMode = 0x64,
            Snapshot = 0x65,
            Still = 0x66,
            Selection = 0x80,
            AssignSelection = 0x81,
            ModeStep = 0x82,
            RecallLast = 0x83,
            EnterChannel = 0x84,
            OrderMovie = 0x85,
            Channel = 0x86,
            MediaSelection = 0x87,
            MediaSelectComputer = 0x88,
            MediaSelectTv = 0x89,
            MediaSelectWww = 0x8A,
            MediaSelectDvd = 0x8B,
            MediaSelectTelephone = 0x8C,
            MediaSelectProgramGuide = 0x8D,
            MediaSelectVideoPhone = 0x8E,
            MediaSelectGames = 0x8F,
            MediaSelectMessages = 0x90,
            MediaSelectCd = 0x91,
            MediaSelectVcr = 0x92,
            MediaSelectTuner = 0x93,
            Quit = 0x94,
            Help = 0x95,
            MediaSelectTape = 0x96,
            MediaSelectCable = 0x97,
            MediaSelectSatellite = 0x98,
            MediaSelectSecurity = 0x99,
            MediaSelectHome = 0x9A,
            MediaSelectCall = 0x9B,
            ChannelIncrement = 0x9C,
            ChannelDecrement = 0x9D,
            MediaSelectSap = 0x9E,
            VcrPlus = 0xA0,
            Once = 0xA1,
            Daily = 0xA2,
            Weekly = 0xA3,
            Monthly = 0xA4,
            Play = 0xB0,
            Pause = 0xB1,
            Record = 0xB2,
            FastForward = 0xB3,
            Rewind = 0xB4,
            ScanNextTrack = 0xB5,
            ScanPreviousTrack = 0xB6,
            Stop = 0xB7,
            Eject = 0xB8,
            RandomPlay = 0xB9,
            SelectDisc = 0xBA,
            EnterDisc = 0xBB,
            Repeat = 0xBC,
            Tracking = 0xBD,
            TrackNormal = 0xBE,
            SlowTracking = 0xBF,
            FrameForward = 0xC0,
            FrameBack = 0xC1,
            Mark = 0xC2,
            ClearMark = 0xC3,
            RepeatFromMark = 0xC4,
            ReturnToMark = 0xC5,
            SearchMarkForward = 0xC6,
            SearchMarkBackwards = 0xC7,
            CounterReset = 0xC8,
            ShowCounter = 0xC9,
            TrackingIncrement = 0xCA,
            TrackingDecrement = 0xCB,
            StopEject = 0xCC,
            PlayPause = 0xCD,
            PlaySkip = 0xCE,
            Volume = 0xE0,
            Balance = 0xE1,
            Mute = 0xE2,
            Bass = 0xE3,
            Treble = 0xE4,
            BassBoost = 0xE5,
            SurroundMode = 0xE6,
            Loudness = 0xE7,
            Mpx = 0xE8,
            VolumeIncrement = 0xE9,
            VolumeDecrement = 0xEA,
            SpeedSelect = 0xF0,
            PlaybackSpeed = 0xF1,
            StandardPlay = 0xF2,
            LongPlay = 0xF3,
            ExtendedPlay = 0xF4,
            Slow = 0xF5,
            FanEnable = 0x100,
            FanSpeed = 0x101,
            LightEnable = 0x102,
            LightIlluminationLevel = 0x103,
            ClimateControlEnable = 0x104,
            RoomTemperature = 0x105,
            SecurityEnable = 0x106,
            FireAlarm = 0x107,
            PoliceAlarm = 0x108,
            Proximity = 0x109,
            Motion = 0x10A,
            DuressAlarm = 0x10B,
            HoldupAlarm = 0x10C,
            MedicalAlarm = 0x10D,
            BalanceRight = 0x150,
            BalanceLeft = 0x151,
            BassIncrement = 0x152,
            BassDecrement = 0x153,
            TrebleIncrement = 0x154,
            TrebleDecrement = 0x155,
            SpeakerSystem = 0x160,
            ChannelLeft = 0x161,
            ChannelRight = 0x162,
            ChannelCenter = 0x163,
            ChannelFront = 0x164,
            ChannelCenterFront = 0x165,
            ChannelSide = 0x166,
            ChannelSurround = 0x167,
            ChannelLowFrequencyEnhancement = 0x168,
            ChannelTop = 0x169,
            ChannelUnknown = 0x16A,
            SubChannel = 0x170,
            SubChannelIncrement = 0x171,
            SubChannelDecrement = 0x172,
            AlternateAudioIncrement = 0x173,
            AlternateAudioDecrement = 0x174,
            ApplicationLaunchButtons = 0x180,
            AppLaunchLaunchButtonConfigurationTool = 0x181,
            AppLaunchProgrammableButtonConfiguration = 0x182,
            AppLaunchConsumerControlConfiguration = 0x183,
            AppLaunchWordProcessor = 0x184,
            AppLaunchTextEditor = 0x185,
            AppLaunchSpreadsheet = 0x186,
            AppLaunchGraphicsEditor = 0x187,
            AppLaunchPresentationApp = 0x188,
            AppLaunchDatabaseApp = 0x189,
            AppLaunchEmailReader = 0x18A,
            AppLaunchNewsreader = 0x18B,
            AppLaunchVoicemail = 0x18C,
            AppLaunchContactsAddressBook = 0x18D,
            AppLaunchCalendarSchedule = 0x18E,
            AppLaunchTaskProjectManager = 0x18F,
            AppLaunchLogJournalTimecard = 0x190,
            AppLaunchCheckbookFinance = 0x191,
            AppLaunchCalculator = 0x192,
            AppLaunchAVCapturePlayback = 0x193,
            AppLaunchLocalMachineBrowser = 0x194,
            AppLaunchLanWanBrowser = 0x195,
            AppLaunchInternetBrowser = 0x196,
            AppLaunchRemoteNetworkingIspConnect = 0x197,
            AppLaunchNetworkConference = 0x198,
            AppLaunchNetworkChat = 0x199,
            AppLaunchTelephonyDialer = 0x19A,
            AppLaunchLogon = 0x19B,
            AppLaunchLogoff = 0x19C,
            AppLaunchLogonLogoff = 0x19D,
            AppLaunchTerminalLockScreensaver = 0x19E,
            AppLaunchControlPanel = 0x19F,
            AppLaunchCommandLineProcessorRun = 0x1A0,
            AppLaunchProcessTaskManager = 0x1A1,
            AppLaunchSelectTaskApplication = 0x1A2,
            AppLaunchNextTaskApplication = 0x1A3,
            AppLaunchPreviousTaskApplication = 0x1A4,
            AppLaunchPreemptiveHaltTaskApplication = 0x1A5,
            AppLaunchIntegratedHelpCenter = 0x1A6,
            AppLaunchDocuments = 0x1A7,
            AppLaunchThesaurus = 0x1A8,
            AppLaunchDictionary = 0x1A9,
            AppLaunchDesktop = 0x1AA,
            AppLaunchSpellCheck = 0x1AB,
            AppLaunchGrammarCheck = 0x1AC,
            AppLaunchWirelessStatus = 0x1AD,
            AppLaunchKeyboardLayout = 0x1AE,
            AppLaunchVirusProtection = 0x1AF,
            AppLaunchEncryption = 0x1B0,
            AppLaunchScreenSaver = 0x1B1,
            AppLaunchAlarms = 0x1B2,
            AppLaunchClock = 0x1B3,
            AppLaunchFileBrowser = 0x1B4,
            AppLaunchPowerStatus = 0x1B5,
            AppLaunchImageBrowser = 0x1B6,
            AppLaunchAudioBrowser = 0x1B7,
            AppLaunchMovieBrowser = 0x1B8,
            AppLaunchDigitalRightsManager = 0x1B9,
            AppLaunchDigitalWallet = 0x1BA,
            AppLaunchInstantMessaging = 0x1BC,
            AppLaunchOemFeaturesTipsTutorialBrowser = 0x1BD,
            AppLaunchOemHelp = 0x1BE,
            AppLaunchOnlineCommunity = 0x1BF,
            AppLaunchEntertainmentContentBrowser = 0x1C0,
            AppLaunchOnlineShoppingBrowser = 0x1C1,
            AppLaunchSmartcardInformationHelp = 0x1C2,
            AppLaunchMarketMonitorFinanceBrowser = 0x1C3,
            AppLaunchCustomizedCorporateNewsBrowser = 0x1C4,
            AppLaunchOnlineActivityBrowser = 0x1C5,
            AppLaunchResearchSearchBrowser = 0x1C6,
            AppLaunchAudioPlayer = 0x1C7,
            GenericGuiApplicationControls = 0x200,
            AppCtrlNew = 0x201,
            AppCtrlOpen = 0x202,
            AppCtrlClose = 0x203,
            AppCtrlExit = 0x204,
            AppCtrlMaximize = 0x205,
            AppCtrlMinimize = 0x206,
            AppCtrlSave = 0x207,
            AppCtrlPrint = 0x208,
            AppCtrlProperties = 0x209,
            AppCtrlUndo = 0x21A,
            AppCtrlCopy = 0x21B,
            AppCtrlCut = 0x21C,
            AppCtrlPaste = 0x21D,
            AppCtrlSelectAll = 0x21E,
            AppCtrlFind = 0x21F,
            AppCtrlFindAndReplace = 0x220,
            AppCtrlSearch = 0x221,
            AppCtrlGoTo = 0x222,
            AppCtrlHome = 0x223,
            AppCtrlBack = 0x224,
            AppCtrlForward = 0x225,
            AppCtrlStop = 0x226,
            AppCtrlRefresh = 0x227,
            AppCtrlPreviousLink = 0x228,
            AppCtrlNextLink = 0x229,
            AppCtrlBookmarks = 0x22A,
            AppCtrlHistory = 0x22B,
            AppCtrlSubscriptions = 0x22C,
            AppCtrlZoomIn = 0x22D,
            AppCtrlZoomOut = 0x22E,
            AppCtrlZoom = 0x22F,
            AppCtrlFullScreenView = 0x230,
            AppCtrlNormalView = 0x231,
            AppCtrlViewToggle = 0x232,
            AppCtrlScrollUp = 0x233,
            AppCtrlScrollDown = 0x234,
            AppCtrlScroll = 0x235,
            AppCtrlPanLeft = 0x236,
            AppCtrlPanRight = 0x237,
            AppCtrlPan = 0x238,
            AppCtrlNewWindow = 0x239,
            AppCtrlTileHorizontally = 0x23A,
            AppCtrlTileVertically = 0x23B,
            AppCtrlFormat = 0x23C,
            AppCtrlEdit = 0x23D,
            AppCtrlBold = 0x23E,
            AppCtrlItalics = 0x23F,
            AppCtrlUnderline = 0x240,
            AppCtrlStrikethrough = 0x241,
            AppCtrlSubscript = 0x242,
            AppCtrlSuperscript = 0x243,
            AppCtrlAllCaps = 0x244,
            AppCtrlRotate = 0x245,
            AppCtrlResize = 0x246,
            AppCtrlFlipHorizontal = 0x247,
            AppCtrlFlipVertical = 0x248,
            AppCtrlMirrorHorizontal = 0x249,
            AppCtrlMirrorVertical = 0x24A,
            AppCtrlFontSelect = 0x24B,
            AppCtrlFontColor = 0x24C,
            AppCtrlFontSize = 0x24D,
            AppCtrlJustifyLeft = 0x24E,
            AppCtrlJustifyCenterH = 0x24F,
            AppCtrlJustifyRight = 0x250,
            AppCtrlJustifyBlockH = 0x251,
            AppCtrlJustifyTop = 0x252,
            AppCtrlJustifyCenterV = 0x253,
            AppCtrlJustifyBottom = 0x254,
            AppCtrlJustifyBlockV = 0x255,
            AppCtrlIndentDecrease = 0x256,
            AppCtrlIndentIncrease = 0x257,
            AppCtrlNumberedList = 0x258,
            AppCtrlRestartNumbering = 0x259,
            AppCtrlBulletedList = 0x25A,
            AppCtrlPromote = 0x25B,
            AppCtrlDemote = 0x25C,
            AppCtrlYes = 0x25D,
            AppCtrlNo = 0x25E,
            AppCtrlCancel = 0x25F,
            AppCtrlCatalog = 0x260,
            AppCtrlBuyCheckout = 0x261,
            AppCtrlAddToCart = 0x262,
            AppCtrlExpand = 0x263,
            AppCtrlExpandAll = 0x264,
            AppCtrlCollapse = 0x265,
            AppCtrlCollapseAll = 0x266,
            AppCtrlPrintPreview = 0x267,
            AppCtrlPasteSpecial = 0x268,
            AppCtrlInsertMode = 0x269,
            AppCtrlDelete = 0x26A,
            AppCtrlLock = 0x26B,
            AppCtrlUnlock = 0x26C,
            AppCtrlProtect = 0x26D,
            AppCtrlUnprotect = 0x26E,
            AppCtrlAttachComment = 0x26F,
            AppCtrlDeleteComment = 0x270,
            AppCtrlViewComment = 0x271,
            AppCtrlSelectWord = 0x272,
            AppCtrlSelectSentence = 0x273,
            AppCtrlDistributeVertically = 0x29C
        }
    }
}