using System;
using ScheduleOne.Management.Presets.Options;
using UnityEngine;

namespace ScheduleOne.Management.Presets
{
	// Token: 0x020005D2 RID: 1490
	public class PotPreset : Preset
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x00095C9C File Offset: 0x00093E9C
		// (set) Token: 0x060024A1 RID: 9377 RVA: 0x00095CA3 File Offset: 0x00093EA3
		protected static PotPreset DefaultPreset { get; set; }

		// Token: 0x060024A2 RID: 9378 RVA: 0x00095CAC File Offset: 0x00093EAC
		public override Preset GetCopy()
		{
			PotPreset potPreset = new PotPreset();
			this.CopyTo(potPreset);
			return potPreset;
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x00095CC8 File Offset: 0x00093EC8
		public override void CopyTo(Preset other)
		{
			base.CopyTo(other);
			if (other is PotPreset)
			{
				PotPreset potPreset = other as PotPreset;
				this.Seeds.CopyTo(potPreset.Seeds);
				this.Additives.CopyTo(potPreset.Additives);
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x00095D10 File Offset: 0x00093F10
		public override void InitializeOptions()
		{
			this.Seeds = new ItemList("Seed Types", ManagementUtilities.WeedSeedAssetPaths, true, true);
			this.Seeds.All = true;
			this.Additives = new ItemList("Additives", ManagementUtilities.AdditiveAssetPaths, true, true);
			this.Seeds.None = true;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00095D64 File Offset: 0x00093F64
		public static PotPreset GetDefaultPreset()
		{
			if (PotPreset.DefaultPreset == null)
			{
				PotPreset.DefaultPreset = new PotPreset
				{
					PresetName = "Default",
					ObjectType = ManageableObjectType.Pot,
					PresetColor = new Color32(180, 180, 180, byte.MaxValue)
				};
			}
			return PotPreset.DefaultPreset;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00095DB8 File Offset: 0x00093FB8
		public static PotPreset GetNewBlankPreset()
		{
			PotPreset potPreset = PotPreset.GetDefaultPreset().GetCopy() as PotPreset;
			potPreset.PresetName = "New Preset";
			return potPreset;
		}

		// Token: 0x04001B1F RID: 6943
		public ItemList Seeds;

		// Token: 0x04001B20 RID: 6944
		public ItemList Additives;
	}
}
