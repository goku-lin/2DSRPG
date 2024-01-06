using System;

namespace InControl.NativeProfile
{
		public class LogitechDriveFXRacingWheelMacProfile : Xbox360DriverMacProfile
	{
				public LogitechDriveFXRacingWheelMacProfile()
		{
			base.Name = "Logitech DriveFX Racing Wheel";
			base.Meta = "Logitech DriveFX Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(51875)
				}
			};
		}
	}
}
