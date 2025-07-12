using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x02000235 RID: 565
	[CreateAssetMenu(menuName = "Ardenfall/Foliage/Billboard Asset")]
	public class BillboardAsset : ScriptableObject
	{
		// Token: 0x04000D48 RID: 3400
		public GameObject prefab;

		// Token: 0x04000D49 RID: 3401
		public BillboardRenderSettings renderSettings;

		// Token: 0x04000D4A RID: 3402
		[Header("Values")]
		public int textureSize = 512;

		// Token: 0x04000D4B RID: 3403
		public float cutoff = 0.15f;

		// Token: 0x04000D4C RID: 3404
		[Header("LODs")]
		public bool pickLastLOD = true;

		// Token: 0x04000D4D RID: 3405
		public int LODIndex;

		// Token: 0x04000D4E RID: 3406
		[HideInInspector]
		public List<Texture2D> generatedTextures;

		// Token: 0x04000D4F RID: 3407
		[HideInInspector]
		public Mesh generatedMesh;

		// Token: 0x04000D50 RID: 3408
		[HideInInspector]
		public Material generatedMaterial;

		// Token: 0x04000D51 RID: 3409
		[HideInInspector]
		public GameObject generatedPrefab;
	}
}
