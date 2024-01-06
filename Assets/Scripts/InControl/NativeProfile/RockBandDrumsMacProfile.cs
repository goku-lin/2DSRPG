using System;

namespace InControl.NativeProfile
{
		public class RockBandDrumsMacProfile : Xbox360DriverMacProfile
	{
				public RockBandDrumsMacProfile()
		{
			base.Name = "Rock Band Drums";
			base.Meta = "Rock Band Drums on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
