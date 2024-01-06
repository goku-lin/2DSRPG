using System;

namespace InControl.NativeProfile
{
		public class MadCatzNeoFightStickMacProfile : Xbox360DriverMacProfile
	{
				public MadCatzNeoFightStickMacProfile()
		{
			base.Name = "Mad Catz Neo Fight Stick";
			base.Meta = "Mad Catz Neo Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61498)
				}
			};
		}
	}
}
