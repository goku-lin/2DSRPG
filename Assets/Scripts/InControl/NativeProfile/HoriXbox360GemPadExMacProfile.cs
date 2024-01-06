using System;

namespace InControl.NativeProfile
{
		public class HoriXbox360GemPadExMacProfile : Xbox360DriverMacProfile
	{
				public HoriXbox360GemPadExMacProfile()
		{
			base.Name = "Hori Xbox 360 Gem Pad Ex";
			base.Meta = "Hori Xbox 360 Gem Pad Ex on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21773)
				}
			};
		}
	}
}
