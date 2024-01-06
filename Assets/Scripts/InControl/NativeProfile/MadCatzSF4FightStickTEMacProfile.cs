using System;

namespace InControl.NativeProfile
{
		public class MadCatzSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick TE";
			base.Meta = "Mad Catz SF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18232)
				}
			};
		}
	}
}
