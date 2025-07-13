using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class SmartMeshData
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600016A RID: 362 RVA: 0x00007D82 File Offset: 0x00005F82
	// (set) Token: 0x0600016B RID: 363 RVA: 0x00007D8A File Offset: 0x00005F8A
	public Mesh mesh { get; private set; }

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600016C RID: 364 RVA: 0x00007D93 File Offset: 0x00005F93
	// (set) Token: 0x0600016D RID: 365 RVA: 0x00007D9B File Offset: 0x00005F9B
	public Matrix4x4 transform { get; private set; }

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600016E RID: 366 RVA: 0x00007DA4 File Offset: 0x00005FA4
	public IList<Material> materials
	{
		get
		{
			return new ReadOnlyCollection<Material>(this._materials);
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00007DB4 File Offset: 0x00005FB4
	public SmartMeshData(Mesh inMesh, Material[] inMaterials, Matrix4x4 inTransform)
	{
		this.mesh = inMesh;
		this._materials = inMaterials;
		this.transform = inTransform;
		if (this._materials.Length != this.mesh.subMeshCount)
		{
			Debug.LogWarning("SmartMeshData has incorrect number of materials. Resizing to match submesh count");
			Material[] array = new Material[this.mesh.subMeshCount];
			for (int i = 0; i < this._materials.Length; i++)
			{
				if (i < this._materials.Length)
				{
					array[i] = this._materials[i];
				}
				else
				{
					array[i] = null;
				}
			}
			this._materials = array;
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00007E42 File Offset: 0x00006042
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials) : this(inputMesh, inputMaterials, Matrix4x4.identity)
	{
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00007E51 File Offset: 0x00006051
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, Quaternion.identity, Vector3.one))
	{
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00007E6B File Offset: 0x0000606B
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position, Quaternion rotation) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, rotation, Vector3.one))
	{
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00007E82 File Offset: 0x00006082
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position, Quaternion rotation, Vector3 scale) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, rotation, scale))
	{
	}

	// Token: 0x04000136 RID: 310
	private Material[] _materials;
}
