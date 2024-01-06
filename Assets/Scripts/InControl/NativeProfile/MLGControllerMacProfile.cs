using System;

namespace InControl.NativeProfile
{
		public class MLGControllerMacProfile : Xbox360DriverMacProfile
	{
				public MLGControllerMacProfile()
		{
			base.Name = "MLG Controller";
			base.Meta = "MLG Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61475)
				}
			};
		}
	}
}
