using System;

namespace InControl.NativeProfile
{
		public class MadCatzPortableDrumMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzPortableDrumMacProfile()
		{
			base.Name = "Mad Catz Portable Drum";
			base.Meta = "Mad Catz Portable Drum on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(39025)
				}
			};
		}
	}
}
