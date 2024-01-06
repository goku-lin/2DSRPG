using System;

namespace InControl.NativeProfile
{
		public class RockCandyControllerMacProfile : Xbox360DriverMacProfile
	{
				public RockCandyControllerMacProfile()
		{
			base.Name = "Rock Candy Controller";
			base.Meta = "Rock Candy Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(287)
				}
			};
		}
	}
}
