using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000221 RID: 545
	public class CharacterPreBuilt : ScriptableObject
	{
		// Token: 0x04000CA2 RID: 3234
		[SerializeField]
		public CharacterSettings settings;

		// Token: 0x04000CA3 RID: 3235
		[SerializeField]
		public List<PreBuiltData> preBuiltDatas = new List<PreBuiltData>();

		// Token: 0x04000CA4 RID: 3236
		[SerializeField]
		public List<PreBuiltBlendshape> blendshapes = new List<PreBuiltBlendshape>();
	}
}
