using System;

namespace InControl.NativeProfile
{
		public class HORIRealArcadeProVXMacProfile : Xbox360DriverMacProfile
	{
				public HORIRealArcadeProVXMacProfile()
		{
			base.Name = "HORI Real Arcade Pro VX";
			base.Meta = "HORI Real Arcade Pro VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(27)
				}
			};
		}
	}
}
