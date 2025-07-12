using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200099F RID: 2463
	[CreateAssetMenu(fileName = "Avatar Layer", menuName = "ScriptableObjects/Avatar Layer", order = 1)]
	[Serializable]
	public class AvatarLayer : ScriptableObject
	{
		// Token: 0x04002F8D RID: 12173
		public string Name;

		// Token: 0x04002F8E RID: 12174
		public string AssetPath;

		// Token: 0x04002F8F RID: 12175
		public Texture2D Texture;

		// Token: 0x04002F90 RID: 12176
		public Texture2D Normal;

		// Token: 0x04002F91 RID: 12177
		public Texture2D Normal_DefaultFormat;

		// Token: 0x04002F92 RID: 12178
		public int Order;

		// Token: 0x04002F93 RID: 12179
		public Material CombinedMaterial;
	}
}
