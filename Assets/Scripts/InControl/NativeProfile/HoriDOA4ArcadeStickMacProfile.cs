using System;

namespace InControl.NativeProfile
{
		public class HoriDOA4ArcadeStickMacProfile : Xbox360DriverMacProfile
	{
				public HoriDOA4ArcadeStickMacProfile()
		{
			base.Name = "Hori DOA4 Arcade Stick";
			base.Meta = "Hori DOA4 Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				}
			};
		}
	}
}
