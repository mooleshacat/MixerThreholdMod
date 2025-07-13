using System;
using ScheduleOne.Growing;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000972 RID: 2418
	[CreateAssetMenu(fileName = "AdditiveDefinition", menuName = "ScriptableObjects/Item Definitions/AdditiveDefinition", order = 1)]
	[Serializable]
	public class AdditiveDefinition : StorableItemDefinition
	{
		// Token: 0x04002EAA RID: 11946
		public Additive AdditivePrefab;
	}
}
