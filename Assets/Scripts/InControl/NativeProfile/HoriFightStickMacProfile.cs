using System;

namespace InControl.NativeProfile
{
		public class HoriFightStickMacProfile : Xbox360DriverMacProfile
	{
				public HoriFightStickMacProfile()
		{
			base.Name = "Hori Fight Stick";
			base.Meta = "Hori Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
