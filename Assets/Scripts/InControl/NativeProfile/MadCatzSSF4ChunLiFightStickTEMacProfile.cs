using System;

namespace InControl.NativeProfile
{
		public class MadCatzSSF4ChunLiFightStickTEMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzSSF4ChunLiFightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Chun-Li Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Chun-Li Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61501)
				}
			};
		}
	}
}
