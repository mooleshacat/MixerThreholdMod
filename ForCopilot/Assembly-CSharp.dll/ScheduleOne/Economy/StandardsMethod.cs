using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Economy
{
	// Token: 0x020006B4 RID: 1716
	public static class StandardsMethod
	{
		// Token: 0x06002EEF RID: 12015 RVA: 0x000C4BAC File Offset: 0x000C2DAC
		public static string GetName(this ECustomerStandard property)
		{
			switch (property)
			{
			case ECustomerStandard.VeryLow:
				return "Very Low";
			case ECustomerStandard.Low:
				return "Low";
			case ECustomerStandard.Moderate:
				return "Moderate";
			case ECustomerStandard.High:
				return "High";
			case ECustomerStandard.VeryHigh:
				return "Very High";
			default:
				return "Standard";
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000C4BF8 File Offset: 0x000C2DF8
		public static EQuality GetCorrespondingQuality(this ECustomerStandard property)
		{
			switch (property)
			{
			case ECustomerStandard.VeryLow:
				return EQuality.Trash;
			case ECustomerStandard.Low:
				return EQuality.Poor;
			case ECustomerStandard.Moderate:
				return EQuality.Standard;
			case ECustomerStandard.High:
				return EQuality.Premium;
			case ECustomerStandard.VeryHigh:
				return EQuality.Heavenly;
			default:
				return EQuality.Standard;
			}
		}
	}
}
