using System;

namespace InControl.NativeProfile
{
		public class MadCatzFightStickTE2MacProfile : Xbox360DriverMacProfile
	{
				public MadCatzFightStickTE2MacProfile()
		{
			base.Name = "Mad Catz Fight Stick TE2";
			base.Meta = "Mad Catz Fight Stick TE2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61568)
				}
			};
		}
	}
}
