///
/// Most of that stuff was taken from OpenHMD:
/// https://github.com/OpenHMD/OpenHMD/tree/master/src/drv_oculus_rift_s
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Oculus.Rift.S
{

    //    typedef struct {

    //    uint8_t id;

    //    uint32_t timestamp;
    //    uint16_t unknown_varying2;

    //    int16_t accel[3];
    //    int16_t gyro[3];
    //}
    //rift_s_controller_imu_block_t;


    /// <summary>
    /// IMU: Internal Measurement Unit
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ImuBlock
    {
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;

        //[MarshalAs(UnmanagedType.U4)]
        public uint timestamp;

        //[MarshalAs(UnmanagedType.U2)]
        public ushort unknown_varying2;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I2)]
        public fixed short accel[3];

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I2)]
        public fixed short gyro[3];
    }

    //typedef struct {
    // /* 0x08, 0x0c, 0x0d or 0x0e block */
    // uint8_t id;

    // uint8_t val;
    //}	rift_s_controller_maskbyte_block_t;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MaskByteBlock
    {
        //0x08, 0x0c, 0x0d or 0x0e block
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;

        //[MarshalAs(UnmanagedType.U1)]
        public byte val;
    }


    //typedef struct {
    // /* 0x1b trigger/grip block */
    // uint8_t id;
    // uint8_t vals[3];
    //}	rift_s_controller_triggrip_block_t;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TriggerGripBlock
    {
        // 0x1b trigger/grip block
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U1)]
        public fixed byte vals[3];
    }


    //typedef struct {
    // /* 0x22 joystick axes block */
    // uint8_t id;
    // uint32_t val;
    //}	rift_s_controller_joystick_block_t;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct JoystickBlock
    {
        // 0x22 joystick axes block
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;

        //[MarshalAs(UnmanagedType.U4)]
        public uint val;
    }

    //typedef struct {
    // /* 0x27 - capsense block */
    // uint8_t id;

    // uint8_t a_x;
    // uint8_t b_y;
    // uint8_t joystick;
    // uint8_t trigger;
    //}	rift_s_controller_capsense_block_t;


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CapSenseBlock
    {
        // 0x27 - capsense block
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;

        //[MarshalAs(UnmanagedType.U1)]
        public byte a_x;

        //[MarshalAs(UnmanagedType.U1)]
        public byte b_y;

        //[MarshalAs(UnmanagedType.U1)]
        public byte joystick;

        //[MarshalAs(UnmanagedType.U1)]
        public byte trigger;
    }

    //typedef struct {
    // uint8_t data[19];
    //}	rift_s_controller_raw_block_t;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RawBlock
    {
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.U1)]
        public fixed byte data[19]; // sizeof(ImuBlock)
    }

    /*
    typedef union
    {
        uint8_t block_id;
        rift_s_controller_imu_block_t imu;
        rift_s_controller_maskbyte_block_t maskbyte;
        rift_s_controller_triggrip_block_t triggrip;
        rift_s_controller_joystick_block_t joystick;
        rift_s_controller_capsense_block_t capsense;
        rift_s_controller_raw_block_t raw;
    }
    rift_s_controller_info_block_t;
    */


    [StructLayout(LayoutKind.Explicit)]
    public struct InfoBlock
    {
        [FieldOffset(0)]
        public byte block_id;

        [FieldOffset(0)]
        public ImuBlock imu;

        [FieldOffset(0)]
        public MaskByteBlock maskbyte;

        [FieldOffset(0)]
        public TriggerGripBlock triggrip;

        [FieldOffset(0)]
        public JoystickBlock joystick;

        [FieldOffset(0)]
        public CapSenseBlock capsense;

        [FieldOffset(0)]
        public RawBlock raw;
    }


    //    typedef struct {

    //    uint8_t id;

    //    uint64_t device_id;

    //    /* Length of the data block, which contains variable length entries
    // * If this is < 4, then the flags and log aren't valid. */

    //    uint8_t data_len;

    //    /* 0x04 = new log line
    //	 * 0x02 = parity bit, toggles each line when receiving log chars 
    //	 * other bits, unknown */
    //    uint8_t flags;
    //    // Contains up to 3 bytes of debug log chars
    //    uint8_t log[3];

    //    uint8_t num_info;
    //    rift_s_controller_info_block_t info[8];

    //    uint8_t extra_bytes_len;
    //    uint8_t extra_bytes[48];
    //}
    //rift_s_controller_report_t;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe class ControllerReport
    {
        //[MarshalAs(UnmanagedType.U1)]
        public byte id;
        //[MarshalAs(UnmanagedType.U8)]
        public ulong device_id;
        /* Length of the data block, which contains variable length entries
         * If this is < 4, then the flags and log aren't valid. */
        //[MarshalAs(UnmanagedType.U1)]
        public byte data_len;
        /* 0x04 = new log line
         * 0x02 = parity bit, toggles each line when receiving log chars 
         * other bits, unknown */
        //[MarshalAs(UnmanagedType.U1)]
        public byte flags;
        // Contains up to 3 bytes of debug log chars
        //[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1)]
        public byte[] log = new byte[3];

        // Info
        //[MarshalAs(UnmanagedType.U1)]
        public byte num_info;
        //[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8)]
        public InfoBlock[] info = new InfoBlock[8];

        // Extra
        //[MarshalAs(UnmanagedType.U1)]
        public byte extra_bytes_len;
        //[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1)]
        public byte[] extra_bytes = new byte[48];
    }


    enum BlockIds
    {
        MASK08 = 0x08,      /* Unknown. Vals seen 0x28, 0x0a, 0x32, 0x46, 0x00... */
        BUTTONS = 0x0c,  /* Button states */
        FINGERS = 0x0d,  /* Finger positions */
        MASK0e = 0x0e,      /* Unknown. Only seen 0x00 */
        TRIGGRIP = 0x1b,    /* Trigger + Grip */
        JOYSTICK = 0x22,    /* Joystick X/Y */
        CAPSENSE = 0x27,    /* Capacity sensor */
        IMU = 0x91
    }


    //    typedef struct {
    //  uint64_t device_id;
    //    uint32_t device_type;

    //    /* 0x04 = new log line
    //     * 0x02 = parity bit, toggles each line when receiving log chars 
    //     * other bits, unknown */
    //    uint8_t log_flags;
    //    int log_bytes;
    //    uint8_t log[MAX_LOG_SIZE];

    //    bool imu_time_valid;
    //    uint32_t imu_timestamp;
    //    uint16_t imu_unknown_varying2;
    //    int16_t raw_accel[3];
    //    int16_t raw_gyro[3];

    //    /* 0x8, 0x0c 0x0d or 0xe block */
    //    uint8_t mask08;
    //    uint8_t buttons;
    //    uint8_t fingers;
    //    uint8_t mask0e;

    //    uint16_t trigger;
    //    uint16_t grip;

    //    int16_t joystick_x;
    //    int16_t joystick_y;

    //    uint8_t capsense_a_x;
    //    uint8_t capsense_b_y;
    //    uint8_t capsense_joystick;
    //    uint8_t capsense_trigger;

    //    uint8_t extra_bytes_len;
    //    uint8_t extra_bytes[48];

    //    bool have_config;
    //    rift_s_controller_config config;

    //    bool have_calibration;
    //    rift_s_controller_imu_calibration calibration;

    //    vec3f accel;
    //    vec3f gyro;
    //    vec3f mag;
    //    fusion imu_fusion;
    //}
    //rift_s_controller_state;



    /// <summary>
    /// We made this a class rather than a struct so that we can work with object reference rather than making copies.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe class ControllerState
    {
        public const int MAX_LOG_SIZE = 1024;

        public ulong device_id;
        public uint device_type;

        /* 0x04 = new log line
        * 0x02 = parity bit, toggles each line when receiving log chars 
        * other bits, unknown */
        public byte log_flags;
        public int log_bytes;
        byte[] log = new byte[MAX_LOG_SIZE];
        //public fixed byte log[MAX_LOG_SIZE];

        public bool imu_time_valid;
        public uint imu_timestamp;
        public ushort imu_unknown_varying2;
        //public fixed short raw_accel[3];
        public short[] raw_accel = new short[3];
        //public fixed short raw_gyro[3];
        public short[] raw_gyro = new short[3];

        /* 0x8, 0x0c 0x0d or 0xe block */
        public byte mask08;
        public byte buttons;
        public byte fingers;
        public byte mask0e;

        public ushort trigger;
        public ushort grip;

        public short joystick_x;
        public short joystick_y;

        public byte capsense_a_x;
        public byte capsense_b_y;
        public byte capsense_joystick;
        public byte capsense_trigger;

        public byte extra_bytes_len;
        //public fixed byte extra_bytes[48];
        public short[] extra_bytes = new short[48];



        public string Dump()
        {
            string blank = "                                      ";
            string res = "";
            res += "\nID: " + device_id.ToString("X") + blank;
            res += "\nAccel: (" + raw_accel[0] + "," + raw_accel[1] + "," + raw_accel[2] + ")" + blank;
            res += "\nGyro: (" + raw_gyro[0] + "," + raw_gyro[1] + "," + raw_gyro[2] + ")" + blank;
            res += "\nMask08: " + mask08 + blank;
            res += "\nButtons: " + buttons + blank;
            res += "\nFingers: " + fingers + blank;
            res += "\nMask0E: " + mask0e + blank;
            res += "\nTrigger: " + trigger + blank;
            res += "\nGrip: " + grip + blank;
            res += "\nJoystick: (" + joystick_x + "," + joystick_y + ")" + blank;
            res += "\nCapsense A/X: " + capsense_a_x + blank;
            res += "\nCapsense B/Y: " + capsense_b_y + blank;
            res += "\nCapsense stick: " + capsense_joystick + blank;
            res += "\nCapsense trigger: " + capsense_trigger + blank;

            return res;
        }
    }


    class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aInputReport"></param>
        /// <returns></returns>
        public unsafe static ControllerReport ParseControllerInputReport(byte[] aInputReport)
        {
            ControllerReport report = new ControllerReport();
            var size = aInputReport.Length;

            // Apparently that fixed stuff allow use to do some pointer arithmetics
            fixed (byte* ptr = aInputReport)
            {
                byte* buf = ptr;
                report.id = buf[0];
                report.device_id = *(ulong*)(buf + 1);
                report.data_len = buf[9];
                report.num_info = 0;
                report.extra_bytes_len = 0;
                report.flags = 0;
                report.log[0] = 0;
                report.log[1] = 0;
                report.log[2] = 0;


                if (report.data_len < 4)
                {
                    //if (report.data_len != 0)
                    //LOGW("Controller report with data len %u - please report it", report->data_len);
                    return report; // No more to read
                }

                /* Advance the buffer pointer to the end of the common header.
                 * We now have data_len bytes left to read
                 */
                buf += 10;
                size -= 10;

                if (report.data_len > size)
                {
                    //LOGW("Controller report with data len %u > packet size 62 - please report it", report->data_len);
                    report.data_len = (byte)size;
                }

                byte avail = report.data_len;

                report.flags = buf[0];
                report.log[0] = buf[1];
                report.log[1] = buf[2];
                report.log[2] = buf[3];
                buf += 4;
                avail -= 4;

                /* While we have at least 2 bytes (type + at least 1 byte data), read a block */
                // sizeof(report->info) / sizeof(report->info[0]) == 8
                while (avail > 1 && report.num_info < 8)
                {
                    //rift_s_controller_info_block_t* info = report->info + report->num_info;

                    ref InfoBlock info = ref report.info[report.num_info];

                    byte block_size = 0;
                    info.block_id = buf[0];

                    BlockIds blockId = (BlockIds)info.block_id;
                    switch (blockId)
                    {
                        case BlockIds.MASK08:
                        case BlockIds.BUTTONS:
                        case BlockIds.FINGERS:
                        case BlockIds.MASK0e:
                            block_size = (byte)sizeof(MaskByteBlock);
                            break;
                        case BlockIds.TRIGGRIP:
                            block_size = (byte)sizeof(TriggerGripBlock);
                            break;
                        case BlockIds.JOYSTICK:
                            block_size = (byte)sizeof(JoystickBlock);
                            break;
                        case BlockIds.CAPSENSE:
                            block_size = (byte)sizeof(CapSenseBlock);
                            break;
                        case BlockIds.IMU:
                            block_size = (byte)sizeof(ImuBlock);
                            break;
                        default:
                            break;
                    }

                    if (block_size == 0 || avail < block_size)
                        break; /* Invalid block, or not enough data */

                    //memcpy(info->raw.data, buf, block_size);
                    fixed (byte* rd = info.raw.data)
                        Buffer.MemoryCopy(buf, rd, sizeof(RawBlock), block_size);

                    buf += block_size;
                    avail -= block_size;
                    report.num_info++;
                }

                if (avail > 0)
                {
                    //assert(avail < sizeof(report->extra_bytes));
                    report.extra_bytes_len = avail;
                    //memcpy(report->extra_bytes, buf, avail);
                    fixed (byte* eb = report.extra_bytes)
                    {
                        Buffer.MemoryCopy(eb, buf, avail, avail);
                    }
                }
            }

            return report;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="aReport"></param>
        /// <returns></returns>
        public unsafe static bool UpdateControllerState(ref ControllerState ctrl, ControllerReport aReport)
        {
            /* Collect state updates */
            ctrl.extra_bytes_len = 0;

            for (int i = 0; i < aReport.num_info; i++)
            {
                InfoBlock info = aReport.info[i];

                BlockIds blockId = (BlockIds)info.block_id;
                switch (blockId)
                {
                    case BlockIds.MASK08:
                        ctrl.mask08 = info.maskbyte.val;
                        break;
                    case BlockIds.BUTTONS:
                        ctrl.buttons = info.maskbyte.val;
                        break;
                    case BlockIds.FINGERS:
                        ctrl.fingers = info.maskbyte.val;
                        break;
                    case BlockIds.MASK0e:
                        ctrl.mask0e = info.maskbyte.val;
                        break;
                    case BlockIds.TRIGGRIP:
                        {
                            ctrl.trigger = (ushort)(((ushort)info.triggrip.vals[1] & 0x0f) << 8 | info.triggrip.vals[0]);
                            ctrl.grip = (ushort)((ushort)(info.triggrip.vals[1] & 0xf0) >> 4 | ((ushort)(info.triggrip.vals[2]) << 4));
                            break;
                        }
                    case BlockIds.JOYSTICK:
                        ctrl.joystick_x = (short)info.joystick.val;
                        ctrl.joystick_y = (short)(info.joystick.val >> 16);
                        break;
                    case BlockIds.CAPSENSE:
                        ctrl.capsense_a_x = info.capsense.a_x;
                        ctrl.capsense_b_y = info.capsense.b_y;
                        ctrl.capsense_joystick = info.capsense.joystick;
                        ctrl.capsense_trigger = info.capsense.trigger;
                        break;
                    case BlockIds.IMU:
                        {
                            int j;

#if DUMP_CONTROLLER_STATE
				/* print the state before updating the IMU timestamp a 2nd time */
				if (saw_imu_update)
					print_controller_state (ctrl);
				saw_imu_update = true;
#endif

                            ctrl.imu_unknown_varying2 = info.imu.unknown_varying2;

                            for (j = 0; j < 3; j++)
                            {
                                ctrl.raw_accel[j] = info.imu.accel[j];
                                ctrl.raw_gyro[j] = info.imu.gyro[j];
                            }
                            //handle_imu_update(ctrl, info->imu.timestamp, ctrl->raw_accel, ctrl->raw_gyro);
                            break;
                        }
                    default:
                        //LOGW("Invalid controller info block with ID %02x from device %08" PRIx64 ". Please report it.\n",
                        //    info->block_id, ctrl->device_id);
                        //return false;
                        break;
                }
            }

            //TODO: 
            //            if (report->extra_bytes_len > 0)
            //            {
            //                if (report->extra_bytes_len <= sizeof(ctrl->extra_bytes))
            //			memcpy(ctrl->extra_bytes, report->extra_bytes, report->extra_bytes_len);

            //        else
            //                {
            //                    LOGW("Controller report from %16" PRIx64" had too many extra bytes - %u (max %u)\n",
            //                        ctrl->device_id, report->extra_bytes_len, (unsigned int)(sizeof(ctrl->extra_bytes)));
            //                    return false;
            //                }

            //            }
            //            ctrl->extra_bytes_len = report->extra_bytes_len;

            //#if DUMP_CONTROLLER_STATE
            //	print_controller_state (ctrl);
            //#endif

            //            /* Finally, update and output the log */
            //            if (report->flags & 0x04)
            //            {
            //                /* New log line is starting, reset the counter */
            //                ctrl->log_bytes = 0;
            //            }

            //            if (ctrl->log_flags & 0x04 || (ctrl->log_flags & 0x02) != (report->flags & 0x02))
            //            {
            //                /* New log bytes in this report, collect them */
            //                for (int i = 0; i < 3; i++)
            //                {
            //                    uint8_t c = report->log[i];
            //                    if (c != '\0')
            //                    {
            //                        if (ctrl->log_bytes == (MAX_LOG_SIZE - 1))
            //                        {
            //                            /* Log line got too long... output it */
            //                            ctrl->log[MAX_LOG_SIZE - 1] = '\0';
            //                            LOGD("Controller: %s\n", ctrl->log);
            //                            ctrl->log_bytes = 0;
            //                        }
            //                        ctrl->log[ctrl->log_bytes++] = c;
            //                    }
            //                    else if (ctrl->log_bytes > 0)
            //                    {
            //                        /* Found the end of the string */
            //                        ctrl->log[ctrl->log_bytes] = '\0';
            //                        printf("L	%s\n", ctrl->log);
            //                        ctrl->log_bytes = 0;
            //                    }
            //                }
            //            }
            //            ctrl->log_flags = report->flags;



            return true;

        }


    }


}
