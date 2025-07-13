using System;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056D RID: 1389
	public class UnconsciousBehaviour : Behaviour
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x00089A64 File Offset: 0x00087C64
		protected override void Begin()
		{
			base.Begin();
			base.Npc.behaviour.RagdollBehaviour.Disable();
			base.Npc.Movement.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
			base.Npc.Movement.SetRagdollDraggable(true);
			base.Npc.dialogueHandler.HideWorldspaceDialogue();
			base.Npc.awareness.SetAwarenessActive(false);
			base.Npc.Avatar.EmotionManager.ClearOverrides();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Sleeping", "Dead", 0f, 20);
			this.Particles.Play();
			base.Npc.PlayVO(EVOLineType.Die);
			this.timeOnLastSnore = Time.time;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00089B3C File Offset: 0x00087D3C
		protected override void End()
		{
			base.End();
			base.Npc.awareness.SetAwarenessActive(true);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("Dead");
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			this.Particles.Stop();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00089BA6 File Offset: 0x00087DA6
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.PlaySnoreSounds && Time.time - this.timeOnLastSnore > 6f)
			{
				base.Npc.PlayVO(EVOLineType.Snore);
				this.timeOnLastSnore = Time.time;
			}
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x00089BF0 File Offset: 0x00087DF0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00089C09 File Offset: 0x00087E09
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00089C22 File Offset: 0x00087E22
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x00089C30 File Offset: 0x00087E30
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001990 RID: 6544
		public const float SnoreInterval = 6f;

		// Token: 0x04001991 RID: 6545
		public ParticleSystem Particles;

		// Token: 0x04001992 RID: 6546
		public bool PlaySnoreSounds = true;

		// Token: 0x04001993 RID: 6547
		private float timeOnLastSnore;

		// Token: 0x04001994 RID: 6548
		private bool dll_Excuted;

		// Token: 0x04001995 RID: 6549
		private bool dll_Excuted;
	}
}
