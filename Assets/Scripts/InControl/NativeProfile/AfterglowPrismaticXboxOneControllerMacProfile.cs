using System;

namespace InControl.NativeProfile
{
		public class AfterglowPrismaticXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public AfterglowPrismaticXboxOneControllerMacProfile()
		{
			base.Name = "Afterglow Prismatic Xbox One Controller";
			base.Meta = "Afterglow Prismatic Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(313)
				}
			};
		}
	}
}
