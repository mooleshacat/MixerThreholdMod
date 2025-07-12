using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056B RID: 1387
	public class SentryBehaviour : Behaviour
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x00089761 File Offset: 0x00087961
		// (set) Token: 0x0600214B RID: 8523 RVA: 0x00089769 File Offset: 0x00087969
		public SentryLocation AssignedLocation { get; private set; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x00089772 File Offset: 0x00087972
		private Transform standPoint
		{
			get
			{
				return this.AssignedLocation.StandPoints[this.AssignedLocation.AssignedOfficers.IndexOf(this.officer)];
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0008979A File Offset: 0x0008799A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.SentryBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00085751 File Offset: 0x00083951
		protected override void Begin()
		{
			base.Begin();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00085759 File Offset: 0x00083959
		protected override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000897AE File Offset: 0x000879AE
		protected override void End()
		{
			base.End();
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000897C5 File Offset: 0x000879C5
		protected override void Pause()
		{
			base.Pause();
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000897DC File Offset: 0x000879DC
		public void AssignLocation(SentryLocation loc)
		{
			if (this.AssignedLocation != null)
			{
				this.UnassignLocation();
			}
			this.AssignedLocation = loc;
			this.AssignedLocation.AssignedOfficers.Add(this.officer);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0008980F File Offset: 0x00087A0F
		public void UnassignLocation()
		{
			if (this.AssignedLocation != null)
			{
				this.AssignedLocation.AssignedOfficers.Remove(this.officer);
				this.AssignedLocation = null;
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00089840 File Offset: 0x00087A40
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(1930, this.FLASHLIGHT_MAX_TIME))
			{
				if (this.UseFlashlight && !this.flashlightEquipped)
				{
					this.SetFlashlightEquipped(true);
				}
			}
			else if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			this.officer.BodySearchChance = 0.1f;
			if (!base.Npc.Movement.IsMoving)
			{
				if (Vector3.Distance(base.Npc.transform.position, this.standPoint.position) < 2f)
				{
					this.officer.BodySearchChance = 0.75f;
					if (!base.Npc.Movement.FaceDirectionInProgress)
					{
						base.Npc.Movement.FaceDirection(this.standPoint.forward, 0.5f);
						return;
					}
				}
				else if (base.Npc.Movement.CanMove())
				{
					base.Npc.Movement.SetDestination(this.standPoint.position);
				}
			}
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00089956 File Offset: 0x00087B56
		private void SetFlashlightEquipped(bool equipped)
		{
			this.flashlightEquipped = equipped;
			if (equipped)
			{
				base.Npc.SetEquippable_Networked(null, "Tools/Flashlight/Flashlight_AvatarEquippable");
				return;
			}
			base.Npc.SetEquippable_Networked(null, string.Empty);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0008999F File Offset: 0x00087B9F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000899B8 File Offset: 0x00087BB8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000899D1 File Offset: 0x00087BD1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000899DF File Offset: 0x00087BDF
		protected override void dll()
		{
			base.Awake();
			this.officer = (base.Npc as PoliceOfficer);
		}

		// Token: 0x04001984 RID: 6532
		public const float BODY_SEARCH_CHANCE = 0.75f;

		// Token: 0x04001985 RID: 6533
		public const int FLASHLIGHT_MIN_TIME = 1930;

		// Token: 0x04001986 RID: 6534
		public int FLASHLIGHT_MAX_TIME = 500;

		// Token: 0x04001987 RID: 6535
		public const string FLASHLIGHT_ASSET_PATH = "Tools/Flashlight/Flashlight_AvatarEquippable";

		// Token: 0x04001988 RID: 6536
		public bool UseFlashlight = true;

		// Token: 0x04001989 RID: 6537
		private bool flashlightEquipped;

		// Token: 0x0400198B RID: 6539
		private PoliceOfficer officer;

		// Token: 0x0400198C RID: 6540
		private bool dll_Excuted;

		// Token: 0x0400198D RID: 6541
		private bool dll_Excuted;
	}
}
