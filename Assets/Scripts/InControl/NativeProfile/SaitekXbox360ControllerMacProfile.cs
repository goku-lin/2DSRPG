using System;

namespace InControl.NativeProfile
{
		public class SaitekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public SaitekXbox360ControllerMacProfile()
		{
			base.Name = "Saitek Xbox 360 Controller";
			base.Meta = "Saitek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(51970)
				}
			};
		}
	}
}
