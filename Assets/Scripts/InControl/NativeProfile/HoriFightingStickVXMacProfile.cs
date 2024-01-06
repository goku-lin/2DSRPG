using System;

namespace InControl.NativeProfile
{
		public class HoriFightingStickVXMacProfile : Xbox360DriverMacProfile
	{
				public HoriFightingStickVXMacProfile()
		{
			base.Name = "Hori Fighting Stick VX";
			base.Meta = "Hori Fighting Stick VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62723)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21762)
				}
			};
		}
	}
}
