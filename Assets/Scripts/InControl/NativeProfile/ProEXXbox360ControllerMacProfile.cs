using System;

namespace InControl.NativeProfile
{
		public class ProEXXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public ProEXXbox360ControllerMacProfile()
		{
			base.Name = "Pro EX Xbox 360 Controller";
			base.Meta = "Pro EX Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21258)
				}
			};
		}
	}
}
