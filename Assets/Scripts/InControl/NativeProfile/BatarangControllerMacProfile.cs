using System;

namespace InControl.NativeProfile
{
		public class BatarangControllerMacProfile : Xbox360DriverMacProfile
	{
				public BatarangControllerMacProfile()
		{
			base.Name = "Batarang Controller";
			base.Meta = "Batarang Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16144)
				}
			};
		}
	}
}
