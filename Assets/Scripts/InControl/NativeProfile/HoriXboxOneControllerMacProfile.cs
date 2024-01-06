using System;

namespace InControl.NativeProfile
{
		public class HoriXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public HoriXboxOneControllerMacProfile()
		{
			base.Name = "Hori Xbox One Controller";
			base.Meta = "Hori Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(103)
				}
			};
		}
	}
}
