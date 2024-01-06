using System;

namespace InControl.NativeProfile
{
		public class MadCatzBrawlStickMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzBrawlStickMacProfile()
		{
			base.Name = "Mad Catz Brawl Stick";
			base.Meta = "Mad Catz Brawl Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61465)
				}
			};
		}
	}
}
