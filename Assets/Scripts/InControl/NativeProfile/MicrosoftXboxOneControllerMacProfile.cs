using System;

namespace InControl.NativeProfile
{
		public class MicrosoftXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public MicrosoftXboxOneControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Controller";
			base.Meta = "Microsoft Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(733)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(746)
				}
			};
		}
	}
}
