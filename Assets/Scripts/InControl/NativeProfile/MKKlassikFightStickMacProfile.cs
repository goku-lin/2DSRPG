using System;

namespace InControl.NativeProfile
{
		public class MKKlassikFightStickMacProfile : Xbox360DriverMacProfile
	{
				public MKKlassikFightStickMacProfile()
		{
			base.Name = "MK Klassik Fight Stick";
			base.Meta = "MK Klassik Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(771)
				}
			};
		}
	}
}
