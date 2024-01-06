using System;

namespace InControl.NativeProfile
{
		public class RockCandyXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public RockCandyXbox360ControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox 360 Controller";
			base.Meta = "Rock Candy Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(543)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64254)
				}
			};
		}
	}
}
