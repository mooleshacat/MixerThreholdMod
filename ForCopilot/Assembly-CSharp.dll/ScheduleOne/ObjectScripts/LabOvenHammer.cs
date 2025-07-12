using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C33 RID: 3123
	public class LabOvenHammer : MonoBehaviour
	{
		// Token: 0x06005669 RID: 22121 RVA: 0x0016DA19 File Offset: 0x0016BC19
		private void Start()
		{
			this.Draggable.Rb.centerOfMass = this.CoM.localPosition;
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0016DA38 File Offset: 0x0016BC38
		private void Update()
		{
			this.Rotator.enabled = this.Draggable.IsHeld;
			if (this.Draggable.IsHeld)
			{
				this.Rotator.TargetRotation.z = Mathf.Lerp(this.MinAngle, this.MaxAngle, Mathf.Clamp01(Mathf.InverseLerp(this.MinHeight, this.MaxHeight, base.transform.localPosition.y)));
			}
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x0016DAAF File Offset: 0x0016BCAF
		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(collision);
			}
		}

		// Token: 0x04003FE4 RID: 16356
		public Draggable Draggable;

		// Token: 0x04003FE5 RID: 16357
		public DraggableConstraint Constraint;

		// Token: 0x04003FE6 RID: 16358
		public RotateRigidbodyToTarget Rotator;

		// Token: 0x04003FE7 RID: 16359
		public Transform CoM;

		// Token: 0x04003FE8 RID: 16360
		public Transform ImpactPoint;

		// Token: 0x04003FE9 RID: 16361
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04003FEA RID: 16362
		[Header("Settings")]
		public float MinHeight;

		// Token: 0x04003FEB RID: 16363
		public float MaxHeight = 0.3f;

		// Token: 0x04003FEC RID: 16364
		public float MinAngle = 100f;

		// Token: 0x04003FED RID: 16365
		public float MaxAngle = 40f;

		// Token: 0x04003FEE RID: 16366
		public UnityEvent<Collision> onCollision;
	}
}
