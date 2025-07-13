using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B1 RID: 2225
	public class SetRendererMaterial : MonoBehaviour
	{
		// Token: 0x06003C45 RID: 15429 RVA: 0x000FDEE4 File Offset: 0x000FC0E4
		[Button]
		public void SetMaterial()
		{
			foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				Material[] sharedMaterials = meshRenderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					sharedMaterials[j] = this.Material;
				}
				meshRenderer.sharedMaterials = sharedMaterials;
			}
		}

		// Token: 0x04002B08 RID: 11016
		public Material Material;
	}
}
