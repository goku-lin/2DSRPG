using System;

namespace InControl.NativeProfile
{
		public class HoriBlueSoloControllerMacProfile : Xbox360DriverMacProfile
	{
				public HoriBlueSoloControllerMacProfile()
		{
			base.Name = "Hori Blue Solo Controller ";
			base.Meta = "Hori Blue Solo Controller\ton Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64001)
				}
			};
		}
	}
}
