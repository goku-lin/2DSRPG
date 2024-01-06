using System;

namespace InControl.NativeProfile
{
		public class RazerWildcatControllerMacProfile : Xbox360DriverMacProfile
	{
				public RazerWildcatControllerMacProfile()
		{
			base.Name = "Razer Wildcat Controller";
			base.Meta = "Razer Wildcat Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2563)
				}
			};
		}
	}
}
