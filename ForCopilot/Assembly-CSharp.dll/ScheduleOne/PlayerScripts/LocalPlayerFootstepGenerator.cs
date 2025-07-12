using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000613 RID: 1555
	public class LocalPlayerFootstepGenerator : MonoBehaviour
	{
		// Token: 0x06002604 RID: 9732 RVA: 0x00099958 File Offset: 0x00097B58
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			if (!PlayerSingleton<PlayerMovement>.Instance.canMove)
			{
				this.currentDistance = 0f;
				this.lastFramePosition = base.transform.position;
				return;
			}
			Vector3 position = base.transform.position;
			this.currentDistance += Vector3.Distance(position, this.lastFramePosition) * (PlayerSingleton<PlayerMovement>.Instance.isSprinting ? 0.75f : 1f);
			if (this.currentDistance >= this.DistancePerStep)
			{
				this.currentDistance = 0f;
				this.lastFramePosition = position;
				this.TriggerStep();
			}
			this.lastFramePosition = position;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00099A04 File Offset: 0x00097C04
		public void TriggerStep()
		{
			EMaterialType arg;
			if (this.IsGrounded(out arg))
			{
				this.onStep.Invoke(arg, PlayerSingleton<PlayerMovement>.Instance.isSprinting ? 1f : 0.5f);
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00099A40 File Offset: 0x00097C40
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

		// Token: 0x04001C17 RID: 7191
		public float DistancePerStep = 0.75f;

		// Token: 0x04001C18 RID: 7192
		public Transform ReferencePoint;

		// Token: 0x04001C19 RID: 7193
		public LayerMask GroundDetectionMask;

		// Token: 0x04001C1A RID: 7194
		public UnityEvent<EMaterialType, float> onStep = new UnityEvent<EMaterialType, float>();

		// Token: 0x04001C1B RID: 7195
		private float currentDistance;

		// Token: 0x04001C1C RID: 7196
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
