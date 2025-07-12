using System;
using System.Collections.Generic;
using ScheduleOne.Economy;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F1 RID: 753
	public class DeaddropQuest : Quest
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0004B878 File Offset: 0x00049A78
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0004B880 File Offset: 0x00049A80
		public DeadDrop Drop { get; private set; }

		// Token: 0x060010F0 RID: 4336 RVA: 0x0004B889 File Offset: 0x00049A89
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			if (!DeaddropQuest.DeaddropQuests.Contains(this))
			{
				DeaddropQuest.DeaddropQuests.Add(this);
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0004B8AA File Offset: 0x00049AAA
		public void SetDrop(DeadDrop drop)
		{
			this.Drop = drop;
			this.Entries[0].SetPoILocation(this.Drop.transform.position);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0004B8D4 File Offset: 0x00049AD4
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Active && this.Drop.Storage.ItemCount == 0)
			{
				this.Entries[0].Complete();
				this.Complete(false);
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004B90F File Offset: 0x00049B0F
		private void OnDestroy()
		{
			Quest.Quests.Remove(this);
			DeaddropQuest.DeaddropQuests.Remove(this);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0004B929 File Offset: 0x00049B29
		public override void End()
		{
			base.End();
			DeaddropQuest.DeaddropQuests.Remove(this);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004B93D File Offset: 0x00049B3D
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
			if (state != EQuestState.Active)
			{
				Quest.Quests.Remove(this);
				DeaddropQuest.DeaddropQuests.Remove(this);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004833E File Offset: 0x0004653E
		public override bool ShouldSave()
		{
			return base.QuestState == EQuestState.Active;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0004B964 File Offset: 0x00049B64
		public override SaveData GetSaveData()
		{
			List<QuestEntryData> list = new List<QuestEntryData>();
			for (int i = 0; i < this.Entries.Count; i++)
			{
				list.Add(this.Entries[i].GetSaveData());
			}
			return new DeaddropQuestData(base.GUID.ToString(), base.QuestState, base.IsTracked, this.title, this.Description, base.Expires, new GameDateTimeData(base.Expiry), list.ToArray(), this.Drop.GUID.ToString());
		}

		// Token: 0x04001100 RID: 4352
		public static List<DeaddropQuest> DeaddropQuests = new List<DeaddropQuest>();
	}
}
