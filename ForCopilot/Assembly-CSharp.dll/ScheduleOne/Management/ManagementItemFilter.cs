using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Management
{
	// Token: 0x020005C7 RID: 1479
	public class ManagementItemFilter
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x00095335 File Offset: 0x00093535
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x0009533D File Offset: 0x0009353D
		public ManagementItemFilter.EMode Mode { get; private set; } = ManagementItemFilter.EMode.Blacklist;

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x00095346 File Offset: 0x00093546
		// (set) Token: 0x0600246D RID: 9325 RVA: 0x0009534E File Offset: 0x0009354E
		public List<ItemDefinition> Items { get; private set; } = new List<ItemDefinition>();

		// Token: 0x0600246E RID: 9326 RVA: 0x00095357 File Offset: 0x00093557
		public ManagementItemFilter(ManagementItemFilter.EMode mode)
		{
			this.Mode = mode;
			this.Items = new List<ItemDefinition>();
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x00095383 File Offset: 0x00093583
		public void SetMode(ManagementItemFilter.EMode mode)
		{
			this.Mode = mode;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0009538C File Offset: 0x0009358C
		public void AddItem(ItemDefinition item)
		{
			this.Items.Add(item);
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0009539A File Offset: 0x0009359A
		public void RemoveItem(ItemDefinition item)
		{
			this.Items.Remove(item);
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000953A9 File Offset: 0x000935A9
		public bool Contains(ItemDefinition item)
		{
			return this.Items.Contains(item);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000953B7 File Offset: 0x000935B7
		public bool DoesItemMeetFilter(ItemInstance item)
		{
			if (this.Mode != ManagementItemFilter.EMode.Whitelist)
			{
				return !this.Items.Contains(item.Definition);
			}
			return this.Items.Contains(item.Definition);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000953E8 File Offset: 0x000935E8
		public string GetDescription()
		{
			if (this.Mode == ManagementItemFilter.EMode.Blacklist)
			{
				if (this.Items.Count == 0)
				{
					return "All";
				}
				return this.Items.Count.ToString() + " blacklisted";
			}
			else
			{
				if (this.Items.Count == 0)
				{
					return "None";
				}
				return this.Items.Count.ToString() + " whitelisted";
			}
		}

		// Token: 0x020005C8 RID: 1480
		public enum EMode
		{
			// Token: 0x04001B08 RID: 6920
			Whitelist,
			// Token: 0x04001B09 RID: 6921
			Blacklist
		}
	}
}
