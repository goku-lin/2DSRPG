using System;
using System.Runtime.InteropServices;

namespace InControl
{
    public struct NativeDeviceInfo
    {
        public bool HasSameVendorID(NativeDeviceInfo deviceInfo)
        {
            return this.vendorID == deviceInfo.vendorID;
        }

        public bool HasSameProductID(NativeDeviceInfo deviceInfo)
        {
            return this.productID == deviceInfo.productID;
        }

        public bool HasSameVersionNumber(NativeDeviceInfo deviceInfo)
        {
            return this.versionNumber == deviceInfo.versionNumber;
        }

        public bool HasSameLocation(NativeDeviceInfo deviceInfo)
        {
            return !string.IsNullOrEmpty(this.location) && this.location == deviceInfo.location;
        }

        public bool HasSameSerialNumber(NativeDeviceInfo deviceInfo)
        {
            return !string.IsNullOrEmpty(this.serialNumber) && this.serialNumber == deviceInfo.serialNumber;
        }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string location;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string serialNumber;

        public ushort vendorID;

        public ushort productID;

        public uint versionNumber;

        public NativeDeviceDriverType driverType;

        public NativeDeviceTransportType transportType;

        public uint numButtons;

        public uint numAnalogs;
    }
}
