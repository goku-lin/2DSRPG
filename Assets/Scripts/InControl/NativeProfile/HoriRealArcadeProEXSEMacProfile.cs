using System;

namespace InControl.NativeProfile
{
		public class HoriRealArcadeProEXSEMacProfile : Xbox360DriverMacProfile
	{
				public HoriRealArcadeProEXSEMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX SE";
			base.Meta = "Hori Real Arcade Pro EX SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(22)
				}
			};
		}
	}
}
