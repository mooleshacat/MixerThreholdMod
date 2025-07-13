using System;
using FishNet;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200055B RID: 1371
	public class IdleBehaviour : Behaviour
	{
		// Token: 0x06002082 RID: 8322 RVA: 0x00085751 File Offset: 0x00083951
		protected override void Begin()
		{
			base.Begin();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00085759 File Offset: 0x00083959
		protected override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00085764 File Offset: 0x00083964
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IdlePoint == null)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (!base.Npc.Movement.IsAsCloseAsPossible(this.IdlePoint.position, 0.5f))
				{
					this.facingDir = false;
					base.SetDestination(this.IdlePoint.position, true);
					return;
				}
				if (!this.facingDir)
				{
					this.facingDir = true;
					base.Npc.Movement.FaceDirection(this.IdlePoint.forward, 0.5f);
					return;
				}
			}
			else
			{
				this.facingDir = false;
			}
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00085813 File Offset: 0x00083A13
		protected override void Pause()
		{
			base.Pause();
			this.facingDir = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00085839 File Offset: 0x00083A39
		protected override void End()
		{
			base.End();
			this.facingDir = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0008585F File Offset: 0x00083A5F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00085878 File Offset: 0x00083A78
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00085891 File Offset: 0x00083A91
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0008589F File Offset: 0x00083A9F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400190F RID: 6415
		public Transform IdlePoint;

		// Token: 0x04001910 RID: 6416
		private bool facingDir;

		// Token: 0x04001911 RID: 6417
		private bool dll_Excuted;

		// Token: 0x04001912 RID: 6418
		private bool dll_Excuted;
	}
}
