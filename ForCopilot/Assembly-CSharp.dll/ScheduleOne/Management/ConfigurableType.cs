using System;

namespace ScheduleOne.Management
{
	// Token: 0x020005AB RID: 1451
	public static class ConfigurableType
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x0009364C File Offset: 0x0009184C
		public static string GetTypeName(EConfigurableType type)
		{
			switch (type)
			{
			case EConfigurableType.Pot:
				return "Pot";
			case EConfigurableType.PackagingStation:
				return "Packaging Station";
			case EConfigurableType.LabOven:
				return "Lab Oven";
			case EConfigurableType.Botanist:
				return "Botanist";
			case EConfigurableType.Packager:
				return "Handler";
			case EConfigurableType.ChemistryStation:
				return "Chemistry Station";
			case EConfigurableType.Chemist:
				return "Chemist";
			case EConfigurableType.Cauldron:
				return "Cauldron";
			case EConfigurableType.Cleaner:
				return "Cleaner";
			case EConfigurableType.BrickPress:
				return "Brick Press";
			case EConfigurableType.MixingStation:
				return "Mixing Station";
			case EConfigurableType.DryingRack:
				return "Drying Rack";
			default:
				Console.LogError("Unknown Configurable Type: " + type.ToString(), null);
				return string.Empty;
			}
		}
	}
}
