using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C25 RID: 3109
	[Serializable]
	public class DryingOperation
	{
		// Token: 0x06005515 RID: 21781 RVA: 0x00167BED File Offset: 0x00165DED
		public DryingOperation(string itemID, int quantity, EQuality startQuality, int time)
		{
			this.ItemID = itemID;
			this.Quantity = quantity;
			this.StartQuality = startQuality;
			this.Time = time;
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x0000494F File Offset: 0x00002B4F
		public DryingOperation()
		{
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x00167C12 File Offset: 0x00165E12
		public void IncreaseQuality()
		{
			this.StartQuality++;
			this.Time = 0;
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x00167C29 File Offset: 0x00165E29
		public QualityItemInstance GetQualityItemInstance()
		{
			QualityItemInstance qualityItemInstance = Registry.GetItem(this.ItemID).GetDefaultInstance(this.Quantity) as QualityItemInstance;
			qualityItemInstance.SetQuality(this.StartQuality);
			return qualityItemInstance;
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x00167C52 File Offset: 0x00165E52
		public EQuality GetQuality()
		{
			if (this.Time >= 720)
			{
				return this.StartQuality + 1;
			}
			return this.StartQuality;
		}

		// Token: 0x04003F3C RID: 16188
		public string ItemID;

		// Token: 0x04003F3D RID: 16189
		public int Quantity;

		// Token: 0x04003F3E RID: 16190
		public EQuality StartQuality;

		// Token: 0x04003F3F RID: 16191
		public int Time;
	}
}
