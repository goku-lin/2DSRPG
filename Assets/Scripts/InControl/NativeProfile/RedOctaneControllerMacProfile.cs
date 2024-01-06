using System;

namespace InControl.NativeProfile
{
		public class RedOctaneControllerMacProfile : Xbox360DriverMacProfile
	{
				public RedOctaneControllerMacProfile()
		{
			base.Name = "Red Octane Controller";
			base.Meta = "Red Octane Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(63489)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(672)
				}
			};
		}
	}
}
