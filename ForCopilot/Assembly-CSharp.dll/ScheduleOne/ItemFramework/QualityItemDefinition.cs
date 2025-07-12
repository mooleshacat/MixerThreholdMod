using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000994 RID: 2452
	[CreateAssetMenu(fileName = "StorableItemDefinition", menuName = "ScriptableObjects/QualityItemDefinition", order = 1)]
	[Serializable]
	public class QualityItemDefinition : StorableItemDefinition
	{
		// Token: 0x06004248 RID: 16968 RVA: 0x00116BBA File Offset: 0x00114DBA
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new QualityItemInstance(this, quantity, this.DefaultQuality);
		}

		// Token: 0x04002F1A RID: 12058
		[Header("Quality")]
		public EQuality DefaultQuality = EQuality.Standard;
	}
}
