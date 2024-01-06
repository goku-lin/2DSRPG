using System;

namespace InControl.NativeProfile
{
		public class HoriPadEXTurboControllerMacProfile : Xbox360DriverMacProfile
	{
				public HoriPadEXTurboControllerMacProfile()
		{
			base.Name = "Hori Pad EX Turbo Controller";
			base.Meta = "Hori Pad EX Turbo Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(12)
				}
			};
		}
	}
}
