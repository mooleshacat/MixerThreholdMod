using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E9 RID: 489
	public class LightningSpawnArea : MonoBehaviour
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002FC88 File Offset: 0x0002DE88
		public void OnDrawGizmosSelected()
		{
			Vector3 localScale = base.transform.localScale;
			Gizmos.color = Color.yellow;
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, this.lightningArea);
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002FCE6 File Offset: 0x0002DEE6
		private void OnEnable()
		{
			LightningRenderer.AddSpawnArea(this);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002FCEE File Offset: 0x0002DEEE
		private void OnDisable()
		{
			LightningRenderer.RemoveSpawnArea(this);
		}

		// Token: 0x04000BA0 RID: 2976
		[Tooltip("Dimensions of the lightning area where lightning bolts will be spawned inside randomly.")]
		public Vector3 lightningArea = new Vector3(40f, 20f, 20f);
	}
}
