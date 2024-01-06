using System;

namespace InControl.NativeProfile
{
		public class HoriRealArcadeProVXSAMacProfile : Xbox360DriverMacProfile
	{
				public HoriRealArcadeProVXSAMacProfile()
		{
			base.Name = "Hori Real Arcade Pro VX SA";
			base.Meta = "Hori Real Arcade Pro VX SA on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62722)
				}
			};
		}
	}
}
