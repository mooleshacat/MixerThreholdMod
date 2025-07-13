using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000984 RID: 2436
	[CreateAssetMenu(fileName = "IntegerItemDefinition", menuName = "ScriptableObjects/IntegerItemDefinition", order = 1)]
	[Serializable]
	public class IntegerItemDefinition : StorableItemDefinition
	{
		// Token: 0x060041AC RID: 16812 RVA: 0x00114EEF File Offset: 0x001130EF
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new IntegerItemInstance(this, quantity, this.DefaultValue);
		}

		// Token: 0x04002ECA RID: 11978
		public int DefaultValue;
	}
}
