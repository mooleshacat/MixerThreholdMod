using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x06000183 RID: 387 RVA: 0x000083EB File Offset: 0x000065EB
	public void Start()
	{
		this.ChangeMaterial();
	}

	// Token: 0x06000184 RID: 388 RVA: 0x000083F3 File Offset: 0x000065F3
	public void ChangeMaterial()
	{
		this.targetRenderer.sharedMaterial = this.materials[UnityEngine.Random.Range(0, this.materials.Length)];
	}

	// Token: 0x0400014D RID: 333
	public Renderer targetRenderer;

	// Token: 0x0400014E RID: 334
	public Material[] materials;
}
