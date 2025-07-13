using System;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C60 RID: 3168
	public class CarStopper : MonoBehaviour
	{
		// Token: 0x0600592E RID: 22830 RVA: 0x00178B58 File Offset: 0x00176D58
		protected virtual void Update()
		{
			float num = 70f;
			if (this.isActive)
			{
				this.Obstacle.enabled = true;
				this.blocker.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(this.blocker.localEulerAngles.z + Time.deltaTime * num / this.moveTime, 0f, num));
				return;
			}
			this.Obstacle.enabled = false;
			this.blocker.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(this.blocker.localEulerAngles.z - Time.deltaTime * num / this.moveTime, 0f, num));
		}

		// Token: 0x04004154 RID: 16724
		public bool isActive;

		// Token: 0x04004155 RID: 16725
		[Header("References")]
		[SerializeField]
		protected Transform blocker;

		// Token: 0x04004156 RID: 16726
		public NavMeshObstacle Obstacle;

		// Token: 0x04004157 RID: 16727
		private float moveTime = 0.5f;
	}
}
