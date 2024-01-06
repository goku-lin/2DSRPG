using System;

namespace InControl.NativeProfile
{
		public class RazerAtroxArcadeStickMacProfile : Xbox360DriverMacProfile
	{
				public RazerAtroxArcadeStickMacProfile()
		{
			base.Name = "Razer Atrox Arcade Stick";
			base.Meta = "Razer Atrox Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2560)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(20480)
				}
			};
		}
	}
}
