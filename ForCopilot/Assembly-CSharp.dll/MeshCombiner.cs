using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200003D RID: 61
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600013B RID: 315 RVA: 0x0000715D File Offset: 0x0000535D
	// (set) Token: 0x0600013C RID: 316 RVA: 0x00007165 File Offset: 0x00005365
	public bool CreateMultiMaterialMesh
	{
		get
		{
			return this.createMultiMaterialMesh;
		}
		set
		{
			this.createMultiMaterialMesh = value;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600013D RID: 317 RVA: 0x0000716E File Offset: 0x0000536E
	// (set) Token: 0x0600013E RID: 318 RVA: 0x00007176 File Offset: 0x00005376
	public bool CombineInactiveChildren
	{
		get
		{
			return this.combineInactiveChildren;
		}
		set
		{
			this.combineInactiveChildren = value;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600013F RID: 319 RVA: 0x0000717F File Offset: 0x0000537F
	// (set) Token: 0x06000140 RID: 320 RVA: 0x00007187 File Offset: 0x00005387
	public bool DeactivateCombinedChildren
	{
		get
		{
			return this.deactivateCombinedChildren;
		}
		set
		{
			this.deactivateCombinedChildren = value;
			this.CheckDeactivateCombinedChildren();
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000141 RID: 321 RVA: 0x00007196 File Offset: 0x00005396
	// (set) Token: 0x06000142 RID: 322 RVA: 0x0000719E File Offset: 0x0000539E
	public bool DeactivateCombinedChildrenMeshRenderers
	{
		get
		{
			return this.deactivateCombinedChildrenMeshRenderers;
		}
		set
		{
			this.deactivateCombinedChildrenMeshRenderers = value;
			this.CheckDeactivateCombinedChildren();
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000143 RID: 323 RVA: 0x000071AD File Offset: 0x000053AD
	// (set) Token: 0x06000144 RID: 324 RVA: 0x000071B5 File Offset: 0x000053B5
	public bool GenerateUVMap
	{
		get
		{
			return this.generateUVMap;
		}
		set
		{
			this.generateUVMap = value;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000145 RID: 325 RVA: 0x000071BE File Offset: 0x000053BE
	// (set) Token: 0x06000146 RID: 326 RVA: 0x000071C6 File Offset: 0x000053C6
	public bool DestroyCombinedChildren
	{
		get
		{
			return this.destroyCombinedChildren;
		}
		set
		{
			this.destroyCombinedChildren = value;
			this.CheckDestroyCombinedChildren();
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000147 RID: 327 RVA: 0x000071D5 File Offset: 0x000053D5
	// (set) Token: 0x06000148 RID: 328 RVA: 0x000071DD File Offset: 0x000053DD
	public string FolderPath
	{
		get
		{
			return this.folderPath;
		}
		set
		{
			this.folderPath = value;
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x000071E6 File Offset: 0x000053E6
	private void CheckDeactivateCombinedChildren()
	{
		if (this.deactivateCombinedChildren || this.deactivateCombinedChildrenMeshRenderers)
		{
			this.destroyCombinedChildren = false;
		}
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000071FF File Offset: 0x000053FF
	private void CheckDestroyCombinedChildren()
	{
		if (this.destroyCombinedChildren)
		{
			this.deactivateCombinedChildren = false;
			this.deactivateCombinedChildrenMeshRenderers = false;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00007218 File Offset: 0x00005418
	public void CombineMeshes(bool showCreatedMeshInfo)
	{
		Vector3 localScale = base.transform.localScale;
		int siblingIndex = base.transform.GetSiblingIndex();
		Transform parent = base.transform.parent;
		base.transform.parent = null;
		Quaternion rotation = base.transform.rotation;
		Vector3 position = base.transform.position;
		Vector3 localScale2 = base.transform.localScale;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		base.transform.localScale = Vector3.one;
		if (!this.createMultiMaterialMesh)
		{
			this.CombineMeshesWithSingleMaterial(showCreatedMeshInfo);
		}
		else
		{
			this.CombineMeshesWithMutliMaterial(showCreatedMeshInfo);
		}
		base.transform.rotation = rotation;
		base.transform.position = position;
		base.transform.localScale = localScale2;
		base.transform.parent = parent;
		base.transform.SetSiblingIndex(siblingIndex);
		base.transform.localScale = localScale;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00007310 File Offset: 0x00005510
	private MeshFilter[] GetMeshFiltersToCombine()
	{
		MeshCombiner.<>c__DisplayClass33_0 CS$<>8__locals1 = new MeshCombiner.<>c__DisplayClass33_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.meshFilters = base.GetComponentsInChildren<MeshFilter>(this.combineInactiveChildren);
		this.meshFiltersToSkip = (from meshFilter in this.meshFiltersToSkip
		where meshFilter != CS$<>8__locals1.meshFilters[0]
		select meshFilter).ToArray<MeshFilter>();
		this.meshFiltersToSkip = (from meshFilter in this.meshFiltersToSkip
		where meshFilter != null
		select meshFilter).ToArray<MeshFilter>();
		int i;
		int j;
		for (i = 0; i < this.meshFiltersToSkip.Length; i = j + 1)
		{
			CS$<>8__locals1.meshFilters = (from meshFilter in CS$<>8__locals1.meshFilters
			where meshFilter != CS$<>8__locals1.<>4__this.meshFiltersToSkip[i]
			select meshFilter).ToArray<MeshFilter>();
			j = i;
		}
		return CS$<>8__locals1.meshFilters;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000073FC File Offset: 0x000055FC
	private void CombineMeshesWithSingleMaterial(bool showCreatedMeshInfo)
	{
		MeshFilter[] meshFiltersToCombine = this.GetMeshFiltersToCombine();
		CombineInstance[] array = new CombineInstance[meshFiltersToCombine.Length - 1];
		long num = 0L;
		for (int i = 0; i < meshFiltersToCombine.Length - 1; i++)
		{
			array[i].subMeshIndex = 0;
			array[i].mesh = meshFiltersToCombine[i + 1].sharedMesh;
			array[i].transform = meshFiltersToCombine[i + 1].transform.localToWorldMatrix;
			num += (long)array[i].mesh.vertices.Length;
		}
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(this.combineInactiveChildren);
		if (componentsInChildren.Length >= 2)
		{
			componentsInChildren[0].sharedMaterials = new Material[1];
			componentsInChildren[0].sharedMaterial = componentsInChildren[1].sharedMaterial;
		}
		else
		{
			componentsInChildren[0].sharedMaterials = new Material[0];
		}
		Mesh mesh = new Mesh();
		mesh.name = base.name;
		if (num > 65535L)
		{
			mesh.indexFormat = IndexFormat.UInt32;
		}
		mesh.CombineMeshes(array);
		this.GenerateUV(mesh);
		meshFiltersToCombine[0].sharedMesh = mesh;
		this.DeactivateCombinedGameObjects(meshFiltersToCombine);
		if (showCreatedMeshInfo)
		{
			if (num <= 65535L)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=#00cc00><b>Mesh \"",
					base.name,
					"\" was created from ",
					array.Length.ToString(),
					" children meshes and has ",
					num.ToString(),
					" vertices.</b></color>"
				}));
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"<color=#ff3300><b>Mesh \"",
				base.name,
				"\" was created from ",
				array.Length.ToString(),
				" children meshes and has ",
				num.ToString(),
				" vertices. Some old devices, like Android with Mali-400 GPU, do not support over 65535 vertices.</b></color>"
			}));
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000075C8 File Offset: 0x000057C8
	private void CombineMeshesWithMutliMaterial(bool showCreatedMeshInfo)
	{
		MeshFilter[] meshFiltersToCombine = this.GetMeshFiltersToCombine();
		MeshRenderer[] array = new MeshRenderer[meshFiltersToCombine.Length];
		array[0] = base.GetComponent<MeshRenderer>();
		List<Material> list = new List<Material>();
		for (int i = 0; i < meshFiltersToCombine.Length - 1; i++)
		{
			array[i + 1] = meshFiltersToCombine[i + 1].GetComponent<MeshRenderer>();
			if (array[i + 1] != null)
			{
				Material[] sharedMaterials = array[i + 1].sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (!list.Contains(sharedMaterials[j]))
					{
						list.Add(sharedMaterials[j]);
					}
				}
			}
		}
		List<CombineInstance> list2 = new List<CombineInstance>();
		long num = 0L;
		for (int k = 0; k < list.Count; k++)
		{
			List<CombineInstance> list3 = new List<CombineInstance>();
			for (int l = 0; l < meshFiltersToCombine.Length - 1; l++)
			{
				if (array[l + 1] != null)
				{
					Material[] sharedMaterials2 = array[l + 1].sharedMaterials;
					for (int m = 0; m < sharedMaterials2.Length; m++)
					{
						if (list[k] == sharedMaterials2[m])
						{
							CombineInstance item = new CombineInstance
							{
								subMeshIndex = m,
								mesh = meshFiltersToCombine[l + 1].sharedMesh,
								transform = meshFiltersToCombine[l + 1].transform.localToWorldMatrix
							};
							list3.Add(item);
							num += (long)item.mesh.vertices.Length;
						}
					}
				}
			}
			Mesh mesh = new Mesh();
			if (num > 65535L)
			{
				mesh.indexFormat = IndexFormat.UInt32;
			}
			mesh.CombineMeshes(list3.ToArray(), true);
			list2.Add(new CombineInstance
			{
				subMeshIndex = 0,
				mesh = mesh,
				transform = Matrix4x4.identity
			});
		}
		array[0].sharedMaterials = list.ToArray();
		Mesh mesh2 = new Mesh();
		mesh2.name = base.name;
		if (num > 65535L)
		{
			mesh2.indexFormat = IndexFormat.UInt32;
		}
		mesh2.CombineMeshes(list2.ToArray(), false);
		this.GenerateUV(mesh2);
		meshFiltersToCombine[0].sharedMesh = mesh2;
		this.DeactivateCombinedGameObjects(meshFiltersToCombine);
		if (showCreatedMeshInfo)
		{
			if (num <= 65535L)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=#00cc00><b>Mesh \"",
					base.name,
					"\" was created from ",
					(meshFiltersToCombine.Length - 1).ToString(),
					" children meshes and has ",
					list2.Count.ToString(),
					" submeshes, and ",
					num.ToString(),
					" vertices.</b></color>"
				}));
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"<color=#ff3300><b>Mesh \"",
				base.name,
				"\" was created from ",
				(meshFiltersToCombine.Length - 1).ToString(),
				" children meshes and has ",
				list2.Count.ToString(),
				" submeshes, and ",
				num.ToString(),
				" vertices. Some old devices, like Android with Mali-400 GPU, do not support over 65535 vertices.</b></color>"
			}));
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x000078E0 File Offset: 0x00005AE0
	private void DeactivateCombinedGameObjects(MeshFilter[] meshFilters)
	{
		for (int i = 0; i < meshFilters.Length - 1; i++)
		{
			if (!this.destroyCombinedChildren)
			{
				if (this.deactivateCombinedChildren)
				{
					meshFilters[i + 1].gameObject.SetActive(false);
				}
				if (this.deactivateCombinedChildrenMeshRenderers)
				{
					MeshRenderer component = meshFilters[i + 1].gameObject.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(meshFilters[i + 1].gameObject);
			}
		}
	}

	// Token: 0x06000150 RID: 336 RVA: 0x000045B1 File Offset: 0x000027B1
	private void GenerateUV(Mesh combinedMesh)
	{
	}

	// Token: 0x04000112 RID: 274
	private const int Mesh16BitBufferVertexLimit = 65535;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private bool createMultiMaterialMesh = true;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	private bool combineInactiveChildren;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	private bool deactivateCombinedChildren;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	private bool deactivateCombinedChildrenMeshRenderers;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	private bool generateUVMap;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	private bool destroyCombinedChildren = true;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private string folderPath = "Prefabs/CombinedMeshes";

	// Token: 0x0400011A RID: 282
	[SerializeField]
	[Tooltip("MeshFilters with Meshes which we don't want to combine into one Mesh.")]
	private MeshFilter[] meshFiltersToSkip = new MeshFilter[0];
}
