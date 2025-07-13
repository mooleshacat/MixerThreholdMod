using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A6 RID: 2214
	public class ParticleCollisionDetector : MonoBehaviour
	{
		// Token: 0x06003C1A RID: 15386 RVA: 0x000FD6E9 File Offset: 0x000FB8E9
		private void Awake()
		{
			this.ps = base.GetComponent<ParticleSystem>();
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x000FD6F7 File Offset: 0x000FB8F7
		public void OnParticleCollision(GameObject other)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(other);
			}
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x000FD710 File Offset: 0x000FB910
		private void OnParticleTrigger()
		{
			Component collider = this.ps.trigger.GetCollider(0);
			if (collider != null && this.onCollision != null)
			{
				this.onCollision.Invoke(collider.gameObject);
			}
		}

		// Token: 0x04002AEB RID: 10987
		public UnityEvent<GameObject> onCollision = new UnityEvent<GameObject>();

		// Token: 0x04002AEC RID: 10988
		private ParticleSystem ps;
	}
}
