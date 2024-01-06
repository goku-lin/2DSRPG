using System;

namespace InControl.NativeProfile
{
		public class MadCatzControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzControllerMacProfile()
		{
			base.Name = "Mad Catz Controller";
			base.Meta = "Mad Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18198)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63746)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61642)
				}
			};
		}
	}
}
