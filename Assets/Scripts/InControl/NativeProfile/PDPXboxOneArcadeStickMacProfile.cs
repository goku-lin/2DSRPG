using System;

namespace InControl.NativeProfile
{
		public class PDPXboxOneArcadeStickMacProfile : XboxOneDriverMacProfile
	{
				public PDPXboxOneArcadeStickMacProfile()
		{
			base.Name = "PDP Xbox One Arcade Stick";
			base.Meta = "PDP Xbox One Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(348)
				}
			};
		}
	}
}
