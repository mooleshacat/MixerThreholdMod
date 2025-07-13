using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000183 RID: 387
	public class LiquidLevelPouringSync : MonoBehaviour
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x00021818 File Offset: 0x0001FA18
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.lv = base.transform.parent.GetComponent<LiquidVolume>();
			this.UpdateColliderPos();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00021842 File Offset: 0x0001FA42
		private void OnParticleCollision(GameObject other)
		{
			if (this.lv.level < 1f)
			{
				this.lv.level += this.fillSpeed;
			}
			this.UpdateColliderPos();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00021874 File Offset: 0x0001FA74
		private void UpdateColliderPos()
		{
			Vector3 position = new Vector3(base.transform.position.x, this.lv.liquidSurfaceYPosition - base.transform.localScale.y * 0.5f - this.sinkFactor, base.transform.position.z);
			this.rb.position = position;
			if (this.lv.level >= 1f)
			{
				base.transform.localRotation = Quaternion.Euler(UnityEngine.Random.value * 30f - 15f, UnityEngine.Random.value * 30f - 15f, UnityEngine.Random.value * 30f - 15f);
				return;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}

		// Token: 0x04000836 RID: 2102
		public float fillSpeed = 0.01f;

		// Token: 0x04000837 RID: 2103
		public float sinkFactor = 0.1f;

		// Token: 0x04000838 RID: 2104
		private LiquidVolume lv;

		// Token: 0x04000839 RID: 2105
		private Rigidbody rb;
	}
}
