using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x0200059A RID: 1434
	public class AdvancedTransitRoute : TransitRoute
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x0008EEA2 File Offset: 0x0008D0A2
		// (set) Token: 0x060022A2 RID: 8866 RVA: 0x0008EEAA File Offset: 0x0008D0AA
		public ManagementItemFilter Filter { get; private set; } = new ManagementItemFilter(ManagementItemFilter.EMode.Blacklist);

		// Token: 0x060022A3 RID: 8867 RVA: 0x0008EEB3 File Offset: 0x0008D0B3
		public AdvancedTransitRoute(ITransitEntity source, ITransitEntity destination) : base(source, destination)
		{
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0008EECC File Offset: 0x0008D0CC
		public AdvancedTransitRoute(AdvancedTransitRouteData data) : base((!string.IsNullOrEmpty(data.SourceGUID)) ? GUIDManager.GetObject<ITransitEntity>(new Guid(data.SourceGUID)) : null, (!string.IsNullOrEmpty(data.DestinationGUID)) ? GUIDManager.GetObject<ITransitEntity>(new Guid(data.DestinationGUID)) : null)
		{
			this.Filter.SetMode(data.FilterMode);
			for (int i = 0; i < data.FilterItemIDs.Count; i++)
			{
				ItemDefinition item = Registry.GetItem(data.FilterItemIDs[i]);
				if (item != null)
				{
					this.Filter.AddItem(item);
				}
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0008EF7C File Offset: 0x0008D17C
		public ItemInstance GetItemReadyToMove()
		{
			if (base.Source == null || base.Destination == null)
			{
				return null;
			}
			foreach (ItemSlot itemSlot in base.Source.OutputSlots)
			{
				if (itemSlot.ItemInstance != null && this.Filter.DoesItemMeetFilter(itemSlot.ItemInstance))
				{
					int inputCapacityForItem = base.Destination.GetInputCapacityForItem(itemSlot.ItemInstance, null, true);
					if (inputCapacityForItem > 0)
					{
						return itemSlot.ItemInstance.GetCopy(Mathf.Min(inputCapacityForItem, itemSlot.ItemInstance.Quantity));
					}
				}
			}
			return null;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0008F034 File Offset: 0x0008D234
		public AdvancedTransitRouteData GetData()
		{
			List<string> list = new List<string>();
			foreach (ItemDefinition itemDefinition in this.Filter.Items)
			{
				list.Add(itemDefinition.ID);
			}
			string sourceGUID = string.Empty;
			string destinationGUID = string.Empty;
			if (base.Source != null)
			{
				sourceGUID = base.Source.GUID.ToString();
			}
			if (base.Destination != null)
			{
				destinationGUID = base.Destination.GUID.ToString();
			}
			return new AdvancedTransitRouteData(sourceGUID, destinationGUID, this.Filter.Mode, list);
		}
	}
}
