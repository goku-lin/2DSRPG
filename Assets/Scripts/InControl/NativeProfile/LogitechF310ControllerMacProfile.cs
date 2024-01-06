using System;

namespace InControl.NativeProfile
{
		public class LogitechF310ControllerMacProfile : Xbox360DriverMacProfile
	{
				public LogitechF310ControllerMacProfile()
		{
			base.Name = "Logitech F310 Controller";
			base.Meta = "Logitech F310 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49693)
				}
			};
		}
	}
}
