using System;

namespace InControl.NativeProfile
{
		public class LogitechControllerMacProfile : Xbox360DriverMacProfile
	{
				public LogitechControllerMacProfile()
		{
			base.Name = "Logitech Controller";
			base.Meta = "Logitech Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(62209)
				}
			};
		}
	}
}
