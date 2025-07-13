using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using ScheduleOne.Trash;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C4E RID: 3150
	[Serializable]
	public class TrashGrabberInstance : StorableItemInstance
	{
		// Token: 0x060058D9 RID: 22745 RVA: 0x00177B5F File Offset: 0x00175D5F
		public TrashGrabberInstance()
		{
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x00177B72 File Offset: 0x00175D72
		public TrashGrabberInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x00177B88 File Offset: 0x00175D88
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance(base.Definition, quantity);
			trashGrabberInstance.Content.LoadFromData(this.Content.GetData());
			return trashGrabberInstance;
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x00177BC4 File Offset: 0x00175DC4
		public void LoadContentData(TrashContentData content)
		{
			this.Content.LoadFromData(content);
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x00177BD2 File Offset: 0x00175DD2
		public override ItemData GetItemData()
		{
			return new TrashGrabberData(this.ID, this.Quantity, this.Content.GetData());
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x00177BF0 File Offset: 0x00175DF0
		public void AddTrash(string id, int quantity)
		{
			this.Content.AddTrash(id, quantity);
			base.InvokeDataChange();
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x00177C05 File Offset: 0x00175E05
		public void RemoveTrash(string id, int quantity)
		{
			this.Content.RemoveTrash(id, quantity);
			base.InvokeDataChange();
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x00177C1A File Offset: 0x00175E1A
		public void ClearTrash()
		{
			this.Content.Clear();
			base.InvokeDataChange();
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x00177C2D File Offset: 0x00175E2D
		public int GetTotalSize()
		{
			return this.Content.GetTotalSize();
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x00177C3C File Offset: 0x00175E3C
		public List<string> GetTrashIDs()
		{
			List<string> list = new List<string>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add(entry.TrashID);
			}
			return list;
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x00177CA0 File Offset: 0x00175EA0
		public List<int> GetTrashQuantities()
		{
			List<int> list = new List<int>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add(entry.Quantity);
			}
			return list;
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x00177D04 File Offset: 0x00175F04
		public List<ushort> GetTrashUshortQuantities()
		{
			List<ushort> list = new List<ushort>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add((ushort)entry.Quantity);
			}
			return list;
		}

		// Token: 0x0400410A RID: 16650
		public const int TRASH_CAPACITY = 20;

		// Token: 0x0400410B RID: 16651
		private TrashContent Content = new TrashContent();
	}
}
