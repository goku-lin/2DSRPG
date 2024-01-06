using System;

namespace InControl.NativeProfile
{
		public class MadCatzProControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzProControllerMacProfile()
		{
			base.Name = "Mad Catz Pro Controller";
			base.Meta = "Mad Catz Pro Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18214)
				}
			};
		}
	}
}
