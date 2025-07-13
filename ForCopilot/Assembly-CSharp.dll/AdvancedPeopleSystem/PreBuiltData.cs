using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000220 RID: 544
	[Serializable]
	public class PreBuiltData
	{
		// Token: 0x04000C9F RID: 3231
		[SerializeField]
		public string GroupName;

		// Token: 0x04000CA0 RID: 3232
		[SerializeField]
		public List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000CA1 RID: 3233
		[SerializeField]
		public List<Material> materials = new List<Material>();
	}
}
