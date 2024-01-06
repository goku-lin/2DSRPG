using System;

namespace InControl.NativeProfile
{
		public class MicrosoftXboxControllerMacProfile : Xbox360DriverMacProfile
	{
				public MicrosoftXboxControllerMacProfile()
		{
			base.Name = "Microsoft Xbox Controller";
			base.Meta = "Microsoft Xbox Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(ushort.MaxValue),
					ProductID = new ushort?(ushort.MaxValue)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(649)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(645)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(514)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(647)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				}
			};
		}
	}
}
