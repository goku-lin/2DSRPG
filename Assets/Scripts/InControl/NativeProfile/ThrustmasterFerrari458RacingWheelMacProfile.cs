using System;

namespace InControl.NativeProfile
{
		public class ThrustmasterFerrari458RacingWheelMacProfile : Xbox360DriverMacProfile
	{
				public ThrustmasterFerrari458RacingWheelMacProfile()
		{
			base.Name = "Thrustmaster Ferrari 458 Racing Wheel";
			base.Meta = "Thrustmaster Ferrari 458 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23296)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23299)
				}
			};
		}
	}
}
