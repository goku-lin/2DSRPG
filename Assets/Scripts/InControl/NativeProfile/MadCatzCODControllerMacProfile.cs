using System;

namespace InControl.NativeProfile
{
		public class MadCatzCODControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzCODControllerMacProfile()
		{
			base.Name = "Mad Catz COD Controller";
			base.Meta = "Mad Catz COD Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61477)
				}
			};
		}
	}
}
