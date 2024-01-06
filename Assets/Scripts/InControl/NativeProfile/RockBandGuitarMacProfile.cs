using System;

namespace InControl.NativeProfile
{
		public class RockBandGuitarMacProfile : Xbox360DriverMacProfile
	{
				public RockBandGuitarMacProfile()
		{
			base.Name = "Rock Band Guitar";
			base.Meta = "Rock Band Guitar on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(2)
				}
			};
		}
	}
}
