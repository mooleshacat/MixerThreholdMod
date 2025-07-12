using System;
using ScheduleOne.Economy;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FA RID: 762
	public class Quest_GearingUp : Quest
	{
		// Token: 0x06001112 RID: 4370 RVA: 0x0004C0C9 File Offset: 0x0004A2C9
		protected override void Start()
		{
			base.Start();
			this.Supplier.onDeaddropReady.AddListener(new UnityAction(this.DropReady));
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0004C0F0 File Offset: 0x0004A2F0
		protected override void MinPass()
		{
			base.MinPass();
			if (this.CollectDropEntry.State == EQuestState.Active && !this.setCollectionPosition)
			{
				DeadDrop deadDrop = DeadDrop.DeadDrops.Find((DeadDrop x) => x.Storage.ItemCount > 0);
				if (deadDrop != null)
				{
					this.setCollectionPosition = true;
					this.CollectDropEntry.SetPoILocation(deadDrop.transform.position);
				}
			}
			if (this.WaitForDropEntry.State == EQuestState.Active)
			{
				float num = (float)this.Supplier.minsUntilDeaddropReady;
				if (num > 0f)
				{
					this.WaitForDropEntry.SetEntryTitle("Wait for the dead drop (" + num.ToString() + " mins)");
					return;
				}
				this.WaitForDropEntry.SetEntryTitle("Wait for the dead drop");
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0004C1BE File Offset: 0x0004A3BE
		private void DropReady()
		{
			if (this.WaitForDropEntry.State == EQuestState.Active)
			{
				this.WaitForDropEntry.Complete();
				this.MinPass();
			}
		}

		// Token: 0x0400110E RID: 4366
		public QuestEntry WaitForDropEntry;

		// Token: 0x0400110F RID: 4367
		public QuestEntry CollectDropEntry;

		// Token: 0x04001110 RID: 4368
		public Supplier Supplier;

		// Token: 0x04001111 RID: 4369
		private bool setCollectionPosition;
	}
}
