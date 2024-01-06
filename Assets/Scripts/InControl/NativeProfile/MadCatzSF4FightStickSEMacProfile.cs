using System;

namespace InControl.NativeProfile
{
		public class MadCatzSF4FightStickSEMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSF4FightStickSEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick SE";
			base.Meta = "Mad Catz SF4 Fight Stick SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18200)
				}
			};
		}
	}
}
