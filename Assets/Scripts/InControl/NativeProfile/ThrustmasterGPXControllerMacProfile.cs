using System;

namespace InControl.NativeProfile
{
		public class ThrustmasterGPXControllerMacProfile : Xbox360DriverMacProfile
	{
				public ThrustmasterGPXControllerMacProfile()
		{
			base.Name = "Thrustmaster GPX Controller";
			base.Meta = "Thrustmaster GPX Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1103),
					ProductID = new ushort?(45862)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23298)
				}
			};
		}
	}
}
