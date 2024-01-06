using System;

namespace InControl.NativeProfile
{
		public class XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public XboxOneControllerMacProfile()
		{
			base.Name = "Xbox One Controller";
			base.Meta = "Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21786)
				}
			};
		}
	}
}
