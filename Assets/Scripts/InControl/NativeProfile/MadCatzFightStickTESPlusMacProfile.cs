using System;

namespace InControl.NativeProfile
{
		public class MadCatzFightStickTESPlusMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzFightStickTESPlusMacProfile()
		{
			base.Name = "Mad Catz Fight Stick TES Plus";
			base.Meta = "Mad Catz Fight Stick TES Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61506)
				}
			};
		}
	}
}
