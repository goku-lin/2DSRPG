using System;

namespace InControl.NativeProfile
{
		public class RazerOnzaTEControllerMacProfile : Xbox360DriverMacProfile
	{
				public RazerOnzaTEControllerMacProfile()
		{
			base.Name = "Razer Onza TE Controller";
			base.Meta = "Razer Onza TE Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64768)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64768)
				}
			};
		}
	}
}
