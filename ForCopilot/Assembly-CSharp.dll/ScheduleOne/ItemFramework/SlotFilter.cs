using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000996 RID: 2454
	[Serializable]
	public class SlotFilter
	{
		// Token: 0x06004250 RID: 16976 RVA: 0x00116CB8 File Offset: 0x00114EB8
		public SlotFilter()
		{
			this.Type = SlotFilter.EType.None;
			this.ItemIDs = new List<string>();
			this.AllowedQualities = new List<EQuality>
			{
				EQuality.Trash,
				EQuality.Poor,
				EQuality.Standard,
				EQuality.Premium,
				EQuality.Heavenly
			};
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x00116D24 File Offset: 0x00114F24
		public bool DoesItemMatchFilter(ItemInstance instance)
		{
			return (this.Type != SlotFilter.EType.Whitelist || this.ItemIDs.Contains(instance.ID)) && (this.Type != SlotFilter.EType.Blacklist || !this.ItemIDs.Contains(instance.ID)) && (!(instance is QualityItemInstance) || this.AllowedQualities.Contains(((QualityItemInstance)instance).Quality));
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x00116D90 File Offset: 0x00114F90
		public bool IsDefault()
		{
			return this.Type == SlotFilter.EType.None && this.ItemIDs.Count == 0 && this.AllowedQualities.Count == 5;
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x00116DB7 File Offset: 0x00114FB7
		public SlotFilter Clone()
		{
			return new SlotFilter
			{
				Type = this.Type,
				ItemIDs = new List<string>(this.ItemIDs),
				AllowedQualities = new List<EQuality>(this.AllowedQualities)
			};
		}

		// Token: 0x04002F1C RID: 12060
		public SlotFilter.EType Type;

		// Token: 0x04002F1D RID: 12061
		public List<string> ItemIDs = new List<string>();

		// Token: 0x04002F1E RID: 12062
		public List<EQuality> AllowedQualities = new List<EQuality>();

		// Token: 0x02000997 RID: 2455
		public enum EType
		{
			// Token: 0x04002F20 RID: 12064
			None,
			// Token: 0x04002F21 RID: 12065
			Whitelist,
			// Token: 0x04002F22 RID: 12066
			Blacklist
		}
	}
}
