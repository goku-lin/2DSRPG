using System;

namespace InControl.NativeProfile
{
		public class EASportsControllerMacProfile : Xbox360DriverMacProfile
	{
				public EASportsControllerMacProfile()
		{
			base.Name = "EA Sports Controller";
			base.Meta = "EA Sports Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(305)
				}
			};
		}
	}
}
