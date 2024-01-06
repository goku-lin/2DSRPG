using System;

namespace InControl.NativeProfile
{
		public class HoriRealArcadeProEXPremiumVLXMacProfile : Xbox360DriverMacProfile
	{
				public HoriRealArcadeProEXPremiumVLXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX Premium VLX";
			base.Meta = "Hori Real Arcade Pro EX Premium VLX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62726)
				}
			};
		}
	}
}
