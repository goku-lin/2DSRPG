using System;

namespace InControl.NativeProfile
{
		public class HoriRealArcadeProHayabusaMacProfile : Xbox360DriverMacProfile
	{
				public HoriRealArcadeProHayabusaMacProfile()
		{
			base.Name = "Hori Real Arcade Pro Hayabusa";
			base.Meta = "Hori Real Arcade Pro Hayabusa on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(99)
				}
			};
		}
	}
}
