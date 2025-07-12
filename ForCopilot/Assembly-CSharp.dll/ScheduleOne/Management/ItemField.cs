using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005AD RID: 1453
	public class ItemField : ConfigField
	{
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x0009372C File Offset: 0x0009192C
		// (set) Token: 0x060023B4 RID: 9140 RVA: 0x00093734 File Offset: 0x00091934
		public ItemDefinition SelectedItem { get; protected set; }

		// Token: 0x060023B5 RID: 9141 RVA: 0x0009373D File Offset: 0x0009193D
		public ItemField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00093763 File Offset: 0x00091963
		public void SetItem(ItemDefinition item, bool network)
		{
			this.SelectedItem = item;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onItemChanged != null)
			{
				this.onItemChanged.Invoke(this.SelectedItem);
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x00093795 File Offset: 0x00091995
		public override bool IsValueDefault()
		{
			return this.SelectedItem == null;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000937A3 File Offset: 0x000919A3
		public ItemFieldData GetData()
		{
			return new ItemFieldData((this.SelectedItem != null) ? this.SelectedItem.ID.ToString() : "");
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000937D0 File Offset: 0x000919D0
		public void Load(ItemFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.ItemID))
			{
				ItemDefinition item = Registry.GetItem(data.ItemID);
				if (item != null)
				{
					this.SetItem(item, true);
				}
			}
		}

		// Token: 0x04001AB3 RID: 6835
		public bool CanSelectNone = true;

		// Token: 0x04001AB4 RID: 6836
		public List<ItemDefinition> Options = new List<ItemDefinition>();

		// Token: 0x04001AB5 RID: 6837
		public UnityEvent<ItemDefinition> onItemChanged = new UnityEvent<ItemDefinition>();
	}
}
