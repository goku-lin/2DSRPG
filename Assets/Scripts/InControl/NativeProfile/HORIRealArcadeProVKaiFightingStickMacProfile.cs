using System;

namespace InControl.NativeProfile
{
		public class HORIRealArcadeProVKaiFightingStickMacProfile : Xbox360DriverMacProfile
	{
				public HORIRealArcadeProVKaiFightingStickMacProfile()
		{
			base.Name = "HORI Real Arcade Pro V Kai Fighting Stick";
			base.Meta = "HORI Real Arcade Pro V Kai Fighting Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21774)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(120)
				}
			};
		}
	}
}
