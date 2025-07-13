using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.Map;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B2 RID: 1202
	public class NPCEvent_StayInBuilding : NPCEvent
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x000712D6 File Offset: 0x0006F4D6
		public new string ActionName
		{
			get
			{
				return "Stay in Building";
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x000712DD File Offset: 0x0006F4DD
		private bool InBuilding
		{
			get
			{
				return this.npc.CurrentBuilding == this.Building;
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000712F5 File Offset: 0x0006F4F5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCEvent_StayInBuilding_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0007130C File Offset: 0x0006F50C
		public override string GetName()
		{
			if (this.Building == null)
			{
				return this.ActionName + " (No building set)";
			}
			return this.ActionName + " (" + this.Building.BuildingName + ")";
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00071358 File Offset: 0x0006F558
		public override void Started()
		{
			base.Started();
			if (!base.IsActive)
			{
				return;
			}
			if (this.Building == null)
			{
				return;
			}
			if (InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00071394 File Offset: 0x0006F594
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log("StayInBuilding: ActiveMinPassed");
				Debug.Log("In building: " + this.InBuilding.ToString());
				Debug.Log("Is entering: " + this.IsEntering.ToString());
			}
			if (this.Building == null || this.Building.Doors.Length == 0)
			{
				return;
			}
			if (!this.InBuilding && !this.IsEntering && (!this.npc.Movement.IsMoving || Vector3.Distance(this.npc.Movement.CurrentDestination, this.GetEntryPoint().position) > 2f))
			{
				if (Vector3.Distance(this.npc.transform.position, this.GetEntryPoint().position) < 0.5f)
				{
					this.PlayEnterAnimation();
					return;
				}
				if (this.npc.Movement.CanMove())
				{
					base.SetDestination(this.GetEntryPoint().position, true);
				}
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000714C4 File Offset: 0x0006F6C4
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.Building == null || this.Building.Doors.Length == 0)
			{
				return;
			}
			if (InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00071502 File Offset: 0x0006F702
		public override void JumpTo()
		{
			base.JumpTo();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			this.PlayEnterAnimation();
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0007153A File Offset: 0x0006F73A
		public override void End()
		{
			base.End();
			this.CancelEnter();
			if (this.InBuilding)
			{
				this.ExitBuilding();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00071567 File Offset: 0x0006F767
		public override void Interrupt()
		{
			base.Interrupt();
			this.CancelEnter();
			if (this.InBuilding)
			{
				this.ExitBuilding();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00071594 File Offset: 0x0006F794
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0007159C File Offset: 0x0006F79C
		public override void Resume()
		{
			base.Resume();
			if (!this.InBuilding && InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000715C5 File Offset: 0x0006F7C5
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Success || result == NPCMovement.WalkResult.Partial)
			{
				this.PlayEnterAnimation();
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000715ED File Offset: 0x0006F7ED
		[ObserversRpc(RunLocally = true)]
		private void PlayEnterAnimation()
		{
			this.RpcWriter___Observers_PlayEnterAnimation_2166136261();
			this.RpcLogic___PlayEnterAnimation_2166136261();
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000715FB File Offset: 0x0006F7FB
		private void CancelEnter()
		{
			this.IsEntering = false;
			if (this.enterRoutine != null)
			{
				base.StopCoroutine(this.enterRoutine);
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00071618 File Offset: 0x0006F818
		private void EnterBuilding(int doorIndex)
		{
			if (this.Building == null)
			{
				Console.LogWarning("Building is null in StayInBuilding event", null);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				this.npc.EnterBuilding(null, this.Building.GUID.ToString(), doorIndex);
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0007166C File Offset: 0x0006F86C
		private void ExitBuilding()
		{
			if (InstanceFinder.IsServer)
			{
				this.npc.ExitBuilding("");
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00071688 File Offset: 0x0006F888
		private Transform GetEntryPoint()
		{
			if (this.Door != null)
			{
				return this.Door.AccessPoint;
			}
			if (this.Building == null)
			{
				return null;
			}
			StaticDoor closestDoor = this.Building.GetClosestDoor(this.npc.Movement.FootPosition, true);
			if (closestDoor == null)
			{
				return null;
			}
			return closestDoor.AccessPoint;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x000716F0 File Offset: 0x0006F8F0
		private StaticDoor GetDoor(out int doorIndex)
		{
			doorIndex = -1;
			if (this.Door != null)
			{
				return this.Door;
			}
			if (this.Building == null)
			{
				return null;
			}
			if (this.npc == null)
			{
				return null;
			}
			StaticDoor closestDoor = this.Building.GetClosestDoor(this.npc.Movement.FootPosition, true);
			doorIndex = ArrayExt.IndexOf<StaticDoor>(this.Building.Doors, closestDoor);
			return closestDoor;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00071766 File Offset: 0x0006F966
		[CompilerGenerated]
		private IEnumerator <PlayEnterAnimation>g__Enter|19_0()
		{
			this.IsEntering = true;
			yield return new WaitUntil(() => !this.npc.Movement.IsMoving);
			int doorIndex;
			StaticDoor door = this.GetDoor(out doorIndex);
			if (door != null)
			{
				Transform faceDir = door.transform;
				this.npc.Movement.FacePoint(faceDir.position, 0.5f);
				float t = 0f;
				while (Vector3.SignedAngle(this.npc.Avatar.transform.forward, faceDir.position - this.npc.Avatar.CenterPoint, Vector3.up) > 15f && t < 1f)
				{
					yield return new WaitForEndOfFrame();
					t += Time.deltaTime;
				}
				faceDir = null;
			}
			this.npc.Avatar.Anim.SetTrigger("GrabItem");
			yield return new WaitForSeconds(0.6f);
			this.IsEntering = false;
			this.enterRoutine = null;
			this.EnterBuilding(doorIndex);
			yield break;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0007178A File Offset: 0x0006F98A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_PlayEnterAnimation_2166136261));
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x000717BA File Offset: 0x0006F9BA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x000717D3 File Offset: 0x0006F9D3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000717E4 File Offset: 0x0006F9E4
		private void RpcWriter___Observers_PlayEnterAnimation_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0007188D File Offset: 0x0006FA8D
		private void RpcLogic___PlayEnterAnimation_2166136261()
		{
			if (this.IsEntering)
			{
				return;
			}
			this.enterRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PlayEnterAnimation>g__Enter|19_0());
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000718B0 File Offset: 0x0006FAB0
		private void RpcReader___Observers_PlayEnterAnimation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlayEnterAnimation_2166136261();
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000718DA File Offset: 0x0006FADA
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x0400165A RID: 5722
		public NPCEnterableBuilding Building;

		// Token: 0x0400165B RID: 5723
		[Header("Optionally specify door to use. Otherwise closest door will be used.")]
		public StaticDoor Door;

		// Token: 0x0400165C RID: 5724
		private bool IsEntering;

		// Token: 0x0400165D RID: 5725
		private Coroutine enterRoutine;

		// Token: 0x0400165E RID: 5726
		private bool dll_Excuted;

		// Token: 0x0400165F RID: 5727
		private bool dll_Excuted;
	}
}
