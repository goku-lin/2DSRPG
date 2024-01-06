using System;

namespace InControl.NativeProfile
{
		public class TrustPredatorJoystickMacProfile : Xbox360DriverMacProfile
	{
				public TrustPredatorJoystickMacProfile()
		{
			base.Name = "Trust Predator Joystick";
			base.Meta = "Trust Predator Joystick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2064),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
