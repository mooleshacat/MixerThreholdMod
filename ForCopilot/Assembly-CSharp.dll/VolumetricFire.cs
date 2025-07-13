using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200006A RID: 106
[ExecuteAlways]
public class VolumetricFire : MonoBehaviour
{
	// Token: 0x06000259 RID: 601 RVA: 0x0000D85C File Offset: 0x0000BA5C
	private void Start()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		component.enabled = false;
		this.material = component.sharedMaterial;
		this.mesh = base.GetComponent<MeshFilter>().sharedMesh;
		this.boundaryCollider = base.GetComponent<Collider>();
		this.randomStatic = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderFlames;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000D8D3 File Offset: 0x0000BAD3
	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= this.RenderFlames;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000D8E6 File Offset: 0x0000BAE6
	private static bool IsVisible(Camera camera, Bounds bounds)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bounds);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
	private void RenderFlames(ScriptableRenderContext context, Camera camera)
	{
		VolumetricFire.IsVisible(camera, this.boundaryCollider.bounds);
		this.internalCount = (this.thickness - 1) * 2;
		float spacing = 0f;
		if (this.internalCount > 0)
		{
			spacing = this.spread / (float)this.internalCount;
		}
		for (int i = 0; i <= this.internalCount; i++)
		{
			float item = (float)i - (float)this.internalCount * 0.5f;
			this.SetupMaterialPropertyBlock(item);
			this.CreateItem(spacing, item, camera);
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000D974 File Offset: 0x0000BB74
	private void SetupMaterialPropertyBlock(float item)
	{
		if (this.materialPropertyBlock == null)
		{
			return;
		}
		this.materialPropertyBlock.SetFloat("_ITEMNUMBER", item);
		this.materialPropertyBlock.SetFloat("_INTERNALCOUNT", (float)this.internalCount);
		this.materialPropertyBlock.SetFloat("_INITIALPOSITIONINT", this.randomStatic);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
	private void CreateItem(float spacing, float item, Camera camera)
	{
		Quaternion quaternion = Quaternion.identity;
		Vector3 pos = Vector3.zero;
		if (this.billboard)
		{
			quaternion *= camera.transform.rotation;
			Vector3 normalized = (base.transform.position - camera.transform.position).normalized;
			pos = base.transform.position - normalized * item * spacing;
		}
		else
		{
			quaternion = base.transform.rotation;
			pos = base.transform.position - base.transform.forward * item * spacing;
		}
		Matrix4x4 matrix = Matrix4x4.TRS(pos, quaternion, base.transform.localScale);
		Graphics.DrawMesh(this.mesh, matrix, this.material, 0, camera, 0, this.materialPropertyBlock, false, false, false);
	}

	// Token: 0x04000278 RID: 632
	private Mesh mesh;

	// Token: 0x04000279 RID: 633
	private Material material;

	// Token: 0x0400027A RID: 634
	[SerializeField]
	[Range(1f, 20f)]
	[Tooltip("Controls the number of additional meshes to render in front of and behind the original mesh")]
	private int thickness = 1;

	// Token: 0x0400027B RID: 635
	[SerializeField]
	[Range(0.01f, 1f)]
	[Tooltip("Controls the total distance between the frontmost mesh and the backmost mesh")]
	private float spread = 0.2f;

	// Token: 0x0400027C RID: 636
	[SerializeField]
	private bool billboard = true;

	// Token: 0x0400027D RID: 637
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x0400027E RID: 638
	private int internalCount;

	// Token: 0x0400027F RID: 639
	private float randomStatic;

	// Token: 0x04000280 RID: 640
	private Collider boundaryCollider;
}
