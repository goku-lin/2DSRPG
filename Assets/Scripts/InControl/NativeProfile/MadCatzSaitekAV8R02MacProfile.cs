using System;

namespace InControl.NativeProfile
{
		public class MadCatzSaitekAV8R02MacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSaitekAV8R02MacProfile()
		{
			base.Name = "Mad Catz Saitek AV8R02";
			base.Meta = "Mad Catz Saitek AV8R02 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(52009)
				}
			};
		}
	}
}
