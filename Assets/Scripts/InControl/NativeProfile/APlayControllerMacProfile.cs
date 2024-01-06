using System;

namespace InControl.NativeProfile
{
		public class APlayControllerMacProfile : Xbox360DriverMacProfile
	{
				public APlayControllerMacProfile()
		{
			base.Name = "A Play Controller";
			base.Meta = "A Play Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64251)
				}
			};
		}
	}
}
