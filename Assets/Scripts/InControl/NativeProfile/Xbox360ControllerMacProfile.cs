using System;

namespace InControl.NativeProfile
{
		public class Xbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public Xbox360ControllerMacProfile()
		{
			base.Name = "Xbox 360 Controller";
			base.Meta = "Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(62721)
				}
			};
		}
	}
}
