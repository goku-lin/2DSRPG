using System;

namespace InControl.NativeProfile
{
		public class TSZPelicanControllerMacProfile : Xbox360DriverMacProfile
	{
				public TSZPelicanControllerMacProfile()
		{
			base.Name = "TSZ Pelican Controller";
			base.Meta = "TSZ Pelican Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(513)
				}
			};
		}
	}
}
