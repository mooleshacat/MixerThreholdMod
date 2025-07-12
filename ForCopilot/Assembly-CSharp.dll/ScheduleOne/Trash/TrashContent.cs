using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;

namespace ScheduleOne.Trash
{
	// Token: 0x0200086A RID: 2154
	[Serializable]
	public class TrashContent
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x000F8D34 File Offset: 0x000F6F34
		public void AddTrash(string trashID, int quantity)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				entry = new TrashContent.Entry(trashID, 0);
				this.Entries.Add(entry);
			}
			this.Entries.Remove(entry);
			this.Entries.Add(entry);
			entry.Quantity += quantity;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x000F8DAC File Offset: 0x000F6FAC
		public void RemoveTrash(string trashID, int quantity)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				return;
			}
			entry.Quantity -= quantity;
			if (entry.Quantity <= 0)
			{
				this.Entries.Remove(entry);
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x000F8E08 File Offset: 0x000F7008
		public int GetTrashQuantity(string trashID)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				return 0;
			}
			return entry.Quantity;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x000F8E45 File Offset: 0x000F7045
		public void Clear()
		{
			this.Entries.Clear();
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x000F8E54 File Offset: 0x000F7054
		public int GetTotalSize()
		{
			int num = 0;
			foreach (TrashContent.Entry entry in this.Entries)
			{
				num += entry.Quantity * entry.UnitSize;
			}
			return num;
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x000F8EB4 File Offset: 0x000F70B4
		public TrashContentData GetData()
		{
			TrashContentData trashContentData = new TrashContentData();
			trashContentData.TrashIDs = new string[this.Entries.Count];
			trashContentData.TrashQuantities = new int[this.Entries.Count];
			for (int i = 0; i < this.Entries.Count; i++)
			{
				trashContentData.TrashIDs[i] = this.Entries[i].TrashID;
				trashContentData.TrashQuantities[i] = this.Entries[i].Quantity;
			}
			return trashContentData;
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x000F8F3C File Offset: 0x000F713C
		public void LoadFromData(TrashContentData data)
		{
			for (int i = 0; i < data.TrashIDs.Length; i++)
			{
				this.AddTrash(data.TrashIDs[i], data.TrashQuantities[i]);
			}
		}

		// Token: 0x04002A26 RID: 10790
		public List<TrashContent.Entry> Entries = new List<TrashContent.Entry>();

		// Token: 0x0200086B RID: 2155
		[Serializable]
		public class Entry
		{
			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000F8F85 File Offset: 0x000F7185
			// (set) Token: 0x06003ACB RID: 15051 RVA: 0x000F8F8D File Offset: 0x000F718D
			public int UnitSize { get; private set; }

			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x06003ACC RID: 15052 RVA: 0x000F8F96 File Offset: 0x000F7196
			// (set) Token: 0x06003ACD RID: 15053 RVA: 0x000F8F9E File Offset: 0x000F719E
			public int UnitValue { get; private set; }

			// Token: 0x06003ACE RID: 15054 RVA: 0x000F8FA8 File Offset: 0x000F71A8
			public Entry(string id, int quantity)
			{
				this.TrashID = id;
				this.Quantity = quantity;
				TrashItem trashPrefab = NetworkSingleton<TrashManager>.Instance.GetTrashPrefab(id);
				if (trashPrefab != null)
				{
					this.UnitSize = trashPrefab.Size;
					this.UnitValue = trashPrefab.SellValue;
				}
			}

			// Token: 0x04002A27 RID: 10791
			public string TrashID;

			// Token: 0x04002A28 RID: 10792
			public int Quantity;
		}
	}
}
