using System;

namespace InControl.NativeProfile
{
		public class POWERAFUS1ONTournamentControllerMacProfile : Xbox360DriverMacProfile
	{
				public POWERAFUS1ONTournamentControllerMacProfile()
		{
			base.Name = "POWER A FUS1ON Tournament Controller";
			base.Meta = "POWER A FUS1ON Tournament Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21399)
				}
			};
		}
	}
}
