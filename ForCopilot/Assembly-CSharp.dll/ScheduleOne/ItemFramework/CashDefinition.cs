using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200096E RID: 2414
	[CreateAssetMenu(fileName = "CashDefinition", menuName = "ScriptableObjects/CashDefinition", order = 1)]
	[Serializable]
	public class CashDefinition : StorableItemDefinition
	{
		// Token: 0x06004171 RID: 16753 RVA: 0x0011493C File Offset: 0x00112B3C
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new CashInstance(this, quantity);
		}
	}
}
