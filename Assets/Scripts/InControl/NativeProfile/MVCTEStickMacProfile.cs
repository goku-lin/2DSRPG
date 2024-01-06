using System;

namespace InControl.NativeProfile
{
		public class MVCTEStickMacProfile : Xbox360DriverMacProfile
	{
				public MVCTEStickMacProfile()
		{
			base.Name = "MVC TE Stick";
			base.Meta = "MVC TE Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61497)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(46904)
				}
			};
		}
	}
}
