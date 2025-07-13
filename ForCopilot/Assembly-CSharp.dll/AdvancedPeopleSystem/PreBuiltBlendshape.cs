using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200021F RID: 543
	[Serializable]
	public class PreBuiltBlendshape
	{
		// Token: 0x06000BC7 RID: 3015 RVA: 0x000365C6 File Offset: 0x000347C6
		public PreBuiltBlendshape(string name, float weight)
		{
			this.name = name;
			this.weight = weight;
		}

		// Token: 0x04000C9D RID: 3229
		[SerializeField]
		public string name;

		// Token: 0x04000C9E RID: 3230
		[SerializeField]
		public float weight;
	}
}
