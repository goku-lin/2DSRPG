using System;

namespace InControl.NativeProfile
{
		public class GameStopControllerMacProfile : Xbox360DriverMacProfile
	{
				public GameStopControllerMacProfile()
		{
			base.Name = "GameStop Controller";
			base.Meta = "GameStop Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1025)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(770)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63745)
				}
			};
		}
	}
}
