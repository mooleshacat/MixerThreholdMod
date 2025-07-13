using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004C1 RID: 1217
	public class NPCSignal_WalkToLocation : NPCSignal
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00073A42 File Offset: 0x00071C42
		public new string ActionName
		{
			get
			{
				return "Walk to location";
			}
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00073A49 File Offset: 0x00071C49
		public override string GetName()
		{
			return this.ActionName + " (" + this.Destination.name + ")";
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00073A6B File Offset: 0x00071C6B
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00073A85 File Offset: 0x00071C85
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
			if (!this.npc.Movement.IsMoving && !this.IsAtDestination())
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00072D48 File Offset: 0x00070F48
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00073AB9 File Offset: 0x00071CB9
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00072DA4 File Offset: 0x00070FA4
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00073AE3 File Offset: 0x00071CE3
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00073B0E File Offset: 0x00071D0E
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00073B38 File Offset: 0x00071D38
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				Debug.LogWarning("NPC walk to location not successful");
				return;
			}
			this.ReachedDestination();
			this.End();
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00073B65 File Offset: 0x00071D65
		[ObserversRpc]
		private void ReachedDestination()
		{
			this.RpcWriter___Observers_ReachedDestination_2166136261();
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00073B87 File Offset: 0x00071D87
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_ReachedDestination_2166136261));
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00073BB7 File Offset: 0x00071DB7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00073BD0 File Offset: 0x00071DD0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00073BE0 File Offset: 0x00071DE0
		private void RpcWriter___Observers_ReachedDestination_2166136261()
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

		// Token: 0x06001AAD RID: 6829 RVA: 0x00073C89 File Offset: 0x00071E89
		private void RpcLogic___ReachedDestination_2166136261()
		{
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00073CB4 File Offset: 0x00071EB4
		private void RpcReader___Observers_ReachedDestination_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReachedDestination_2166136261();
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00073CD4 File Offset: 0x00071ED4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A4 RID: 5796
		public Transform Destination;

		// Token: 0x040016A5 RID: 5797
		public bool FaceDestinationDir = true;

		// Token: 0x040016A6 RID: 5798
		public float DestinationThreshold = 1f;

		// Token: 0x040016A7 RID: 5799
		public bool WarpIfSkipped;

		// Token: 0x040016A8 RID: 5800
		private bool dll_Excuted;

		// Token: 0x040016A9 RID: 5801
		private bool dll_Excuted;
	}
}
