using System;

namespace InControl.NativeProfile
{
		public class PowerAMiniControllerMacProfile : Xbox360DriverMacProfile
	{
				public PowerAMiniControllerMacProfile()
		{
			base.Name = "PowerA Mini Controller";
			base.Meta = "PowerA Mini Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21530)
				}
			};
		}
	}
}
