using System;

namespace InControl.NativeProfile
{
		public class HoriEX2ControllerMacProfile : Xbox360DriverMacProfile
	{
				public HoriEX2ControllerMacProfile()
		{
			base.Name = "Hori EX2 Controller";
			base.Meta = "Hori EX2 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21760)
				}
			};
		}
	}
}
