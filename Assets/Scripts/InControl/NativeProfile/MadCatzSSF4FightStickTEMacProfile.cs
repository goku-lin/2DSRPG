using System;

namespace InControl.NativeProfile
{
		public class MadCatzSSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(63288)
				}
			};
		}
	}
}
