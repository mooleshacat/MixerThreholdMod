using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044F RID: 1103
	public class MixingStationData : GridItemData
	{
		// Token: 0x06001686 RID: 5766 RVA: 0x00064268 File Offset: 0x00062468
		public MixingStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet productContents, ItemSet mixerContents, ItemSet outputContents, MixOperation currentMixOperation, int currentMixTime) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.ProductContents = productContents;
			this.MixerContents = mixerContents;
			this.OutputContents = outputContents;
			this.CurrentMixOperation = currentMixOperation;
			this.CurrentMixTime = currentMixTime;
		}

		// Token: 0x04001496 RID: 5270
		public ItemSet ProductContents;

		// Token: 0x04001497 RID: 5271
		public ItemSet MixerContents;

		// Token: 0x04001498 RID: 5272
		public ItemSet OutputContents;

		// Token: 0x04001499 RID: 5273
		public MixOperation CurrentMixOperation;

		// Token: 0x0400149A RID: 5274
		public int CurrentMixTime;
	}
}
