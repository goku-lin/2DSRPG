using System;

namespace InControl.NativeProfile
{
		public class QanbaFightStickPlusMacProfile : Xbox360DriverMacProfile
	{
				public QanbaFightStickPlusMacProfile()
		{
			base.Name = "Qanba Fight Stick Plus";
			base.Meta = "Qanba Fight Stick Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
