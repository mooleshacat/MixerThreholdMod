using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020A RID: 522
	[Serializable]
	public class ClothesAnchor
	{
		// Token: 0x06000BA2 RID: 2978 RVA: 0x00034F3A File Offset: 0x0003313A
		public ClothesAnchor()
		{
			this.skinnedMesh = new List<SkinnedMeshRenderer>();
		}

		// Token: 0x04000C34 RID: 3124
		public CharacterElementType partType;

		// Token: 0x04000C35 RID: 3125
		public List<SkinnedMeshRenderer> skinnedMesh;
	}
}
