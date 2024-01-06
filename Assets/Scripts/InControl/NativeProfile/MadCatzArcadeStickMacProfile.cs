using System;

namespace InControl.NativeProfile
{
		public class MadCatzArcadeStickMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzArcadeStickMacProfile()
		{
			base.Name = "Mad Catz Arcade Stick";
			base.Meta = "Mad Catz Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18264)
				}
			};
		}
	}
}
