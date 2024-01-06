using System;

namespace InControl.NativeProfile
{
		public class RazerStrikeControllerMacProfile : Xbox360DriverMacProfile
	{
				public RazerStrikeControllerMacProfile()
		{
			base.Name = "Razer Strike Controller";
			base.Meta = "Razer Strike Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(1)
				}
			};
		}
	}
}
