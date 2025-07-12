using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000209 RID: 521
	[Serializable]
	public class CharacterPart
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x00034F27 File Offset: 0x00033127
		public CharacterPart()
		{
			this.skinnedMesh = new List<SkinnedMeshRenderer>();
		}

		// Token: 0x04000C32 RID: 3122
		public string name;

		// Token: 0x04000C33 RID: 3123
		public List<SkinnedMeshRenderer> skinnedMesh;
	}
}
