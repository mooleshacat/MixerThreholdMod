using System;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000353 RID: 851
	public class DraggableConstraint : MonoBehaviour
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x000527BA File Offset: 0x000509BA
		private Vector3 RelativePos
		{
			get
			{
				if (!(this.Container != null))
				{
					return base.transform.localPosition;
				}
				return this.Container.InverseTransformPoint(base.transform.position);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000527EC File Offset: 0x000509EC
		private void Start()
		{
			this.draggable = base.GetComponent<Draggable>();
			if (this.ClampUpDirection)
			{
				this.joint = this.draggable.Rb.gameObject.AddComponent<ConfigurableJoint>();
				if (this.Anchor == null && this.Container != null)
				{
					this.Container.gameObject.AddComponent<Rigidbody>();
					this.Anchor = this.Container.gameObject.GetComponent<Rigidbody>();
					this.Anchor.isKinematic = true;
					this.Anchor.useGravity = false;
				}
				this.joint.connectedBody = this.Anchor;
				this.joint.zMotion = 0;
				this.joint.angularXMotion = 0;
				this.joint.angularYMotion = 0;
				this.joint.angularZMotion = 1;
			}
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000528C8 File Offset: 0x00050AC8
		public void SetContainer(Transform container)
		{
			this.Container = container;
			this.startLocalPos = this.RelativePos;
			if (this.joint != null && this.Anchor == null && this.Container != null)
			{
				this.Anchor = this.Container.gameObject.AddComponent<Rigidbody>();
				this.Anchor.isKinematic = true;
				this.Anchor.useGravity = false;
				this.joint.connectedBody = this.Anchor;
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00052951 File Offset: 0x00050B51
		protected virtual void FixedUpdate()
		{
			if (this.AlignUpToContainerPlane)
			{
				this.AlignToContainerPlane();
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00052961 File Offset: 0x00050B61
		protected virtual void LateUpdate()
		{
			if (this.ProportionalZClamp)
			{
				this.ProportionalClamp();
			}
			if (this.ClampUpDirection)
			{
				this.ClampUpRot();
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00052980 File Offset: 0x00050B80
		private void ProportionalClamp()
		{
			if (this.Container == null)
			{
				return;
			}
			if (this.draggable == null)
			{
				return;
			}
			float num = Mathf.Clamp(Mathf.Abs(this.RelativePos.x) / this.startLocalPos.x, 0f, 1f);
			float num2 = Mathf.Abs(this.startLocalPos.z) * num;
			Vector3 vector = this.Container.InverseTransformPoint(this.draggable.originalHitPoint);
			vector.z = Mathf.Clamp(vector.z, -num2, num2);
			Vector3 originalHitPoint = this.Container.TransformPoint(vector);
			this.draggable.SetOriginalHitPoint(originalHitPoint);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00052A30 File Offset: 0x00050C30
		private void LockRotationX()
		{
			Vector3 eulerAngles = (base.transform.rotation * Quaternion.Inverse(this.Container.rotation)).eulerAngles;
			eulerAngles.x = 0f;
			base.transform.rotation = this.Container.rotation * Quaternion.Euler(eulerAngles);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00052A94 File Offset: 0x00050C94
		private void LockRotationY()
		{
			Vector3 eulerAngles = (base.transform.rotation * Quaternion.Inverse(this.Container.rotation)).eulerAngles;
			eulerAngles.y = 0f;
			base.transform.rotation = this.Container.rotation * Quaternion.Euler(eulerAngles);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00052AF8 File Offset: 0x00050CF8
		private void AlignToContainerPlane()
		{
			Vector3 forward = this.Container.forward;
			Quaternion quaternion = Quaternion.LookRotation(forward, base.transform.up);
			Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, forward).normalized;
			Quaternion.FromToRotation(base.transform.forward, normalized) * quaternion;
			base.transform.rotation = quaternion;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00052B64 File Offset: 0x00050D64
		private void ClampUpRot()
		{
			if (this.joint == null)
			{
				Console.LogWarning("No joint found on DraggableConstraint, cannot clamp up rotation", null);
				return;
			}
			Vector3.Angle(this.draggable.transform.up, Vector3.up);
			SoftJointLimit angularZLimit = this.joint.angularZLimit;
			angularZLimit.limit = this.UpDirectionMaxDifference;
			this.joint.angularZLimit = angularZLimit;
		}

		// Token: 0x04001233 RID: 4659
		public Transform Container;

		// Token: 0x04001234 RID: 4660
		public Rigidbody Anchor;

		// Token: 0x04001235 RID: 4661
		public bool ProportionalZClamp;

		// Token: 0x04001236 RID: 4662
		public bool AlignUpToContainerPlane;

		// Token: 0x04001237 RID: 4663
		[Header("Up Direction Clamping")]
		public bool ClampUpDirection;

		// Token: 0x04001238 RID: 4664
		public float UpDirectionMaxDifference = 45f;

		// Token: 0x04001239 RID: 4665
		private Vector3 startLocalPos;

		// Token: 0x0400123A RID: 4666
		private Draggable draggable;

		// Token: 0x0400123B RID: 4667
		private ConfigurableJoint joint;
	}
}
