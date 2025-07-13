using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B1 RID: 1201
	public class NPCEvent_Sit : NPCEvent
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x00070916 File Offset: 0x0006EB16
		public new string ActionName
		{
			get
			{
				return "Sit";
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00070920 File Offset: 0x0006EB20
		public override string GetName()
		{
			string text = this.ActionName;
			if (this.SeatSet == null)
			{
				text += "(no seat assigned)";
			}
			return text;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00070950 File Offset: 0x0006EB50
		public override void Started()
		{
			base.Started();
			this.seated = false;
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000709A4 File Offset: 0x0006EBA4
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!base.IsActive)
			{
				return;
			}
			if (this.seated)
			{
				this.StartAction(connection, ArrayExt.IndexOf<AvatarSeat>(this.SeatSet.Seats, this.npc.Avatar.Anim.CurrentSeat));
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000709F8 File Offset: 0x0006EBF8
		public override void LateStarted()
		{
			base.LateStarted();
			this.seated = false;
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00070A4C File Offset: 0x0006EC4C
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log("ActiveMinPassed");
				Debug.Log("Moving: " + this.npc.Movement.IsMoving.ToString());
				Debug.Log("At destination: " + this.IsAtDestination().ToString());
				Debug.Log("Seated: " + this.seated.ToString());
			}
			if (!base.IsActive)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.IsAtDestination() || this.seated)
				{
					if (!this.seated)
					{
						if (!this.npc.Movement.FaceDirectionInProgress)
						{
							this.npc.Movement.FaceDirection(this.targetSeat.SittingPoint.forward, 0.5f);
						}
						if (Vector3.Angle(this.npc.Movement.transform.forward, this.targetSeat.SittingPoint.forward) < 10f)
						{
							this.StartAction(null, ArrayExt.IndexOf<AvatarSeat>(this.SeatSet.Seats, this.SeatSet.GetRandomFreeSeat()));
							return;
						}
					}
					else if (!this.npc.Movement.FaceDirectionInProgress && Vector3.Angle(this.npc.Movement.transform.forward, this.targetSeat.SittingPoint.forward) > 15f)
					{
						this.npc.Movement.FaceDirection(this.targetSeat.SittingPoint.forward, 0.5f);
						return;
					}
				}
				else
				{
					base.SetDestination(this.targetSeat.AccessPoint.position, true);
				}
			}
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00070C28 File Offset: 0x0006EE28
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				this.targetSeat = this.SeatSet.GetRandomFreeSeat();
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.targetSeat.AccessPoint.position);
					this.StartAction(null, ArrayExt.IndexOf<AvatarSeat>(this.SeatSet.Seats, this.SeatSet.GetRandomFreeSeat()));
				}
				this.npc.Movement.FaceDirection(this.targetSeat.AccessPoint.forward, 0f);
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00070CE7 File Offset: 0x0006EEE7
		public override void End()
		{
			base.End();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.seated)
			{
				this.EndAction();
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00070D05 File Offset: 0x0006EF05
		public override void Interrupt()
		{
			base.Interrupt();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.seated)
			{
				this.EndAction();
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00070D45 File Offset: 0x0006EF45
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00070D88 File Offset: 0x0006EF88
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.targetSeat = this.SeatSet.GetRandomFreeSeat();
				this.npc.Movement.Warp(this.targetSeat.AccessPoint.position);
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00070DD4 File Offset: 0x0006EFD4
		private bool IsAtDestination()
		{
			return !(this.targetSeat == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.targetSeat.AccessPoint.position) < 1.5f;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00070E14 File Offset: 0x0006F014
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.StartAction(null, ArrayExt.IndexOf<AvatarSeat>(this.SeatSet.Seats, this.SeatSet.GetRandomFreeSeat()));
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00070E60 File Offset: 0x0006F060
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected virtual void StartAction(NetworkConnection conn, int seatIndex)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartAction_2681120339(conn, seatIndex);
				this.RpcLogic___StartAction_2681120339(conn, seatIndex);
			}
			else
			{
				this.RpcWriter___Target_StartAction_2681120339(conn, seatIndex);
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00070EA1 File Offset: 0x0006F0A1
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00070EB8 File Offset: 0x0006F0B8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_2681120339));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_2681120339));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x00070F21 File Offset: 0x0006F121
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00070F3A File Offset: 0x0006F13A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00070F48 File Offset: 0x0006F148
		private void RpcWriter___Observers_StartAction_2681120339(NetworkConnection conn, int seatIndex)
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
			writer.WriteInt32(seatIndex, 1);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00071004 File Offset: 0x0006F204
		protected virtual void RpcLogic___StartAction_2681120339(NetworkConnection conn, int seatIndex)
		{
			if (this.seated)
			{
				return;
			}
			this.seated = true;
			if (seatIndex >= 0 && seatIndex < this.SeatSet.Seats.Length)
			{
				this.targetSeat = this.SeatSet.Seats[seatIndex];
			}
			else
			{
				this.targetSeat = null;
			}
			this.npc.Movement.SetSeat(this.targetSeat);
			if (this.onSeated != null)
			{
				this.onSeated.Invoke();
			}
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0007107C File Offset: 0x0006F27C
		private void RpcReader___Observers_StartAction_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_2681120339(null, seatIndex);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x000710C0 File Offset: 0x0006F2C0
		private void RpcWriter___Target_StartAction_2681120339(NetworkConnection conn, int seatIndex)
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
			writer.WriteInt32(seatIndex, 1);
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0007117C File Offset: 0x0006F37C
		private void RpcReader___Target_StartAction_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_2681120339(base.LocalConnection, seatIndex);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x000711B8 File Offset: 0x0006F3B8
		private void RpcWriter___Observers_EndAction_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00071261 File Offset: 0x0006F461
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.seated)
			{
				return;
			}
			this.seated = false;
			this.npc.Movement.SetSeat(null);
			if (this.onStandUp != null)
			{
				this.onStandUp.Invoke();
			}
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00071298 File Offset: 0x0006F498
		private void RpcReader___Observers_EndAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x000712C2 File Offset: 0x0006F4C2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001651 RID: 5713
		public const float DESTINATION_THRESHOLD = 1.5f;

		// Token: 0x04001652 RID: 5714
		public AvatarSeatSet SeatSet;

		// Token: 0x04001653 RID: 5715
		public bool WarpIfSkipped;

		// Token: 0x04001654 RID: 5716
		private bool seated;

		// Token: 0x04001655 RID: 5717
		private AvatarSeat targetSeat;

		// Token: 0x04001656 RID: 5718
		public UnityEvent onSeated;

		// Token: 0x04001657 RID: 5719
		public UnityEvent onStandUp;

		// Token: 0x04001658 RID: 5720
		private bool dll_Excuted;

		// Token: 0x04001659 RID: 5721
		private bool dll_Excuted;
	}
}
