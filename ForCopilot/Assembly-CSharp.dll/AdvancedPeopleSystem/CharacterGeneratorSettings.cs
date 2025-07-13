using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000217 RID: 535
	[CreateAssetMenu(fileName = "NewCharacterGenerator", menuName = "Advanced People Pack/CharacterGenerator", order = 1)]
	public class CharacterGeneratorSettings : ScriptableObject
	{
		// Token: 0x04000C75 RID: 3189
		public MinMaxIndex hair;

		// Token: 0x04000C76 RID: 3190
		public MinMaxIndex beard;

		// Token: 0x04000C77 RID: 3191
		public MinMaxIndex hat;

		// Token: 0x04000C78 RID: 3192
		public MinMaxIndex accessory;

		// Token: 0x04000C79 RID: 3193
		public MinMaxIndex shirt;

		// Token: 0x04000C7A RID: 3194
		public MinMaxIndex pants;

		// Token: 0x04000C7B RID: 3195
		public MinMaxIndex shoes;

		// Token: 0x04000C7C RID: 3196
		[Space(10f)]
		public MinMaxColor skinColors = new MinMaxColor();

		// Token: 0x04000C7D RID: 3197
		public MinMaxColor eyeColors = new MinMaxColor();

		// Token: 0x04000C7E RID: 3198
		public MinMaxColor hairColors = new MinMaxColor();

		// Token: 0x04000C7F RID: 3199
		[Space(10f)]
		public MinMaxBlendshapes headSize;

		// Token: 0x04000C80 RID: 3200
		public MinMaxBlendshapes headOffset;

		// Token: 0x04000C81 RID: 3201
		public MinMaxBlendshapes height;

		// Token: 0x04000C82 RID: 3202
		public MinMaxBlendshapes fat;

		// Token: 0x04000C83 RID: 3203
		public MinMaxBlendshapes muscles;

		// Token: 0x04000C84 RID: 3204
		public MinMaxBlendshapes thin;

		// Token: 0x04000C85 RID: 3205
		[Space(15f)]
		public List<MinMaxFacialBlendshapes> facialBlendshapes = new List<MinMaxFacialBlendshapes>();

		// Token: 0x04000C86 RID: 3206
		[Space(15f)]
		public List<GeneratorExclude> excludes = new List<GeneratorExclude>();
	}
}
