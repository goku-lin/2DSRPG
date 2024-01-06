using System;

namespace InControl.NativeProfile
{
		public class MadCatzMicroConControllerMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzMicroConControllerMacProfile()
		{
			base.Name = "Mad Catz MicroCon Controller";
			base.Meta = "Mad Catz MicroCon Controller on Mac";
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
