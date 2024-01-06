using System;

namespace InControl.NativeProfile
{
		public class PDPXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
				public PDPXbox360ControllerMacProfile()
		{
			base.Name = "PDP Xbox 360 Controller";
			base.Meta = "PDP Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1281)
				}
			};
		}
	}
}
