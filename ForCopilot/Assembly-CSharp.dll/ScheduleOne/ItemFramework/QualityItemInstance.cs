using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000995 RID: 2453
	[Serializable]
	public class QualityItemInstance : StorableItemInstance
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x00116BD8 File Offset: 0x00114DD8
		public QualityItemInstance()
		{
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00116BE7 File Offset: 0x00114DE7
		public QualityItemInstance(ItemDefinition definition, int quantity, EQuality quality) : base(definition, quantity)
		{
			this.definition = definition;
			this.Quantity = quantity;
			this.ID = definition.ID;
			this.Quality = quality;
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x00116C1C File Offset: 0x00114E1C
		public override bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			QualityItemInstance qualityItemInstance = other as QualityItemInstance;
			return qualityItemInstance != null && qualityItemInstance.Quality == this.Quality && base.CanStackWith(other, checkQuantities);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x00116C4C File Offset: 0x00114E4C
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new QualityItemInstance(base.Definition, quantity, this.Quality);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x00116C78 File Offset: 0x00114E78
		public override ItemData GetItemData()
		{
			return new QualityItemData(this.ID, this.Quantity, this.Quality.ToString());
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x00116C9C File Offset: 0x00114E9C
		public void SetQuality(EQuality quality)
		{
			this.Quality = quality;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x04002F1B RID: 12059
		public EQuality Quality = EQuality.Standard;
	}
}
