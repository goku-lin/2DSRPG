using System;

namespace InControl.NativeProfile
{
		public class BigBenControllerMacProfile : Xbox360DriverMacProfile
	{
				public BigBenControllerMacProfile()
		{
			base.Name = "Big Ben Controller";
			base.Meta = "Big Ben Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5227),
					ProductID = new ushort?(1537)
				}
			};
		}
	}
}
