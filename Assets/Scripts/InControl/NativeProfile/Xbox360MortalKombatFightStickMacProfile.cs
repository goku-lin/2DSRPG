using System;

namespace InControl.NativeProfile
{
		public class Xbox360MortalKombatFightStickMacProfile : Xbox360DriverMacProfile
	{
				public Xbox360MortalKombatFightStickMacProfile()
		{
			base.Name = "Xbox 360 Mortal Kombat Fight Stick";
			base.Meta = "Xbox 360 Mortal Kombat Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63750)
				}
			};
		}
	}
}
