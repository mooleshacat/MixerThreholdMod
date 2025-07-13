using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C4D RID: 3149
	[CreateAssetMenu(fileName = "TrashGrabberDefinition", menuName = "ScriptableObjects/Item Definitions/TrashGrabberDefinition", order = 1)]
	[Serializable]
	public class TrashGrabberDefinition : StorableItemDefinition
	{
		// Token: 0x060058D7 RID: 22743 RVA: 0x00177B56 File Offset: 0x00175D56
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new TrashGrabberInstance(this, quantity);
		}
	}
}
