using System;

namespace InControl.NativeProfile
{
		public class LogitechF510ControllerMacProfile : Xbox360DriverMacProfile
	{
				public LogitechF510ControllerMacProfile()
		{
			base.Name = "Logitech F510 Controller";
			base.Meta = "Logitech F510 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49694)
				}
			};
		}
	}
}
