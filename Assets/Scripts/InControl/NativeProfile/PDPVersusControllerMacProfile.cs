using System;

namespace InControl.NativeProfile
{
		public class PDPVersusControllerMacProfile : Xbox360DriverMacProfile
	{
				public PDPVersusControllerMacProfile()
		{
			base.Name = "PDP Versus Controller";
			base.Meta = "PDP Versus Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63748)
				}
			};
		}
	}
}
