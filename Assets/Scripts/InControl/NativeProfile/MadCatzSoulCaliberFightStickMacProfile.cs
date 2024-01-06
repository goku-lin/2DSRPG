using System;

namespace InControl.NativeProfile
{
		public class MadCatzSoulCaliberFightStickMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSoulCaliberFightStickMacProfile()
		{
			base.Name = "Mad Catz Soul Caliber Fight Stick";
			base.Meta = "Mad Catz Soul Caliber Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61503)
				}
			};
		}
	}
}
