using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000529 RID: 1321
	public class DeadBehaviour : Behaviour
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0007C0AB File Offset: 0x0007A2AB
		public bool IsInMedicalCenter
		{
			get
			{
				return base.Npc.CurrentBuilding == Singleton<Map>.Instance.MedicalCentre;
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0007C0C7 File Offset: 0x0007A2C7
		private void Start()
		{
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0007C0E9 File Offset: 0x0007A2E9
		private void OnDestroy()
		{
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0007C10C File Offset: 0x0007A30C
		protected override void Begin()
		{
			base.Begin();
			base.Npc.behaviour.RagdollBehaviour.Disable();
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				this.EnterMedicalCentre();
			}
			else
			{
				base.Npc.Movement.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
				base.Npc.Movement.SetRagdollDraggable(true);
			}
			base.Npc.dialogueHandler.HideWorldspaceDialogue();
			base.Npc.awareness.SetAwarenessActive(false);
			base.Npc.Avatar.EmotionManager.ClearOverrides();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Sleeping", "Dead", 0f, 20);
			base.Npc.PlayVO(EVOLineType.Die);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0007C1E4 File Offset: 0x0007A3E4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!this.IsInMedicalCenter && !base.Npc.Avatar.Ragdolled)
			{
				if (base.Npc.Movement.IsMoving)
				{
					base.Npc.Movement.Stop();
				}
				this.EnterMedicalCentre();
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0007C239 File Offset: 0x0007A439
		private void SleepStart()
		{
			if (base.Active && !this.IsInMedicalCenter)
			{
				this.EnterMedicalCentre();
			}
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0007C254 File Offset: 0x0007A454
		private void EnterMedicalCentre()
		{
			Console.Log(base.Npc.fullName + " entering medical center", null);
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			base.Npc.EnterBuilding(null, Singleton<Map>.Instance.MedicalCentre.GUID.ToString(), 0);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0007C2C8 File Offset: 0x0007A4C8
		protected override void End()
		{
			base.End();
			base.Npc.awareness.SetAwarenessActive(true);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("Dead");
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			if (this.IsInMedicalCenter)
			{
				base.Npc.ExitBuilding("");
			}
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0007C33F File Offset: 0x0007A53F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0007C358 File Offset: 0x0007A558
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0007C371 File Offset: 0x0007A571
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0007C37F File Offset: 0x0007A57F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400180B RID: 6155
		private bool dll_Excuted;

		// Token: 0x0400180C RID: 6156
		private bool dll_Excuted;
	}
}
