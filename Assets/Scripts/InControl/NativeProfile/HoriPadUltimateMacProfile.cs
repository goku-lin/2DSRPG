using System;

namespace InControl.NativeProfile
{
		public class HoriPadUltimateMacProfile : Xbox360DriverMacProfile
	{
				public HoriPadUltimateMacProfile()
		{
			base.Name = "HoriPad Ultimate";
			base.Meta = "HoriPad Ultimate on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(144)
				}
			};
		}
	}
}
