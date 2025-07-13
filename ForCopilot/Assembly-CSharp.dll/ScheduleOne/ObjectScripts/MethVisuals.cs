using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C09 RID: 3081
	public class MethVisuals : MonoBehaviour
	{
		// Token: 0x06005317 RID: 21271 RVA: 0x0015F01C File Offset: 0x0015D21C
		public void Setup(MethDefinition definition)
		{
			MeshRenderer[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].material = definition.CrystalMaterial;
			}
		}

		// Token: 0x04003E26 RID: 15910
		public MeshRenderer[] Meshes;
	}
}
