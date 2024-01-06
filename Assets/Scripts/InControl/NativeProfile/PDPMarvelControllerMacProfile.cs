using System;

namespace InControl.NativeProfile
{
		public class PDPMarvelControllerMacProfile : Xbox360DriverMacProfile
	{
				public PDPMarvelControllerMacProfile()
		{
			base.Name = "PDP Marvel Controller";
			base.Meta = "PDP Marvel Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(327)
				}
			};
		}
	}
}
