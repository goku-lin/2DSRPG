using System;

namespace InControl.NativeProfile
{
		public class MadCatzFightPadControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzFightPadControllerMacProfile()
		{
			base.Name = "Mad Catz FightPad Controller";
			base.Meta = "Mad Catz FightPad Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61480)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18216)
				}
			};
		}
	}
}
