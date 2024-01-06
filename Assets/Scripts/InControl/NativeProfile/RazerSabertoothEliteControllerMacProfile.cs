using System;

namespace InControl.NativeProfile
{
		public class RazerSabertoothEliteControllerMacProfile : Xbox360DriverMacProfile
	{
				public RazerSabertoothEliteControllerMacProfile()
		{
			base.Name = "Razer Sabertooth Elite Controller";
			base.Meta = "Razer Sabertooth Elite Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(65024)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23812)
				}
			};
		}
	}
}
