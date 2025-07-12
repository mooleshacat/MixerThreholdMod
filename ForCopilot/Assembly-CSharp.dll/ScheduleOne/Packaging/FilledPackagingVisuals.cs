using System;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008D3 RID: 2259
	public class FilledPackagingVisuals : MonoBehaviour
	{
		// Token: 0x06003CDC RID: 15580 RVA: 0x001005E4 File Offset: 0x000FE7E4
		public void ResetVisuals()
		{
			if (this.weedVisuals.Container != null)
			{
				this.weedVisuals.Container.gameObject.SetActive(false);
			}
			if (this.methVisuals.Container != null)
			{
				this.methVisuals.Container.gameObject.SetActive(false);
			}
			if (this.cocaineVisuals.Container != null)
			{
				this.cocaineVisuals.Container.gameObject.SetActive(false);
			}
		}

		// Token: 0x04002BB8 RID: 11192
		public FilledPackagingVisuals.WeedVisuals weedVisuals;

		// Token: 0x04002BB9 RID: 11193
		public FilledPackagingVisuals.MethVisuals methVisuals;

		// Token: 0x04002BBA RID: 11194
		public FilledPackagingVisuals.CocaineVisuals cocaineVisuals;

		// Token: 0x020008D4 RID: 2260
		[Serializable]
		public class MeshIndexPair
		{
			// Token: 0x04002BBB RID: 11195
			public MeshRenderer Mesh;

			// Token: 0x04002BBC RID: 11196
			public int MaterialIndex;
		}

		// Token: 0x020008D5 RID: 2261
		[Serializable]
		public class BaseVisuals
		{
			// Token: 0x04002BBD RID: 11197
			public Transform Container;
		}

		// Token: 0x020008D6 RID: 2262
		[Serializable]
		public class WeedVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002BBE RID: 11198
			public FilledPackagingVisuals.MeshIndexPair[] MainMeshes;

			// Token: 0x04002BBF RID: 11199
			public FilledPackagingVisuals.MeshIndexPair[] SecondaryMeshes;

			// Token: 0x04002BC0 RID: 11200
			public FilledPackagingVisuals.MeshIndexPair[] LeafMeshes;

			// Token: 0x04002BC1 RID: 11201
			public FilledPackagingVisuals.MeshIndexPair[] StemMeshes;
		}

		// Token: 0x020008D7 RID: 2263
		[Serializable]
		public class MethVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002BC2 RID: 11202
			public MeshRenderer[] CrystalMeshes;
		}

		// Token: 0x020008D8 RID: 2264
		[Serializable]
		public class CocaineVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002BC3 RID: 11203
			public MeshRenderer[] RockMeshes;
		}
	}
}
