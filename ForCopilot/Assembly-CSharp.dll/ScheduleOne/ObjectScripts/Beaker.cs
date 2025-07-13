using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C1C RID: 3100
	public class Beaker : StationItem
	{
		// Token: 0x0600544F RID: 21583 RVA: 0x00164600 File Offset: 0x00162800
		private void Start()
		{
			this.Joint.connectedBody = this.Anchor;
			this.Draggable.Rb.centerOfMass = this.Draggable.Rb.transform.InverseTransformPoint(this.CenterOfMass.position);
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x00164650 File Offset: 0x00162850
		private void Update()
		{
			SoftJointLimit angularZLimit = this.Joint.angularZLimit;
			angularZLimit.limit = Mathf.Lerp(this.ClampAngle_MinLiquid, this.ClampAngle_MaxLiquid, this.Container.CurrentLiquidLevel);
			this.Joint.angularZLimit = angularZLimit;
			this.Pourable.AngleFromUpToPour = Mathf.Lerp(this.AngleToPour_MinLiquid, this.AngleToPour_MaxLiquid, this.Container.CurrentLiquidLevel);
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001646BF File Offset: 0x001628BF
		public void SetStatic(bool stat)
		{
			this.Draggable.ClickableEnabled = !stat;
			this.ConvexCollider.enabled = !stat;
			this.ConcaveCollider.enabled = stat;
			this.Draggable.Rb.isKinematic = stat;
		}

		// Token: 0x04003EB9 RID: 16057
		public float ClampAngle_MaxLiquid = 50f;

		// Token: 0x04003EBA RID: 16058
		public float ClampAngle_MinLiquid = 100f;

		// Token: 0x04003EBB RID: 16059
		public float AngleToPour_MaxLiquid = 95f;

		// Token: 0x04003EBC RID: 16060
		public float AngleToPour_MinLiquid = 140f;

		// Token: 0x04003EBD RID: 16061
		[Header("References")]
		public Draggable Draggable;

		// Token: 0x04003EBE RID: 16062
		public DraggableConstraint Constraint;

		// Token: 0x04003EBF RID: 16063
		public Collider ConcaveCollider;

		// Token: 0x04003EC0 RID: 16064
		public Collider ConvexCollider;

		// Token: 0x04003EC1 RID: 16065
		public Transform CenterOfMass;

		// Token: 0x04003EC2 RID: 16066
		public ConfigurableJoint Joint;

		// Token: 0x04003EC3 RID: 16067
		public Rigidbody Anchor;

		// Token: 0x04003EC4 RID: 16068
		public LiquidContainer Container;

		// Token: 0x04003EC5 RID: 16069
		public Fillable Fillable;

		// Token: 0x04003EC6 RID: 16070
		public PourableModule Pourable;

		// Token: 0x04003EC7 RID: 16071
		public GameObject FilterPaper;
	}
}
