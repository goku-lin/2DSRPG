using System;

namespace InControl.NativeProfile
{
		public class ProEXXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public ProEXXboxOneControllerMacProfile()
		{
			base.Name = "Pro EX Xbox One Controller";
			base.Meta = "Pro EX Xbox One Controller on Mac";
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
