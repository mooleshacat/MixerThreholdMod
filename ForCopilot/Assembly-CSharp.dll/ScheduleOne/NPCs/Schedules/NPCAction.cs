using System;
using FishNet;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B4 RID: 1204
	[Serializable]
	public abstract class NPCAction : NetworkBehaviour
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x00071A92 File Offset: 0x0006FC92
		protected string ActionName
		{
			get
			{
				return "ActionName";
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x00071A99 File Offset: 0x0006FC99
		public bool IsEvent
		{
			get
			{
				return this is NPCEvent;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x00071AA4 File Offset: 0x0006FCA4
		public bool IsSignal
		{
			get
			{
				return this is NPCSignal;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x00071AAF File Offset: 0x0006FCAF
		// (set) Token: 0x060019D5 RID: 6613 RVA: 0x00071AB7 File Offset: 0x0006FCB7
		public bool IsActive { get; protected set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x00071AC0 File Offset: 0x0006FCC0
		// (set) Token: 0x060019D7 RID: 6615 RVA: 0x00071AC8 File Offset: 0x0006FCC8
		public bool HasStarted { get; protected set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x00071AD1 File Offset: 0x0006FCD1
		public virtual int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00071AD9 File Offset: 0x0006FCD9
		protected NPCMovement movement
		{
			get
			{
				return this.npc.Movement;
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00071AE6 File Offset: 0x0006FCE6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCAction_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00071AFA File Offset: 0x0006FCFA
		protected override void OnValidate()
		{
			base.OnValidate();
			this.GetReferences();
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00071B08 File Offset: 0x0006FD08
		private void GetReferences()
		{
			if (this.npc == null)
			{
				this.npc = base.GetComponentInParent<NPC>();
			}
			if (this.schedule == null)
			{
				this.schedule = base.GetComponentInParent<NPCScheduleManager>();
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00071B3E File Offset: 0x0006FD3E
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPassed));
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00071B68 File Offset: 0x0006FD68
		public virtual void Started()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " started");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00071BB8 File Offset: 0x0006FDB8
		public virtual void LateStarted()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " late started");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00071C08 File Offset: 0x0006FE08
		public virtual void JumpTo()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " jumped to");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00071C58 File Offset: 0x0006FE58
		public virtual void End()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " ended");
			}
			this.IsActive = false;
			this.schedule.ActiveAction = null;
			this.HasStarted = false;
			if (this.onEnded != null)
			{
				this.onEnded();
			}
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x00071CBC File Offset: 0x0006FEBC
		public virtual void Interrupt()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " interrupted");
			}
			this.IsActive = false;
			this.schedule.ActiveAction = null;
			if (!this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Add(this);
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00071D28 File Offset: 0x0006FF28
		public virtual void Resume()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " resumed");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			if (this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Remove(this);
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00071D98 File Offset: 0x0006FF98
		public virtual void ResumeFailed()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " resume failed");
			}
			this.HasStarted = false;
			if (this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Remove(this);
			}
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00071DF9 File Offset: 0x0006FFF9
		public virtual void Skipped()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(base.gameObject.name + " skipped");
			}
			this.IsActive = false;
			this.HasStarted = false;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveUpdate()
		{
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveMinPassed()
		{
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00071E36 File Offset: 0x00070036
		public virtual void PendingMinPassed()
		{
			if (this.HasStarted && !this.IsActive && !NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.GetEndTime()))
			{
				this.ResumeFailed();
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void MinPassed()
		{
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00071E66 File Offset: 0x00070066
		public virtual bool ShouldStart()
		{
			return base.gameObject.activeInHierarchy;
		}

		// Token: 0x060019EB RID: 6635
		public abstract string GetName();

		// Token: 0x060019EC RID: 6636
		public abstract string GetTimeDescription();

		// Token: 0x060019ED RID: 6637
		public abstract int GetEndTime();

		// Token: 0x060019EE RID: 6638 RVA: 0x00071E78 File Offset: 0x00070078
		protected void SetDestination(Vector3 position, bool teleportIfFail = true)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (teleportIfFail && this.consecutivePathingFailures >= 5 && !this.movement.CanGetTo(position, 1f))
			{
				Console.LogWarning(this.npc.fullName + " too many pathing failures. Warping to " + position.ToString(), null);
				this.movement.Warp(position);
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.movement.SetDestination(position, new Action<NPCMovement.WalkResult>(this.WalkCallback), 1f, 1f);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00071F0C File Offset: 0x0007010C
		protected virtual void WalkCallback(NPCMovement.WalkResult result)
		{
			if (!this.IsActive)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Console.Log("Walk callback result: " + result.ToString(), null);
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00071F66 File Offset: 0x00070166
		public virtual void SetStartTime(int startTime)
		{
			this.StartTime = startTime;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00071F6F File Offset: 0x0007016F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00071F82 File Offset: 0x00070182
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00071F95 File Offset: 0x00070195
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00071FA3 File Offset: 0x000701A3
		protected virtual void dll()
		{
			this.GetReferences();
		}

		// Token: 0x04001666 RID: 5734
		public const int MAX_CONSECUTIVE_PATHING_FAILURES = 5;

		// Token: 0x04001669 RID: 5737
		[SerializeField]
		protected int priority;

		// Token: 0x0400166A RID: 5738
		[Header("Timing Settings")]
		public int StartTime;

		// Token: 0x0400166B RID: 5739
		protected NPC npc;

		// Token: 0x0400166C RID: 5740
		protected NPCScheduleManager schedule;

		// Token: 0x0400166D RID: 5741
		public Action onEnded;

		// Token: 0x0400166E RID: 5742
		protected int consecutivePathingFailures;

		// Token: 0x0400166F RID: 5743
		private bool dll_Excuted;

		// Token: 0x04001670 RID: 5744
		private bool dll_Excuted;
	}
}
