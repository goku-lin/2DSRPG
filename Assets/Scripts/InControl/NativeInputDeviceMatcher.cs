using System;
using System.Text.RegularExpressions;

namespace InControl
{
    public class NativeInputDeviceMatcher
    {
        internal bool Matches(NativeDeviceInfo deviceInfo)
        {
            bool result = false;
            if (this.VendorID != null)
            {
                if (this.VendorID.Value != deviceInfo.vendorID)
                {
                    return false;
                }
                result = true;
            }
            if (this.ProductID != null)
            {
                if (this.ProductID.Value != deviceInfo.productID)
                {
                    return false;
                }
                result = true;
            }
            if (this.VersionNumber != null)
            {
                if (this.VersionNumber.Value != deviceInfo.versionNumber)
                {
                    return false;
                }
                result = true;
            }
            if (this.DriverType != null)
            {
                if (this.DriverType.Value != deviceInfo.driverType)
                {
                    return false;
                }
                result = true;
            }
            if (this.TransportType != null)
            {
                if (this.TransportType.Value != deviceInfo.transportType)
                {
                    return false;
                }
                result = true;
            }
            if (this.NameLiterals != null && this.NameLiterals.Length > 0)
            {
                int num = this.NameLiterals.Length;
                for (int i = 0; i < num; i++)
                {
                    if (string.Equals(deviceInfo.name, this.NameLiterals[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (this.NamePatterns != null && this.NamePatterns.Length > 0)
            {
                int num2 = this.NamePatterns.Length;
                for (int j = 0; j < num2; j++)
                {
                    if (Regex.IsMatch(deviceInfo.name, this.NamePatterns[j], RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            return result;
        }

        public ushort? VendorID;

        public ushort? ProductID;

        public uint? VersionNumber;

        public NativeDeviceDriverType? DriverType;

        public NativeDeviceTransportType? TransportType;

        public string[] NameLiterals;

        public string[] NamePatterns;
    }
}
