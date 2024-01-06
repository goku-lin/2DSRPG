using System;

namespace InControl.NativeProfile
{
		public class MadCatzFPSProMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzFPSProMacProfile()
		{
			base.Name = "Mad Catz FPS Pro";
			base.Meta = "Mad Catz FPS Pro on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61479)
				}
			};
		}
	}
}
