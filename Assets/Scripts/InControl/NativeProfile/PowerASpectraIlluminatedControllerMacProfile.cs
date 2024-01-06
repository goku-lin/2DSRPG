using System;

namespace InControl.NativeProfile
{
		public class PowerASpectraIlluminatedControllerMacProfile : Xbox360DriverMacProfile
	{
				public PowerASpectraIlluminatedControllerMacProfile()
		{
			base.Name = "PowerA Spectra Illuminated Controller";
			base.Meta = "PowerA Spectra Illuminated Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21546)
				}
			};
		}
	}
}
