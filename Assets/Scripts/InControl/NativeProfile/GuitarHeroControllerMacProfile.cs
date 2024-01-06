using System;

namespace InControl.NativeProfile
{
		public class GuitarHeroControllerMacProfile : Xbox360DriverMacProfile
	{
				public GuitarHeroControllerMacProfile()
		{
			base.Name = "Guitar Hero Controller";
			base.Meta = "Guitar Hero Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(18248)
				}
			};
		}
	}
}
