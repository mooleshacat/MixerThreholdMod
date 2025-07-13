using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019D RID: 413
	public class SpriteArtItem : ScriptableObject
	{
		// Token: 0x04000950 RID: 2384
		public Mesh mesh;

		// Token: 0x04000951 RID: 2385
		public Material material;

		// Token: 0x04000952 RID: 2386
		public int rows;

		// Token: 0x04000953 RID: 2387
		public int columns;

		// Token: 0x04000954 RID: 2388
		public int totalFrames;

		// Token: 0x04000955 RID: 2389
		public int animateSpeed;

		// Token: 0x04000956 RID: 2390
		[Tooltip("Color that will be multiplied against the base lightning bolt text color")]
		public Color tintColor = Color.white;
	}
}
