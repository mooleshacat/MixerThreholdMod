using System;
using FishNet.Object;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.Tools;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000491 RID: 1169
	public class NPCAnimation : NetworkBehaviour
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x000696BB File Offset: 0x000678BB
		private void Start()
		{
			this.npc = base.GetComponent<NPC>();
			NPC npc = this.npc;
			npc.onExitVehicle = (Action<LandVehicle>)Delegate.Combine(npc.onExitVehicle, new Action<LandVehicle>(delegate(LandVehicle <p0>)
			{
				this.ResetVelocityCalculations();
			}));
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x000696F0 File Offset: 0x000678F0
		protected virtual void LateUpdate()
		{
			if (this.anim.enabled && !this.anim.IsAvatarCulled && this.npc.isVisible)
			{
				this.UpdateMovementAnimation();
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00069720 File Offset: 0x00067920
		protected virtual void UpdateMovementAnimation()
		{
			Vector3 vector = this.Avatar.transform.InverseTransformVector(this.velocityCalculator.Velocity) / 8f;
			this.anim.SetDirection(this.WalkMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
			this.anim.SetStrafe(this.WalkMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x000697AD File Offset: 0x000679AD
		public virtual void SetRagdollActive(bool active)
		{
			this.Avatar.SetRagdollPhysicsEnabled(active, true);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x000697BC File Offset: 0x000679BC
		public void ResetVelocityCalculations()
		{
			this.velocityCalculator.FlushBuffer();
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000697C9 File Offset: 0x000679C9
		public void StandupStart()
		{
			this.movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("ragdollstandup", 100, 0f));
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000697EC File Offset: 0x000679EC
		public void StandupDone()
		{
			this.movement.SpeedController.RemoveSpeedControl("ragdollstandup");
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00069813 File Offset: 0x00067A13
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00069826 File Offset: 0x00067A26
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00069839 File Offset: 0x00067A39
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00069839 File Offset: 0x00067A39
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001568 RID: 5480
		[Header("References")]
		public Avatar Avatar;

		// Token: 0x04001569 RID: 5481
		[SerializeField]
		protected AvatarAnimation anim;

		// Token: 0x0400156A RID: 5482
		[SerializeField]
		protected NPCMovement movement;

		// Token: 0x0400156B RID: 5483
		protected NPC npc;

		// Token: 0x0400156C RID: 5484
		[SerializeField]
		protected SmoothedVelocityCalculator velocityCalculator;

		// Token: 0x0400156D RID: 5485
		[Header("Settings")]
		public AnimationCurve WalkMapCurve;

		// Token: 0x0400156E RID: 5486
		private bool dll_Excuted;

		// Token: 0x0400156F RID: 5487
		private bool dll_Excuted;
	}
}
