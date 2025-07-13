using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000226 RID: 550
	[Serializable]
	public class CharacterBlendshapeData
	{
		// Token: 0x06000BCE RID: 3022 RVA: 0x0003673E File Offset: 0x0003493E
		public CharacterBlendshapeData(string name, CharacterBlendShapeType t, CharacterBlendShapeGroup g, float value = 0f)
		{
			this.blendshapeName = name;
			this.type = t;
			this.group = g;
			this.value = value;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0000494F File Offset: 0x00002B4F
		public CharacterBlendshapeData()
		{
		}

		// Token: 0x04000CC5 RID: 3269
		public string blendshapeName;

		// Token: 0x04000CC6 RID: 3270
		public CharacterBlendShapeType type;

		// Token: 0x04000CC7 RID: 3271
		public CharacterBlendShapeGroup group;

		// Token: 0x04000CC8 RID: 3272
		[HideInInspector]
		public float value;
	}
}
