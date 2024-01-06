using System;

namespace InControl.NativeProfile
{
		public class JoytekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public JoytekXbox360ControllerMacProfile()
		{
			base.Name = "Joytek Xbox 360 Controller";
			base.Meta = "Joytek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5678),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
