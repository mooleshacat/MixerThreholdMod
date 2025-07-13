using System;
using ScheduleOne.VoiceOver;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000528 RID: 1320
	public class CoweringBehaviour : Behaviour
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x0007BF09 File Offset: 0x0007A109
		protected override void Begin()
		{
			base.Begin();
			this.SetCowering(true);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0007BF18 File Offset: 0x0007A118
		public override void Enable()
		{
			base.Enable();
			Console.Log("CoweringBehaviour Enabled", null);
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0007BF2B File Offset: 0x0007A12B
		protected override void End()
		{
			base.End();
			this.SetCowering(false);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x0007BF3A File Offset: 0x0007A13A
		protected override void Resume()
		{
			base.Resume();
			this.SetCowering(true);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0007BF49 File Offset: 0x0007A149
		protected override void Pause()
		{
			base.Pause();
			this.SetCowering(false);
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0007BF68 File Offset: 0x0007A168
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			base.Npc.Avatar.LookController.OverrideLookTarget(base.Npc.Movement.FootPosition + base.Npc.Avatar.transform.forward * 2f, 5, false);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0007BFC8 File Offset: 0x0007A1C8
		private void SetCowering(bool cowering)
		{
			base.Npc.Avatar.Anim.SetCrouched(cowering);
			base.Npc.Avatar.Anim.SetBool("HandsUp", cowering);
			if (cowering)
			{
				base.Npc.PlayVO(EVOLineType.Scared);
				base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("cowering", 80, 0f));
				return;
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("cowering");
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0007C057 File Offset: 0x0007A257
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0007C070 File Offset: 0x0007A270
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0007C089 File Offset: 0x0007A289
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0007C097 File Offset: 0x0007A297
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001809 RID: 6153
		private bool dll_Excuted;

		// Token: 0x0400180A RID: 6154
		private bool dll_Excuted;
	}
}
