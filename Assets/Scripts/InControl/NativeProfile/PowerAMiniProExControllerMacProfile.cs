using System;

namespace InControl.NativeProfile
{
		public class PowerAMiniProExControllerMacProfile : Xbox360DriverMacProfile
	{
				public PowerAMiniProExControllerMacProfile()
		{
			base.Name = "PowerA Mini Pro Ex Controller";
			base.Meta = "PowerA Mini Pro Ex Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16128)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21274)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21248)
				}
			};
		}
	}
}
