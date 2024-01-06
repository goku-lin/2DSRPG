using System;

namespace InControl.NativeProfile
{
		public class PowerAMiniXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public PowerAMiniXboxOneControllerMacProfile()
		{
			base.Name = "Power A Mini Xbox One Controller";
			base.Meta = "Power A Mini Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21562)
				}
			};
		}
	}
}
