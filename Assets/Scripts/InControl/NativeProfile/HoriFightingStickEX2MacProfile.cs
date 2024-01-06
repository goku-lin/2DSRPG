using System;

namespace InControl.NativeProfile
{
		public class HoriFightingStickEX2MacProfile : Xbox360DriverMacProfile
	{
				public HoriFightingStickEX2MacProfile()
		{
			base.Name = "Hori Fighting Stick EX2";
			base.Meta = "Hori Fighting Stick EX2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62725)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
