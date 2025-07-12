using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008C2 RID: 2242
	[CreateAssetMenu(fileName = "SeedDefinition", menuName = "ScriptableObjects/Item Definitions/SeedDefinition", order = 1)]
	[Serializable]
	public class SeedDefinition : StorableItemDefinition
	{
		// Token: 0x04002B56 RID: 11094
		public FunctionalSeed FunctionSeedPrefab;

		// Token: 0x04002B57 RID: 11095
		public Plant PlantPrefab;
	}
}
