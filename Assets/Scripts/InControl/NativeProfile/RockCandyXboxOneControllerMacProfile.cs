using System;

namespace InControl.NativeProfile
{
		public class RockCandyXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public RockCandyXboxOneControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox One Controller";
			base.Meta = "Rock Candy Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(326)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(582)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(838)
				}
			};
		}
	}
}
