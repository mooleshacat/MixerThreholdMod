using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Materials;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009D7 RID: 2519
	public class AvatarFootstepDetector : MonoBehaviour
	{
		// Token: 0x06004418 RID: 17432 RVA: 0x0011DEF4 File Offset: 0x0011C0F4
		private void LateUpdate()
		{
			if (!this.Avatar.Anim.animator.enabled)
			{
				this.leftDown = false;
				this.rightDown = false;
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.LeftBone.gameObject.activeInHierarchy)
			{
				return;
			}
			if (Vector3.Distance(this.ReferencePoint.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 20f)
			{
				this.leftDown = false;
				this.rightDown = false;
				return;
			}
			if (this.LeftBone.position.y - this.ReferencePoint.position.y < this.StepThreshold)
			{
				if (!this.leftDown)
				{
					this.leftDown = true;
					this.TriggerStep();
				}
			}
			else
			{
				this.leftDown = false;
			}
			if (this.RightBone.position.y - this.ReferencePoint.position.y < this.StepThreshold)
			{
				if (!this.rightDown)
				{
					this.rightDown = true;
					this.TriggerStep();
					return;
				}
			}
			else
			{
				this.rightDown = false;
			}
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0011E004 File Offset: 0x0011C204
		public void TriggerStep()
		{
			EMaterialType arg;
			if (this.IsGrounded(out arg))
			{
				this.onStep.Invoke(arg, 1f);
			}
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0011E02C File Offset: 0x0011C22C
		public bool IsGrounded(out EMaterialType surfaceType)
		{
			surfaceType = EMaterialType.Generic;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.ReferencePoint.position + Vector3.up * 0.1f, Vector3.down, ref raycastHit, 0.25f, this.GroundDetectionMask, 1))
			{
				MaterialTag componentInParent = raycastHit.collider.GetComponentInParent<MaterialTag>();
				if (componentInParent != null)
				{
					surfaceType = componentInParent.MaterialType;
				}
				return true;
			}
			return false;
		}

		// Token: 0x040030F2 RID: 12530
		public const float MAX_DETECTION_RANGE = 20f;

		// Token: 0x040030F3 RID: 12531
		public const float GROUND_DETECTION_RANGE = 0.25f;

		// Token: 0x040030F4 RID: 12532
		public Avatar Avatar;

		// Token: 0x040030F5 RID: 12533
		public Transform ReferencePoint;

		// Token: 0x040030F6 RID: 12534
		public Transform LeftBone;

		// Token: 0x040030F7 RID: 12535
		public Transform RightBone;

		// Token: 0x040030F8 RID: 12536
		public float StepThreshold = 0.1f;

		// Token: 0x040030F9 RID: 12537
		public LayerMask GroundDetectionMask;

		// Token: 0x040030FA RID: 12538
		private bool leftDown;

		// Token: 0x040030FB RID: 12539
		private bool rightDown;

		// Token: 0x040030FC RID: 12540
		public UnityEvent<EMaterialType, float> onStep = new UnityEvent<EMaterialType, float>();
	}
}
