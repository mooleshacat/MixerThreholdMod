using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C50 RID: 3152
	[CreateAssetMenu(fileName = "WateringCanDefinition", menuName = "ScriptableObjects/Item Definitions/WateringCanDefinition", order = 1)]
	[Serializable]
	public class WateringCanDefinition : StorableItemDefinition
	{
		// Token: 0x060058E7 RID: 22759 RVA: 0x00177DA6 File Offset: 0x00175FA6
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new WateringCanInstance(this, quantity, 0f);
		}

		// Token: 0x0400410D RID: 16653
		public const float Capacity = 15f;

		// Token: 0x0400410E RID: 16654
		public GameObject FunctionalWateringCanPrefab;
	}
}
