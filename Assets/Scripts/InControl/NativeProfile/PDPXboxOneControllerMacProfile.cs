using System;

namespace InControl.NativeProfile
{
		public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public PDPXboxOneControllerMacProfile()
		{
			base.Name = "PDP Xbox One Controller";
			base.Meta = "PDP Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(314)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(354)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(353)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(355)
				}
			};
		}
	}
}
