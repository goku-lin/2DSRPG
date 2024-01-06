using System;

namespace InControl.NativeProfile
{
		public class IonDrumRockerMacProfile : Xbox360DriverMacProfile
	{
				public IonDrumRockerMacProfile()
		{
			base.Name = "Ion Drum Rocker";
			base.Meta = "Ion Drum Rocker on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(304)
				}
			};
		}
	}
}
