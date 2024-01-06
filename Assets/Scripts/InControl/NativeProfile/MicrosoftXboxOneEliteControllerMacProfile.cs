using System;

namespace InControl.NativeProfile
{
		public class MicrosoftXboxOneEliteControllerMacProfile : XboxOneDriverMacProfile
	{
				public MicrosoftXboxOneEliteControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Elite Controller";
			base.Meta = "Microsoft Xbox One Elite Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(739)
				}
			};
		}
	}
}
