using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
[ExecuteInEditMode]
public class RenderPipelineLighting : MonoBehaviour
{
	// Token: 0x06000265 RID: 613 RVA: 0x0000DBB4 File Offset: 0x0000BDB4
	private void OnValidate()
	{
		this.Awake();
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000045B1 File Offset: 0x000027B1
	private void Awake()
	{
	}

	// Token: 0x04000285 RID: 645
	[SerializeField]
	private GameObject _standardLighting;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private Material _standardSky;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	private Material _standardTerrain;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private GameObject _universalLighting;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	private Material _universalSky;

	// Token: 0x0400028A RID: 650
	[SerializeField]
	private Material _universalTerrain;

	// Token: 0x0400028B RID: 651
	[SerializeField]
	private GameObject _highDefinitionLighting;

	// Token: 0x0400028C RID: 652
	[SerializeField]
	private Material _highDefinitionSky;

	// Token: 0x0400028D RID: 653
	[SerializeField]
	private Material _highDefinitionTerrain;
}
