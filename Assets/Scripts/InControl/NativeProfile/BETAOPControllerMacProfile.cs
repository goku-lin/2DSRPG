using System;

namespace InControl.NativeProfile
{
		public class BETAOPControllerMacProfile : Xbox360DriverMacProfile
	{
				public BETAOPControllerMacProfile()
		{
			base.Name = "BETAOP Controller";
			base.Meta = "BETAOP Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4544),
					ProductID = new ushort?(21766)
				}
			};
		}
	}
}
