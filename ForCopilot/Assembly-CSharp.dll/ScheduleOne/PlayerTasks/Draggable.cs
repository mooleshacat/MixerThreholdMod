using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000351 RID: 849
	public class Draggable : Clickable
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0005254E File Offset: 0x0005074E
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x00052556 File Offset: 0x00050756
		public Rigidbody Rb { get; protected set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0005255F File Offset: 0x0005075F
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x00052567 File Offset: 0x00050767
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.OpenHand;

		// Token: 0x06001303 RID: 4867 RVA: 0x00052570 File Offset: 0x00050770
		protected virtual void Awake()
		{
			this.Rb = base.GetComponent<Rigidbody>();
			this.constraint = base.GetComponent<DraggableConstraint>();
			if (base.gameObject.isStatic)
			{
				Console.LogWarning("Draggable object is static, this will cause issues with dragging.", null);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000525A4 File Offset: 0x000507A4
		protected virtual void FixedUpdate()
		{
			if (this.Rb == null)
			{
				return;
			}
			this.Rb.drag = (base.IsHeld ? this.HeldRBDrag : this.NormalRBDrag);
			if (!base.IsHeld && !this.Rb.isKinematic)
			{
				this.Rb.angularVelocity = Vector3.ClampMagnitude(this.Rb.angularVelocity, this.Rb.angularVelocity.magnitude * 0.9f);
				this.Rb.velocity = Vector3.ClampMagnitude(this.Rb.velocity, this.Rb.velocity.magnitude * 0.95f);
				this.Rb.AddForce(Vector3.up * this.idleUpForce, 5);
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void PostFixedUpdate()
		{
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00052680 File Offset: 0x00050880
		protected virtual void LateUpdate()
		{
			if (this.LocationRestrictionEnabled && Vector3.Distance(base.transform.position, this.Origin) > this.MaxDistanceFromOrigin)
			{
				base.transform.position = this.Origin + (base.transform.position - this.Origin).normalized * this.MaxDistanceFromOrigin;
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x000526F2 File Offset: 0x000508F2
		protected virtual void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.Invoke(other);
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00052708 File Offset: 0x00050908
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			if (this.DisableGravityWhenDragged)
			{
				this.Rb.useGravity = false;
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00052725 File Offset: 0x00050925
		public override void EndClick()
		{
			base.EndClick();
			if (this.DisableGravityWhenDragged && this.Rb != null)
			{
				this.Rb.useGravity = true;
			}
		}

		// Token: 0x0400121F RID: 4639
		[Header("Drag Force")]
		public float DragForceMultiplier = 30f;

		// Token: 0x04001220 RID: 4640
		public Transform DragForceOrigin;

		// Token: 0x04001221 RID: 4641
		[Header("Rotation")]
		public bool RotationEnabled = true;

		// Token: 0x04001222 RID: 4642
		public float TorqueMultiplier = 20f;

		// Token: 0x04001223 RID: 4643
		[Header("Settings")]
		public Draggable.EDragProjectionMode DragProjectionMode;

		// Token: 0x04001224 RID: 4644
		public bool DisableGravityWhenDragged;

		// Token: 0x04001225 RID: 4645
		public float NormalRBDrag = 3f;

		// Token: 0x04001226 RID: 4646
		public float HeldRBDrag = 15f;

		// Token: 0x04001227 RID: 4647
		public bool CanBeMultiDragged = true;

		// Token: 0x0400122A RID: 4650
		[Header("Additional force")]
		public float idleUpForce;

		// Token: 0x0400122B RID: 4651
		[HideInInspector]
		public bool LocationRestrictionEnabled;

		// Token: 0x0400122C RID: 4652
		[HideInInspector]
		public Vector3 Origin = Vector3.zero;

		// Token: 0x0400122D RID: 4653
		[HideInInspector]
		public float MaxDistanceFromOrigin = 0.5f;

		// Token: 0x0400122E RID: 4654
		public UnityEvent<Collider> onTriggerExit;

		// Token: 0x0400122F RID: 4655
		protected DraggableConstraint constraint;

		// Token: 0x02000352 RID: 850
		public enum EDragProjectionMode
		{
			// Token: 0x04001231 RID: 4657
			CameraForward,
			// Token: 0x04001232 RID: 4658
			FlatCameraForward
		}
	}
}
