using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C67 RID: 3175
	public class Grave : MonoBehaviour
	{
		// Token: 0x0600595D RID: 22877 RVA: 0x001799CC File Offset: 0x00177BCC
		[Button]
		public void RandomizeGrave()
		{
			int num = UnityEngine.Random.Range(0, this.Surfaces.Length);
			int num2 = UnityEngine.Random.Range(0, this.HeadstoneObjects.Length);
			for (int i = 0; i < this.Surfaces.Length; i++)
			{
				this.Surfaces[i].Object.SetActive(i == num);
			}
			for (int j = 0; j < this.HeadstoneObjects.Length; j++)
			{
				this.HeadstoneObjects[j].SetActive(j == num2);
			}
			int num3 = UnityEngine.Random.Range(0, this.Surfaces[num].Materials.Length);
			int num4 = UnityEngine.Random.Range(0, this.HeadstoneMaterials.Length);
			this.Surfaces[num].Mesh.material = this.Surfaces[num].Materials[num3];
			for (int k = 0; k < this.HeadstoneMeshes.Length; k++)
			{
				this.HeadstoneMeshes[k].material = this.HeadstoneMaterials[num4];
			}
		}

		// Token: 0x04004182 RID: 16770
		[Header("References")]
		public Grave.GraveSuface[] Surfaces;

		// Token: 0x04004183 RID: 16771
		public GameObject[] HeadstoneObjects;

		// Token: 0x04004184 RID: 16772
		public MeshRenderer[] HeadstoneMeshes;

		// Token: 0x04004185 RID: 16773
		public Material[] HeadstoneMaterials;

		// Token: 0x02000C68 RID: 3176
		[Serializable]
		public class GraveSuface
		{
			// Token: 0x04004186 RID: 16774
			public GameObject Object;

			// Token: 0x04004187 RID: 16775
			public MeshRenderer Mesh;

			// Token: 0x04004188 RID: 16776
			public Material[] Materials;
		}
	}
}
