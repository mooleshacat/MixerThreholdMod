using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000556 RID: 1366
	public class FootPatrolBehaviour : Behaviour
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000849A3 File Offset: 0x00082BA3
		// (set) Token: 0x0600204D RID: 8269 RVA: 0x000849AB File Offset: 0x00082BAB
		public PatrolGroup Group { get; protected set; }

		// Token: 0x0600204E RID: 8270 RVA: 0x000849B4 File Offset: 0x00082BB4
		protected override void Begin()
		{
			base.Begin();
			if (InstanceFinder.IsServer && this.Group == null)
			{
				Console.LogError("Foot patrol behaviour started without a group!", null);
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("footpatrol", 1, 0.08f));
			(base.Npc as PoliceOfficer).BodySearchChance = 0.4f;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00084A1C File Offset: 0x00082C1C
		protected override void Resume()
		{
			base.Resume();
			if (InstanceFinder.IsServer && this.Group == null)
			{
				Console.LogError("Foot patrol behaviour resumed without a group!", null);
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("footpatrol", 1, 0.08f));
			(base.Npc as PoliceOfficer).BodySearchChance = 0.25f;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00084A84 File Offset: 0x00082C84
		protected override void Pause()
		{
			base.Pause();
			base.Npc.Movement.SpeedController.RemoveSpeedControl("footpatrol");
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00084AD8 File Offset: 0x00082CD8
		protected override void End()
		{
			base.End();
			if (this.Group != null)
			{
				this.Group.Members.Remove(base.Npc);
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("footpatrol");
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00084B48 File Offset: 0x00082D48
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(1930, this.FLASHLIGHT_MAX_TIME))
			{
				if (this.UseFlashlight && !this.flashlightEquipped && this.Group.Members.Count > 0 && this.Group.Members[0] == base.Npc)
				{
					this.SetFlashlightEquipped(true);
				}
			}
			else if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			if (this.Group == null)
			{
				return;
			}
			if (!this.Group.Members.Contains(base.Npc))
			{
				Console.LogWarning("Foot patrol behaviour is not in group members list! Adding now", null);
				this.SetGroup(this.Group);
			}
			if (this.Group.IsPaused())
			{
				if (base.Npc.Movement.IsMoving)
				{
					base.Npc.Movement.Stop();
				}
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				return;
			}
			if (this.IsReadyToAdvance())
			{
				if (this.Group.Members.Count > 0 && this.Group.Members[0] == base.Npc && this.Group.IsGroupReadyToAdvance())
				{
					this.Group.AdvanceGroup();
					return;
				}
			}
			else if (!this.IsAtDestination())
			{
				base.Npc.Movement.SetDestination(this.Group.GetDestination(base.Npc));
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00084CC9 File Offset: 0x00082EC9
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

		// Token: 0x06002054 RID: 8276 RVA: 0x00084CF8 File Offset: 0x00082EF8
		public void SetGroup(PatrolGroup group)
		{
			this.Group = group;
			this.Group.Members.Add(base.Npc);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00084D18 File Offset: 0x00082F18
		public bool IsReadyToAdvance()
		{
			Vector3 destination = this.Group.GetDestination(base.Npc);
			return Vector3.Distance(base.transform.position, destination) < 2f || (!base.Npc.Movement.IsMoving && base.Npc.Movement.IsAsCloseAsPossible(this.Group.GetDestination(base.Npc), 3f));
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00084D90 File Offset: 0x00082F90
		private bool IsAtDestination()
		{
			return this.Group != null && Vector3.Distance(base.Npc.Movement.FootPosition, this.Group.GetDestination(base.Npc)) < 2f;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00084DE3 File Offset: 0x00082FE3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00084DFC File Offset: 0x00082FFC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00084E15 File Offset: 0x00083015
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00084E23 File Offset: 0x00083023
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018F8 RID: 6392
		public const float MOVE_SPEED = 0.08f;

		// Token: 0x040018F9 RID: 6393
		public const int FLASHLIGHT_MIN_TIME = 1930;

		// Token: 0x040018FA RID: 6394
		public int FLASHLIGHT_MAX_TIME = 500;

		// Token: 0x040018FB RID: 6395
		public const string FLASHLIGHT_ASSET_PATH = "Tools/Flashlight/Flashlight_AvatarEquippable";

		// Token: 0x040018FC RID: 6396
		public bool UseFlashlight = true;

		// Token: 0x040018FD RID: 6397
		private bool flashlightEquipped;

		// Token: 0x040018FF RID: 6399
		private bool dll_Excuted;

		// Token: 0x04001900 RID: 6400
		private bool dll_Excuted;
	}
}
