using System;

namespace InControl.NativeProfile
{
		public class MadCatzMLGFightStickTEMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzMLGFightStickTEMacProfile()
		{
			base.Name = "Mad Catz MLG Fight Stick TE";
			base.Meta = "Mad Catz MLG Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61502)
				}
			};
		}
	}
}
