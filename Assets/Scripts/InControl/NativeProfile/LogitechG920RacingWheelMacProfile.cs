using System;

namespace InControl.NativeProfile
{
		public class LogitechG920RacingWheelMacProfile : Xbox360DriverMacProfile
	{
				public LogitechG920RacingWheelMacProfile()
		{
			base.Name = "Logitech G920 Racing Wheel";
			base.Meta = "Logitech G920 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49761)
				}
			};
		}
	}
}
