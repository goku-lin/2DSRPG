using System;

namespace InControl.NativeProfile
{
		public class KonamiDancePadMacProfile : Xbox360DriverMacProfile
	{
				public KonamiDancePadMacProfile()
		{
			base.Name = "Konami Dance Pad";
			base.Meta = "Konami Dance Pad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(4)
				}
			};
		}
	}
}
