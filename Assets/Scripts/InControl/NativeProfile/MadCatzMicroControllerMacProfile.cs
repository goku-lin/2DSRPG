using System;

namespace InControl.NativeProfile
{
		public class MadCatzMicroControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzMicroControllerMacProfile()
		{
			base.Name = "Mad Catz Micro Controller";
			base.Meta = "Mad Catz Micro Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18230)
				}
			};
		}
	}
}
