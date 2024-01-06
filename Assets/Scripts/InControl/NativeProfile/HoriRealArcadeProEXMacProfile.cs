using System;

namespace InControl.NativeProfile
{
		public class HoriRealArcadeProEXMacProfile : Xbox360DriverMacProfile
	{
				public HoriRealArcadeProEXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX";
			base.Meta = "Hori Real Arcade Pro EX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62724)
				}
			};
		}
	}
}
