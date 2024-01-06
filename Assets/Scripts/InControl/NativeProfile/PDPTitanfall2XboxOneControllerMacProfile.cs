using System;

namespace InControl.NativeProfile
{
		public class PDPTitanfall2XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public PDPTitanfall2XboxOneControllerMacProfile()
		{
			base.Name = "PDP Titanfall 2 Xbox One Controller";
			base.Meta = "PDP Titanfall 2 Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(357)
				}
			};
		}
	}
}
