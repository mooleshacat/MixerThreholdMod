using System;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C8 RID: 1736
	public class PivotDoor : MonoBehaviour
	{
		// Token: 0x06002FCF RID: 12239 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000C8FFD File Offset: 0x000C71FD
		private void LateUpdate()
		{
			this.DoorTransform.localRotation = Quaternion.Lerp(this.DoorTransform.localRotation, Quaternion.Euler(0f, this.targetDoorAngle, 0f), Time.deltaTime * this.SwingSpeed);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000C903C File Offset: 0x000C723C
		public virtual void Opened(EDoorSide openSide)
		{
			if (openSide == EDoorSide.Interior)
			{
				this.targetDoorAngle = (this.FlipSide ? this.OpenInwardsAngle : this.OpenOutwardsAngle);
				return;
			}
			if (openSide != EDoorSide.Exterior)
			{
				return;
			}
			this.targetDoorAngle = (this.FlipSide ? this.OpenOutwardsAngle : this.OpenInwardsAngle);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000C908A File Offset: 0x000C728A
		public virtual void Closed()
		{
			this.targetDoorAngle = 0f;
		}

		// Token: 0x0400219F RID: 8607
		[Header("Settings")]
		public Transform DoorTransform;

		// Token: 0x040021A0 RID: 8608
		public bool FlipSide;

		// Token: 0x040021A1 RID: 8609
		public float OpenInwardsAngle = -100f;

		// Token: 0x040021A2 RID: 8610
		public float OpenOutwardsAngle = 100f;

		// Token: 0x040021A3 RID: 8611
		public float SwingSpeed = 5f;

		// Token: 0x040021A4 RID: 8612
		private float targetDoorAngle;
	}
}
