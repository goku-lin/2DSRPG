using System;

namespace InControl.NativeProfile
{
		public class LogitechF710ControllerMacProfile : Xbox360DriverMacProfile
	{
				public LogitechF710ControllerMacProfile()
		{
			base.Name = "Logitech F710 Controller";
			base.Meta = "Logitech F710 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49695)
				}
			};
		}
	}
}
