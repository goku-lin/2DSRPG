using System;

namespace InControl.NativeProfile
{
		public class LogitechChillStreamControllerMacProfile : Xbox360DriverMacProfile
	{
				public LogitechChillStreamControllerMacProfile()
		{
			base.Name = "Logitech Chill Stream Controller";
			base.Meta = "Logitech Chill Stream Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49730)
				}
			};
		}
	}
}
