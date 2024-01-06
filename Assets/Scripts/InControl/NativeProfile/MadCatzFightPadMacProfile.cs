using System;

namespace InControl.NativeProfile
{
		public class MadCatzFightPadMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzFightPadMacProfile()
		{
			base.Name = "Mad Catz FightPad";
			base.Meta = "Mad Catz FightPad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61486)
				}
			};
		}
	}
}
