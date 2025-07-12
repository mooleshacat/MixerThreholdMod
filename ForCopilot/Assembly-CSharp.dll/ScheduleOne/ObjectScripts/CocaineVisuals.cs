using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C08 RID: 3080
	public class CocaineVisuals : MonoBehaviour
	{
		// Token: 0x06005315 RID: 21269 RVA: 0x0015EFEC File Offset: 0x0015D1EC
		public void Setup(CocaineDefinition definition)
		{
			MeshRenderer[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].material = definition.RockMaterial;
			}
		}

		// Token: 0x04003E25 RID: 15909
		public MeshRenderer[] Meshes;
	}
}
