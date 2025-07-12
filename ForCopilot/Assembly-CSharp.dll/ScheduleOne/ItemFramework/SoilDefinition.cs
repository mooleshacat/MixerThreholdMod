using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000970 RID: 2416
	[CreateAssetMenu(fileName = "SoilDefinition", menuName = "ScriptableObjects/Item Definitions/SoilDefinition", order = 1)]
	[Serializable]
	public class SoilDefinition : StorableItemDefinition
	{
		// Token: 0x04002EA2 RID: 11938
		public SoilDefinition.ESoilQuality SoilQuality;

		// Token: 0x04002EA3 RID: 11939
		public Material DrySoilMat;

		// Token: 0x04002EA4 RID: 11940
		public Material WetSoilMat;

		// Token: 0x04002EA5 RID: 11941
		public Color ParticleColor;

		// Token: 0x04002EA6 RID: 11942
		public int Uses = 1;

		// Token: 0x02000971 RID: 2417
		public enum ESoilQuality
		{
			// Token: 0x04002EA8 RID: 11944
			Basic,
			// Token: 0x04002EA9 RID: 11945
			Premium
		}
	}
}
