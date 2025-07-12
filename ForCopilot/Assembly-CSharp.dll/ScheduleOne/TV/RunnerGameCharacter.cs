using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002AB RID: 683
	public class RunnerGameCharacter : MonoBehaviour
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x0003F594 File Offset: 0x0003D794
		public void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "2DObstacle")
			{
				this.Game.PlayerCollided();
				if (this.onHit != null)
				{
					this.onHit.Invoke();
				}
			}
		}

		// Token: 0x04000EBF RID: 3775
		public RunnerGame Game;

		// Token: 0x04000EC0 RID: 3776
		public UnityEvent onHit;
	}
}
