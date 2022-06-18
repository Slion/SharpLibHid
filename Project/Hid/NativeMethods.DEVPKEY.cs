using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windows.Win32.Devices.Properties
{
    /// <summary>
    /// We copy those here from the generated file to make it more convenient to list them and extract their names using reflection.
    /// We needed to use properties rather than fields to make it easier to get their names through reflection.
    /// </summary>
    public static partial class DEVPKEY
    {
            public static DEVPROPKEY Storage_Portable { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 2U
            };
    
            public static DEVPROPKEY Storage_Removable_Media { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 3U
            };
    
            public static DEVPROPKEY Storage_System_Critical { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 4U
            };
    
            public static DEVPROPKEY Storage_Disk_Number { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 5U
            };
    
            public static DEVPROPKEY Storage_Partition_Number { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 6U
            };
    
            public static DEVPROPKEY Storage_Mbr_Type { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 7U
            };
    
            public static DEVPROPKEY Storage_Gpt_Type { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 8U
            };
    
            public static DEVPROPKEY Storage_Gpt_Name { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4D1EBEE8, 0x0803, 0x4774, 0x98, 0x42, 0xB7, 0x7D, 0xB5, 0x02, 0x65, 0xE9),
                pid = 9U
            };
    
            public static DEVPROPKEY IndirectDisplay { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xC50A3F10, 0xAA5C, 0x4247, 0xB8, 0x30, 0xD6, 0xA6, 0xF8, 0xEA, 0xA3, 0x10),
                pid = 1U
            };
    
            public static DEVPROPKEY Device_TerminalLuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xC50A3F10, 0xAA5C, 0x4247, 0xB8, 0x30, 0xD6, 0xA6, 0xF8, 0xEA, 0xA3, 0x10),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_AdapterLuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xC50A3F10, 0xAA5C, 0x4247, 0xB8, 0x30, 0xD6, 0xA6, 0xF8, 0xEA, 0xA3, 0x10),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_ActivityId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xC50A3F10, 0xAA5C, 0x4247, 0xB8, 0x30, 0xD6, 0xA6, 0xF8, 0xEA, 0xA3, 0x10),
                pid = 4U
            };
    
            public static DEVPROPKEY WIA_DeviceType { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x6BDD1FC6, 0x810F, 0x11D0, 0xBE, 0xC7, 0x08, 0x00, 0x2B, 0xE2, 0x09, 0x2F),
                pid = 2U
            };
    
            public static DEVPROPKEY WIA_USDClassId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x6BDD1FC6, 0x810F, 0x11D0, 0xBE, 0xC7, 0x08, 0x00, 0x2B, 0xE2, 0x09, 0x2F),
                pid = 3U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_UsagePage { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_UsageId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 3U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_Is{ get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 4U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_VendorId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 5U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_ProductId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 6U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_VersionNumber { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 7U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_BackgroundAccess { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 8U
            };
    
            public static DEVPROPKEY DeviceInterface_HID_WakeScreenOnInputCapable { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCBF38310, 0x4A17, 0x4310, 0xA1, 0xEB, 0x24, 0x7F, 0x0B, 0x67, 0x59, 0x3B),
                pid = 9U
            };
    
            public static DEVPROPKEY MTPBTH_IsConnected { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xEA1237FA, 0x589D, 0x4472, 0x84, 0xE4, 0x0A, 0xBE, 0x36, 0xFD, 0x62, 0xEF),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceInterface_Autoplay_Silent { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x434DD28F, 0x9E75, 0x450A, 0x9A, 0xB9, 0xFF, 0x61, 0xE6, 0x18, 0xBA, 0xD0),
                pid = 2U
            };
    
            public static DEVPROPKEY NAME { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC),
                pid = 10U
            };
    
            public static DEVPROPKEY Device_DeviceDesc { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_HardwareIds { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_CompatibleIds { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_Service { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_Class { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 9U
            };
    
            public static DEVPROPKEY Device_ClassGuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 10U
            };
    
            public static DEVPROPKEY Device_Driver { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 11U
            };
    
            public static DEVPROPKEY Device_ConfigFlags { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 12U
            };
    
            public static DEVPROPKEY Device_Manufacturer { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 13U
            };
    
            public static DEVPROPKEY Device_FriendlyName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 14U
            };
    
            public static DEVPROPKEY Device_LocationInfo { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 15U
            };
    
            public static DEVPROPKEY Device_PDOName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 16U
            };
    
            public static DEVPROPKEY Device_Capabilities { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 17U
            };
    
            public static DEVPROPKEY Device_UINumber { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 18U
            };
    
            public static DEVPROPKEY Device_UpperFilters { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 19U
            };
    
            public static DEVPROPKEY Device_LowerFilters { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 20U
            };
    
            public static DEVPROPKEY Device_BusTypeGuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 21U
            };
    
            public static DEVPROPKEY Device_LegacyBusType { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 22U
            };
    
            public static DEVPROPKEY Device_BusNumber { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 23U
            };
    
            public static DEVPROPKEY Device_EnumeratorName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 24U
            };
    
            public static DEVPROPKEY Device_Security { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 25U
            };
    
            public static DEVPROPKEY Device_SecuritySDS { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 26U
            };
    
            public static DEVPROPKEY Device_DevType { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 27U
            };
    
            public static DEVPROPKEY Device_Exclusive { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 28U
            };
    
            public static DEVPROPKEY Device_Characteristics { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 29U
            };
    
            public static DEVPROPKEY Device_Address { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 30U
            };
    
            public static DEVPROPKEY Device_UINumberDescFormat { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 31U
            };
    
            public static DEVPROPKEY Device_PowerData { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 32U
            };
    
            public static DEVPROPKEY Device_RemovalPolicy { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 33U
            };
    
            public static DEVPROPKEY Device_RemovalPolicyDefault { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 34U
            };
    
            public static DEVPROPKEY Device_RemovalPolicyOverride { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 35U
            };
    
            public static DEVPROPKEY Device_InstallState { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 36U
            };
    
            public static DEVPROPKEY Device_LocationPaths { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 37U
            };
    
            public static DEVPROPKEY Device_BaseContainerId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0),
                pid = 38U
            };
    
            public static DEVPROPKEY Device_InstanceId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 256U
            };
    
            public static DEVPROPKEY Device_DevNodeStatus { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_ProblemCode { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_EjectionRelations { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_RemovalRelations { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 5U
            };
    
            public static DEVPROPKEY Device_PowerRelations { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_BusRelations { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 7U
            };
    
            public static DEVPROPKEY Device_Parent { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 8U
            };
    
            public static DEVPROPKEY Device_Children { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 9U
            };
    
            public static DEVPROPKEY Device_Siblings { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 10U
            };
    
            public static DEVPROPKEY Device_TransportRelations { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 11U
            };
    
            public static DEVPROPKEY Device_ProblemStatus { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x08, 0xA5, 0xA7),
                pid = 12U
            };
    
            public static DEVPROPKEY Device_Reported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80497100, 0x8C73, 0x48B9, 0xAA, 0xD9, 0xCE, 0x38, 0x7E, 0x19, 0xC5, 0x6E),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_Legacy { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80497100, 0x8C73, 0x48B9, 0xAA, 0xD9, 0xCE, 0x38, 0x7E, 0x19, 0xC5, 0x6E),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_ContainerId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x8C7ED206, 0x3F8A, 0x4827, 0xB3, 0xAB, 0xAE, 0x9E, 0x1F, 0xAE, 0xFC, 0x6C),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_InLocalMachineContainer { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x8C7ED206, 0x3F8A, 0x4827, 0xB3, 0xAB, 0xAE, 0x9E, 0x1F, 0xAE, 0xFC, 0x6C),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_Model { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 39U
            };
    
            public static DEVPROPKEY Device_ModelId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_FriendlyNameAttributes { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_ManufacturerAttributes { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_PresenceNotForDevice { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 5U
            };
    
            public static DEVPROPKEY Device_SignalStrength { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_IsAssociateableByUserAction { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 7U
            };
    
            public static DEVPROPKEY Device_ShowInUninstallUI { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B),
                pid = 8U
            };
    
            public static DEVPROPKEY Device_Numa_Proximity_Domain { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 1U
            };
    
            public static DEVPROPKEY Device_DHP_Rebalance_Policy { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_Numa_Node { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_BusReportedDeviceDesc { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_IsPresent { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 5U
            };
    
            public static DEVPROPKEY Device_HasProblem { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_ConfigurationId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 7U
            };
    
            public static DEVPROPKEY Device_ReportedDeviceIdsHash { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 8U
            };
    
            public static DEVPROPKEY Device_PhysicalDeviceLocation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 9U
            };
    
            public static DEVPROPKEY Device_BiosDeviceName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 10U
            };
    
            public static DEVPROPKEY Device_DriverProblemDesc { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 11U
            };
    
            public static DEVPROPKEY Device_DebuggerSafe { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 12U
            };
    
            public static DEVPROPKEY Device_PostInstallInProgress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 13U
            };
    
            public static DEVPROPKEY Device_Stack { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 14U
            };
    
            public static DEVPROPKEY Device_ExtendedConfigurationIds { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 15U
            };
    
            public static DEVPROPKEY Device_IsRebootRequired { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 16U
            };
    
            public static DEVPROPKEY Device_FirmwareDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 17U
            };
    
            public static DEVPROPKEY Device_FirmwareVersion { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 18U
            };
    
            public static DEVPROPKEY Device_FirmwareRevision { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 19U
            };
    
            public static DEVPROPKEY Device_DependencyProviders { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 20U
            };
    
            public static DEVPROPKEY Device_DependencyDependents { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 21U
            };
    
            public static DEVPROPKEY Device_SoftRestartSupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 22U
            };
    
            public static DEVPROPKEY Device_ExtendedAddress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 23U
            };
    
            public static DEVPROPKEY Device_AssignedToGuest { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 24U
            };
    
            public static DEVPROPKEY Device_CreatorProcessId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0x0B, 0x89, 0x4C, 0xBD, 0xA2),
                pid = 25U
            };
    
            public static DEVPROPKEY Device_SessionId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_InstallDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 100U
            };
    
            public static DEVPROPKEY Device_FirstInstallDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 101U
            };
    
            public static DEVPROPKEY Device_LastArrivalDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 102U
            };
    
            public static DEVPROPKEY Device_LastRemovalDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 103U
            };
    
            public static DEVPROPKEY Device_DriverDate { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_DriverVersion { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 3U
            };
    
            public static DEVPROPKEY Device_DriverDesc { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 4U
            };
    
            public static DEVPROPKEY Device_DriverInfPath { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 5U
            };
    
            public static DEVPROPKEY Device_DriverInfSection { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 6U
            };
    
            public static DEVPROPKEY Device_DriverInfSectionExt { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 7U
            };
    
            public static DEVPROPKEY Device_MatchingDeviceId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 8U
            };
    
            public static DEVPROPKEY Device_DriverProvider { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 9U
            };
    
            public static DEVPROPKEY Device_DriverPropPageProvider { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 10U
            };
    
            public static DEVPROPKEY Device_DriverCoInstallers { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 11U
            };
    
            public static DEVPROPKEY Device_ResourcePickerTags { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 12U
            };
    
            public static DEVPROPKEY Device_ResourcePickerExceptions { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 13U
            };
    
            public static DEVPROPKEY Device_DriverRank { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 14U
            };
    
            public static DEVPROPKEY Device_DriverLogoLevel { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 15U
            };
    
            public static DEVPROPKEY Device_NoConnectSound { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 17U
            };
    
            public static DEVPROPKEY Device_GenericDriverInstalled { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 18U
            };
    
            public static DEVPROPKEY Device_AdditionalSoftwareRequested { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0x0C, 0x75, 0xD6),
                pid = 19U
            };
    
            public static DEVPROPKEY Device_SafeRemovalRequired { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xAFD97640, 0x86A3, 0x4210, 0xB6, 0x7C, 0x28, 0x9C, 0x41, 0xAA, 0xBE, 0x55),
                pid = 2U
            };
    
            public static DEVPROPKEY Device_SafeRemovalRequiredOverride { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xAFD97640, 0x86A3, 0x4210, 0xB6, 0x7C, 0x28, 0x9C, 0x41, 0xAA, 0xBE, 0x55),
                pid = 3U
            };
    
            public static DEVPROPKEY DrvPkg_Model { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 2U
            };
    
            public static DEVPROPKEY DrvPkg_VendorWebSite { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 3U
            };
    
            public static DEVPROPKEY DrvPkg_DetailedDescription { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 4U
            };
    
            public static DEVPROPKEY DrvPkg_DocumentationLink { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 5U
            };
    
            public static DEVPROPKEY DrvPkg_Icon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 6U
            };
    
            public static DEVPROPKEY DrvPkg_BrandingIcon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32),
                pid = 7U
            };
    
            public static DEVPROPKEY DeviceClass_UpperFilters { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 19U
            };
    
            public static DEVPROPKEY DeviceClass_LowerFilters { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 20U
            };
    
            public static DEVPROPKEY DeviceClass_Security { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 25U
            };
    
            public static DEVPROPKEY DeviceClass_SecuritySDS { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 26U
            };
    
            public static DEVPROPKEY DeviceClass_DevType { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 27U
            };
    
            public static DEVPROPKEY DeviceClass_Exclusive { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 28U
            };
    
            public static DEVPROPKEY DeviceClass_Characteristics { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B),
                pid = 29U
            };
    
            public static DEVPROPKEY DeviceClass_Name { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceClass_ClassName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 3U
            };
    
            public static DEVPROPKEY DeviceClass_Icon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 4U
            };
    
            public static DEVPROPKEY DeviceClass_ClassInstaller { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 5U
            };
    
            public static DEVPROPKEY DeviceClass_PropPageProvider { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 6U
            };
    
            public static DEVPROPKEY DeviceClass_NoInstallClass { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 7U
            };
    
            public static DEVPROPKEY DeviceClass_NoDisplayClass { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 8U
            };
    
            public static DEVPROPKEY DeviceClass_SilentInstall { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 9U
            };
    
            public static DEVPROPKEY DeviceClass_NoUseClass { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 10U
            };
    
            public static DEVPROPKEY DeviceClass_DefaultService { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 11U
            };
    
            public static DEVPROPKEY DeviceClass_IconPath { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x08, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66),
                pid = 12U
            };
    
            public static DEVPROPKEY DeviceClass_DHPRebalanceOptOut { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xD14D3EF3, 0x66CF, 0x4BA2, 0x9D, 0x38, 0x0D, 0xDB, 0x37, 0xAB, 0x47, 0x01),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceClass_ClassCoInstallers { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x713D1703, 0xA2E2, 0x49F5, 0x92, 0x14, 0x56, 0x47, 0x2E, 0xF3, 0xDA, 0x5C),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceInterface_FriendlyName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceInterface_Enabled { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 3U
            };
    
            public static DEVPROPKEY DeviceInterface_ClassGuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 4U
            };
    
            public static DEVPROPKEY DeviceInterface_ReferenceString { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 5U
            };
    
            public static DEVPROPKEY DeviceInterface_Restricted { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 6U
            };
    
            public static DEVPROPKEY DeviceInterface_UnrestrictedAppCapabilities { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 8U
            };
    
            public static DEVPROPKEY DeviceInterface_SchematicName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x026E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22),
                pid = 9U
            };
    
            public static DEVPROPKEY DeviceInterfaceClass_DefaultInterface { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x14C83A99, 0x0B3F, 0x44B7, 0xBE, 0x4C, 0xA1, 0x78, 0xD3, 0x99, 0x05, 0x64),
                pid = 2U
            };
    
            public static DEVPROPKEY DeviceInterfaceClass_Name { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x14C83A99, 0x0B3F, 0x44B7, 0xBE, 0x4C, 0xA1, 0x78, 0xD3, 0x99, 0x05, 0x64),
                pid = 3U
            };
    
            public static DEVPROPKEY DeviceContainer_Address { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 51U
            };
    
            public static DEVPROPKEY DeviceContainer_DiscoveryMethod { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 52U
            };
    
            public static DEVPROPKEY DeviceContainer_IsEncrypted { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 53U
            };
    
            public static DEVPROPKEY DeviceContainer_IsAuthenticated { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 54U
            };
    
            public static DEVPROPKEY DeviceContainer_IsConnected { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 55U
            };
    
            public static DEVPROPKEY DeviceContainer_IsPaired { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 56U
            };
    
            public static DEVPROPKEY DeviceContainer_Icon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 57U
            };
    
            public static DEVPROPKEY DeviceContainer_Version { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 65U
            };
    
            public static DEVPROPKEY DeviceContainer_Last_Seen { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 66U
            };
    
            public static DEVPROPKEY DeviceContainer_Last_Connected { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 67U
            };
    
            public static DEVPROPKEY DeviceContainer_IsShowInDisconnectedState { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 68U
            };
    
            public static DEVPROPKEY DeviceContainer_IsLocalMachine { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 70U
            };
    
            public static DEVPROPKEY DeviceContainer_MetadataPath { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 71U
            };
    
            public static DEVPROPKEY DeviceContainer_IsMetadataSearchInProgress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 72U
            };
    
            public static DEVPROPKEY DeviceContainer_MetadataChecksum { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 73U
            };
    
            public static DEVPROPKEY DeviceContainer_IsNotInterestingForDisplay { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 74U
            };
    
            public static DEVPROPKEY DeviceContainer_LaunchDeviceStageOnDeviceConnect { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 76U
            };
    
            public static DEVPROPKEY DeviceContainer_LaunchDeviceStageFromExplorer { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 77U
            };
    
            public static DEVPROPKEY DeviceContainer_BaselineExperienceId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 78U
            };
    
            public static DEVPROPKEY DeviceContainer_IsDeviceUniquelyIdentifiable { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 79U
            };
    
            public static DEVPROPKEY DeviceContainer_AssociationArray { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 80U
            };
    
            public static DEVPROPKEY DeviceContainer_DeviceDescription1 { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 81U
            };
    
            public static DEVPROPKEY DeviceContainer_DeviceDescription2 { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 82U
            };
    
            public static DEVPROPKEY DeviceContainer_HasProblem { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 83U
            };
    
            public static DEVPROPKEY DeviceContainer_IsSharedDevice { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 84U
            };
    
            public static DEVPROPKEY DeviceContainer_IsNetworkDevice { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 85U
            };
    
            public static DEVPROPKEY DeviceContainer_IsDefaultDevice { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 86U
            };
    
            public static DEVPROPKEY DeviceContainer_MetadataCabinet { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 87U
            };
    
            public static DEVPROPKEY DeviceContainer_RequiresPairingElevation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 88U
            };
    
            public static DEVPROPKEY DeviceContainer_ExperienceId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 89U
            };
    
            public static DEVPROPKEY DeviceContainer_Category { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 90U
            };
    
            public static DEVPROPKEY DeviceContainer_Category_Desc_Singular { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 91U
            };
    
            public static DEVPROPKEY DeviceContainer_Category_Desc_Plural { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 92U
            };
    
            public static DEVPROPKEY DeviceContainer_Category_Icon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 93U
            };
    
            public static DEVPROPKEY DeviceContainer_CategoryGroup_Desc { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 94U
            };
    
            public static DEVPROPKEY DeviceContainer_CategoryGroup_Icon { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 95U
            };
    
            public static DEVPROPKEY DeviceContainer_PrimaryCategory { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 97U
            };
    
            public static DEVPROPKEY DeviceContainer_UnpairUninstall { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 98U
            };
    
            public static DEVPROPKEY DeviceContainer_RequiresUninstallElevation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 99U
            };
    
            public static DEVPROPKEY DeviceContainer_DeviceFunctionSubRank { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 100U
            };
    
            public static DEVPROPKEY DeviceContainer_AlwaysShowDeviceAsConnected { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 101U
            };
    
            public static DEVPROPKEY DeviceContainer_ConfigFlags { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 105U
            };
    
            public static DEVPROPKEY DeviceContainer_PrivilegedPackageFamilyNames { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 106U
            };
    
            public static DEVPROPKEY DeviceContainer_CustomPrivilegedPackageFamilyNames { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 107U
            };
    
            public static DEVPROPKEY DeviceContainer_IsRebootRequired { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57),
                pid = 108U
            };
    
            public static DEVPROPKEY DeviceContainer_FriendlyName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD),
                pid = 12288U
            };
    
            public static DEVPROPKEY DeviceContainer_Manufacturer { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD),
                pid = 8192U
            };
    
            public static DEVPROPKEY DeviceContainer_ModelName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD),
                pid = 8194U
            };
    
            public static DEVPROPKEY DeviceContainer_ModelNumber { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD),
                pid = 8195U
            };
    
            public static DEVPROPKEY DeviceContainer_InstallInProgress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29),
                pid = 9U
            };
    
            public static DEVPROPKEY DevQuery_ObjectType { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x13673F42, 0xA3D6, 0x49F6, 0xB4, 0xDA, 0xAE, 0x46, 0xE0, 0xC5, 0x23, 0x7C),
                pid = 2U
            };
    
            public static DEVPROPKEY AudioEndpointPlugin_FactoryCLSID { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x12D83BD7, 0xCF12, 0x46BE, 0x85, 0x40, 0x81, 0x27, 0x10, 0xD3, 0x02, 0x1C),
                pid = 1U
            };
    
            public static DEVPROPKEY AudioEndpointPlugin_DataFlow { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x12D83BD7, 0xCF12, 0x46BE, 0x85, 0x40, 0x81, 0x27, 0x10, 0xD3, 0x02, 0x1C),
                pid = 2U
            };
    
            public static DEVPROPKEY AudioEndpointPlugin_PnPInterface { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x12D83BD7, 0xCF12, 0x46BE, 0x85, 0x40, 0x81, 0x27, 0x10, 0xD3, 0x02, 0x1C),
                pid = 3U
            };
    
            public static DEVPROPKEY AudioEndpointPlugin2_FactoryCLSID { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x12D83BD7, 0xCF12, 0x46BE, 0x85, 0x40, 0x81, 0x27, 0x10, 0xD3, 0x02, 0x1C),
                pid = 4U
            };
    
            public static DEVPROPKEY DeviceInterface_IsVirtualCamera { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x6EDC630D, 0xC2E3, 0x43B7, 0xB2, 0xD1, 0x20, 0x52, 0x5A, 0x1A, 0xF1, 0x20),
                pid = 3U
            };
    
            public static DEVPROPKEY KsAudio_PacketSize_Constraints { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x13E004D6, 0xB066, 0x43BD, 0x91, 0x3B, 0xA4, 0x15, 0xCD, 0x13, 0xDA, 0x87),
                pid = 2U
            };
    
            public static DEVPROPKEY KsAudio_Controller_DeviceInterface_Path { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x13E004D6, 0xB066, 0x43BD, 0x91, 0x3B, 0xA4, 0x15, 0xCD, 0x13, 0xDA, 0x87),
                pid = 3U
            };
    
            public static DEVPROPKEY KsAudio_PacketSize_Constraints2 { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x9404F781, 0x7191, 0x409B, 0x8B, 0x0B, 0x80, 0xBF, 0x6E, 0xC2, 0x29, 0xAE),
                pid = 2U
            };
    
            public static DEVPROPKEY WiFiDirect_DeviceAddress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 1U
            };
    
            public static DEVPROPKEY WiFiDirect_InterfaceAddress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 2U
            };
    
            public static DEVPROPKEY WiFiDirect_InterfaceGuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 3U
            };
    
            public static DEVPROPKEY WiFiDirect_GroupId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 4U
            };
    
            public static DEVPROPKEY WiFiDirect_IsConnected { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 5U
            };
    
            public static DEVPROPKEY WiFiDirect_IsVisible { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 6U
            };
    
            public static DEVPROPKEY WiFiDirect_IsLegacyDevice { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 7U
            };
    
            public static DEVPROPKEY WiFiDirect_MiracastVersion { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 8U
            };
    
            public static DEVPROPKEY WiFiDirect_IsMiracastLCPSupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 9U
            };
    
            public static DEVPROPKEY WiFiDirect_Services { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 10U
            };
    
            public static DEVPROPKEY WiFiDirect_SupportedChannelList { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 11U
            };
    
            public static DEVPROPKEY WiFiDirect_InformationElements { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 12U
            };
    
            public static DEVPROPKEY WiFiDirect_DeviceAddressCopy { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 13U
            };
    
            public static DEVPROPKEY WiFiDirect_IsRecentlyAssociated { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 14U
            };
    
            public static DEVPROPKEY WiFiDirect_Service_Aeps { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 15U
            };
    
            public static DEVPROPKEY WiFiDirect_NoMiracastAutoProject { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 16U
            };
    
            public static DEVPROPKEY InfraCast_Supported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 17U
            };
    
            public static DEVPROPKEY InfraCast_StreamSecuritySupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 18U
            };
    
            public static DEVPROPKEY InfraCast_AccessPointBssid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 19U
            };
    
            public static DEVPROPKEY InfraCast_SinkHostName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 20U
            };
    
            public static DEVPROPKEY InfraCast_ChallengeAep { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 21U
            };
    
            public static DEVPROPKEY WiFiDirect_IsDMGCapable { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 22U
            };
    
            public static DEVPROPKEY InfraCast_DevnodeAep { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 23U
            };
    
            public static DEVPROPKEY WiFiDirect_FoundWsbService { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 24U
            };
    
            public static DEVPROPKEY InfraCast_HostName_ResolutionMode { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 25U
            };
    
            public static DEVPROPKEY InfraCast_SinkIpAddress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 26U
            };
    
            public static DEVPROPKEY WiFiDirect_TransientAssociation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 27U
            };
    
            public static DEVPROPKEY WiFiDirect_LinkQuality { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 28U
            };
    
            public static DEVPROPKEY InfraCast_PinSupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 29U
            };
    
            public static DEVPROPKEY InfraCast_RtspTcpConnectionParametersSupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 30U
            };
    
            public static DEVPROPKEY WiFiDirect_Miracast_SessionMgmtControlPort { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 31U
            };
    
            public static DEVPROPKEY WiFiDirect_RtspTcpConnectionParametersSupported { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x1506935D, 0xE3E7, 0x450F, 0x86, 0x37, 0x82, 0x23, 0x3E, 0xBE, 0x5F, 0x6E),
                pid = 32U
            };
    
            public static DEVPROPKEY WiFiDirectServices_ServiceAddress { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 2U
            };
    
            public static DEVPROPKEY WiFiDirectServices_ServiceName { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 3U
            };
    
            public static DEVPROPKEY WiFiDirectServices_ServiceInformation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 4U
            };
    
            public static DEVPROPKEY WiFiDirectServices_AdvertisementId { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 5U
            };
    
            public static DEVPROPKEY WiFiDirectServices_ServiceConfigMethods { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 6U
            };
    
            public static DEVPROPKEY WiFiDirectServices_RequestServiceInformation { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0x31B37743, 0x7C5E, 0x4005, 0x93, 0xE6, 0xE9, 0x53, 0xF9, 0x2B, 0x82, 0xE9),
                pid = 7U
            };
    
            public static DEVPROPKEY WiFi_InterfaceGuid { get; } = new DEVPROPKEY()
            {
                fmtid = new Guid(0xEF1167EB, 0xCBFC, 0x4341, 0xA5, 0x68, 0xA7, 0xC9, 0x1A, 0x68, 0x98, 0x2C),
                pid = 2U
            };
    };
};
