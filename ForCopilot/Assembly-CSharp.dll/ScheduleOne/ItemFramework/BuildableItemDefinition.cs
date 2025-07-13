using System;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000973 RID: 2419
	[CreateAssetMenu(fileName = "BuildableItemDefinition", menuName = "ScriptableObjects/BuildableItemDefinition", order = 1)]
	[Serializable]
	public class BuildableItemDefinition : StorableItemDefinition
	{
		// Token: 0x04002EAB RID: 11947
		public BuildableItem BuiltItem;

		// Token: 0x04002EAC RID: 11948
		public BuildableItemDefinition.EBuildSoundType BuildSoundType;

		// Token: 0x02000974 RID: 2420
		public enum EBuildSoundType
		{
			// Token: 0x04002EAE RID: 11950
			Cardboard,
			// Token: 0x04002EAF RID: 11951
			Wood,
			// Token: 0x04002EB0 RID: 11952
			Metal
		}
	}
}
