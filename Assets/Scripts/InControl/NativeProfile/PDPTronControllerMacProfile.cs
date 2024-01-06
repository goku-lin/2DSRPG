using System;

namespace InControl.NativeProfile
{
		public class PDPTronControllerMacProfile : Xbox360DriverMacProfile
	{
				public PDPTronControllerMacProfile()
		{
			base.Name = "PDP Tron Controller";
			base.Meta = "PDP Tron Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63747)
				}
			};
		}
	}
}
