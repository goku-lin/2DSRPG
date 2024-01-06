using System;

namespace InControl.NativeProfile
{
		public class NaconGC100XFControllerMacProfile : Xbox360DriverMacProfile
	{
				public NaconGC100XFControllerMacProfile()
		{
			base.Name = "Nacon GC-100XF Controller";
			base.Meta = "Nacon GC-100XF Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4553),
					ProductID = new ushort?(22000)
				}
			};
		}
	}
}
