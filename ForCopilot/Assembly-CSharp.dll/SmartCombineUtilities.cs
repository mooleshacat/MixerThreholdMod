using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000047 RID: 71
public static class SmartCombineUtilities
{
	// Token: 0x06000174 RID: 372 RVA: 0x00007E98 File Offset: 0x00006098
	public static void CombineMeshesSmart(this Mesh mesh, SmartMeshData[] meshData, out Material[] materials)
	{
		IDictionary<Material, SmartCombineUtilities.SmartSubmeshData> dictionary = new Dictionary<Material, SmartCombineUtilities.SmartSubmeshData>();
		IList<CombineInstance> list = new List<CombineInstance>();
		foreach (SmartMeshData smartMeshData in meshData)
		{
			IList<Material> materials2 = smartMeshData.materials;
			for (int j = 0; j < smartMeshData.mesh.subMeshCount; j++)
			{
				SmartCombineUtilities.SmartSubmeshData smartSubmeshData;
				if (dictionary.ContainsKey(materials2[j]))
				{
					smartSubmeshData = dictionary[materials2[j]];
				}
				else
				{
					smartSubmeshData = new SmartCombineUtilities.SmartSubmeshData();
					dictionary.Add(materials2[j], smartSubmeshData);
				}
				CombineInstance item = default(CombineInstance);
				item.mesh = smartMeshData.mesh;
				item.subMeshIndex = j;
				item.transform = smartMeshData.transform;
				smartSubmeshData.combineInstances.Add(item);
			}
		}
		foreach (SmartCombineUtilities.SmartSubmeshData smartSubmeshData2 in dictionary.Values)
		{
			smartSubmeshData2.CombineSubmeshes();
			list.Add(new CombineInstance
			{
				mesh = smartSubmeshData2.mesh,
				subMeshIndex = 0
			});
		}
		mesh.Clear();
		mesh.CombineMeshes(list.ToArray<CombineInstance>(), false, false);
		mesh.Optimize();
		materials = dictionary.Keys.ToArray<Material>();
	}

	// Token: 0x02000048 RID: 72
	private class SmartSubmeshData
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00008004 File Offset: 0x00006204
		// (set) Token: 0x06000176 RID: 374 RVA: 0x0000800C File Offset: 0x0000620C
		public Mesh mesh { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00008015 File Offset: 0x00006215
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000801D File Offset: 0x0000621D
		public IList<CombineInstance> combineInstances { get; private set; }

		// Token: 0x06000179 RID: 377 RVA: 0x00008026 File Offset: 0x00006226
		public SmartSubmeshData()
		{
			this.combineInstances = new List<CombineInstance>();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008039 File Offset: 0x00006239
		public void CombineSubmeshes()
		{
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
			}
			else
			{
				this.mesh.Clear();
			}
			this.mesh.CombineMeshes(this.combineInstances.ToArray<CombineInstance>(), true, true);
		}
	}
}
