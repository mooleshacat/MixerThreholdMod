using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200055A RID: 1370
	public class HeavyFlinchBehaviour : Behaviour
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x0008567C File Offset: 0x0008387C
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (this.remainingFlinchTime > 0f)
			{
				this.remainingFlinchTime = Mathf.Clamp(this.remainingFlinchTime -= Time.deltaTime, 0f, 1.25f);
			}
			if (this.remainingFlinchTime <= 0f)
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000856DA File Offset: 0x000838DA
		public void Flinch()
		{
			this.remainingFlinchTime += 1.25f;
			if (!base.Enabled)
			{
				base.Enable_Networked(null);
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000856FD File Offset: 0x000838FD
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00085716 File Offset: 0x00083916
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x0008572F File Offset: 0x0008392F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0008573D File Offset: 0x0008393D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400190B RID: 6411
		public const float FLINCH_DURATION = 1.25f;

		// Token: 0x0400190C RID: 6412
		private float remainingFlinchTime;

		// Token: 0x0400190D RID: 6413
		private bool dll_Excuted;

		// Token: 0x0400190E RID: 6414
		private bool dll_Excuted;
	}
}
