using System;

namespace InControl.NativeProfile
{
		public class PowerAAirflowControllerMacProfile : Xbox360DriverMacProfile
	{
				public PowerAAirflowControllerMacProfile()
		{
			base.Name = "PowerA Airflow Controller";
			base.Meta = "PowerA Airflow Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16138)
				}
			};
		}
	}
}
