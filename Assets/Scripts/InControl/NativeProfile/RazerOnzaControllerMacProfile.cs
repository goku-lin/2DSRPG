using System;

namespace InControl.NativeProfile
{
		public class RazerOnzaControllerMacProfile : Xbox360DriverMacProfile
	{
				public RazerOnzaControllerMacProfile()
		{
			base.Name = "Razer Onza Controller";
			base.Meta = "Razer Onza Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64769)
				}
			};
		}
	}
}
