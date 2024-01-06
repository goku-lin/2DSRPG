using System;

namespace InControl.NativeProfile
{
		public class MatCatzControllerMacProfile : Xbox360DriverMacProfile
	{
				public MatCatzControllerMacProfile()
		{
			base.Name = "Mat Catz Controller";
			base.Meta = "Mat Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61462)
				}
			};
		}
	}
}
