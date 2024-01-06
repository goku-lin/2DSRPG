using System;

namespace InControl.NativeProfile
{
		public class PDPBattlefieldXBoxOneControllerMacProfile : XboxOneDriverMacProfile
	{
				public PDPBattlefieldXBoxOneControllerMacProfile()
		{
			base.Name = "PDP Battlefield XBox One Controller";
			base.Meta = "PDP Battlefield XBox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(356)
				}
			};
		}
	}
}
