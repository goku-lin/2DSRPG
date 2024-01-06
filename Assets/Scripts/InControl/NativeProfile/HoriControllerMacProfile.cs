using System;

namespace InControl.NativeProfile
{
		public class HoriControllerMacProfile : Xbox360DriverMacProfile
	{
				public HoriControllerMacProfile()
		{
			base.Name = "Hori Controller";
			base.Meta = "Hori Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(21760)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(654)
				}
			};
		}
	}
}
