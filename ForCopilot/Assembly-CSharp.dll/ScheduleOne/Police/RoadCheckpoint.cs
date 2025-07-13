using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Misc;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Police
{
	// Token: 0x0200034E RID: 846
	public class RoadCheckpoint : NetworkBehaviour
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x00051B42 File Offset: 0x0004FD42
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x00051B4A File Offset: 0x0004FD4A
		public RoadCheckpoint.ECheckpointState ActivationState { get; protected set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x00051B53 File Offset: 0x0004FD53
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x00051B5B File Offset: 0x0004FD5B
		public bool Gate1Open
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Gate1Open>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<Gate1Open>k__BackingField(value, true);
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x00051B65 File Offset: 0x0004FD65
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x00051B6D File Offset: 0x0004FD6D
		public bool Gate2Open
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Gate2Open>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<Gate2Open>k__BackingField(value, true);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00051B78 File Offset: 0x0004FD78
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Police.RoadCheckpoint_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00051B98 File Offset: 0x0004FD98
		protected virtual void Update()
		{
			if (this.ActivationState != RoadCheckpoint.ECheckpointState.Disabled)
			{
				this.VehicleObstacle1.gameObject.SetActive(!this.Gate1Open);
				this.VehicleObstacle2.gameObject.SetActive(!this.Gate2Open);
				this.Stopper1.isActive = !this.Gate1Open;
				this.Stopper2.isActive = !this.Gate2Open;
			}
			if (this.ActivationState != this.appliedState)
			{
				this.ApplyState();
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.OpenForNPCs)
			{
				if (this.NPCVehicleDetectionArea1.closestVehicle != null && this.NPCVehicleDetectionArea1.closestVehicle.OccupantNPCs[0] != null)
				{
					if (!this.Gate1Open)
					{
						this.SetGate1Open(true);
					}
					if (!this.Gate2Open)
					{
						this.SetGate2Open(true);
					}
				}
				if (this.NPCVehicleDetectionArea2.closestVehicle != null && this.NPCVehicleDetectionArea2.closestVehicle.OccupantNPCs[0] != null)
				{
					if (!this.Gate1Open)
					{
						this.SetGate1Open(true);
					}
					if (!this.Gate2Open)
					{
						this.SetGate2Open(true);
					}
				}
			}
			if (this.ActivationState != RoadCheckpoint.ECheckpointState.Disabled)
			{
				if (this.Gate1Open)
				{
					this.timeSinceGate1Open += Time.deltaTime;
					if (this.ImmediateVehicleDetector.vehicles.Count > 0)
					{
						this.vehicleDetectedSinceGate1Open = true;
					}
					if (this.timeSinceGate1Open > 15f || (this.vehicleDetectedSinceGate1Open && this.ImmediateVehicleDetector.vehicles.Count == 0))
					{
						this.SetGate1Open(false);
					}
				}
				else
				{
					this.timeSinceGate1Open = 0f;
					this.vehicleDetectedSinceGate1Open = false;
				}
				if (!this.Gate2Open)
				{
					this.timeSinceGate2Open = 0f;
					this.vehicleDetectedSinceGate2Open = false;
					return;
				}
				this.timeSinceGate2Open += Time.deltaTime;
				if (this.ImmediateVehicleDetector.vehicles.Count > 0)
				{
					this.vehicleDetectedSinceGate2Open = true;
				}
				if (this.timeSinceGate2Open > 15f || (this.vehicleDetectedSinceGate2Open && this.ImmediateVehicleDetector.vehicles.Count == 0))
				{
					this.SetGate2Open(false);
					return;
				}
			}
			else
			{
				this.timeSinceGate1Open = 0f;
				this.vehicleDetectedSinceGate1Open = false;
				this.timeSinceGate2Open = 0f;
				this.vehicleDetectedSinceGate2Open = false;
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00051DDF File Offset: 0x0004FFDF
		protected virtual void ApplyState()
		{
			this.appliedState = this.ActivationState;
			if (this.ActivationState == RoadCheckpoint.ECheckpointState.Disabled)
			{
				this.container.SetActive(false);
				return;
			}
			this.container.SetActive(true);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00051E0E File Offset: 0x0005000E
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Enable(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_328543758(conn);
				this.RpcLogic___Enable_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_328543758(conn);
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00051E38 File Offset: 0x00050038
		[ObserversRpc(RunLocally = true)]
		public void Disable()
		{
			this.RpcWriter___Observers_Disable_2166136261();
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00051E51 File Offset: 0x00050051
		public void SetGate1Open(bool o)
		{
			this.Gate1Open = o;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00051E5A File Offset: 0x0005005A
		public void SetGate2Open(bool o)
		{
			this.Gate2Open = o;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00051E64 File Offset: 0x00050064
		private void ResetTrafficCones()
		{
			if (this.trafficConeOriginalTransforms.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.TrafficCones.Length; i++)
			{
				this.TrafficCones[i].transform.position = this.trafficConeOriginalTransforms[this.TrafficCones[i]].Item1;
				this.TrafficCones[i].transform.rotation = this.trafficConeOriginalTransforms[this.TrafficCones[i]].Item2;
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00051EE6 File Offset: 0x000500E6
		public void PlayerDetected(Player player)
		{
			if (this.onPlayerWalkThrough != null)
			{
				this.onPlayerWalkThrough.Invoke(player);
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00051F24 File Offset: 0x00050124
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Gate2Open>k__BackingField = new SyncVar<bool>(this, 1U, 0, 0, 0.25f, 0, this.<Gate2Open>k__BackingField);
			this.syncVar___<Gate1Open>k__BackingField = new SyncVar<bool>(this, 0U, 0, 0, 0.25f, 0, this.<Gate1Open>k__BackingField);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_Enable_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_2166136261));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Police.RoadCheckpoint));
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00051FEF File Offset: 0x000501EF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Gate2Open>k__BackingField.SetRegistered();
			this.syncVar___<Gate1Open>k__BackingField.SetRegistered();
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00052018 File Offset: 0x00050218
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00052028 File Offset: 0x00050228
		private void RpcWriter___Observers_Enable_328543758(NetworkConnection conn)
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

		// Token: 0x060012E7 RID: 4839 RVA: 0x000520D1 File Offset: 0x000502D1
		public void RpcLogic___Enable_328543758(NetworkConnection conn)
		{
			this.ResetTrafficCones();
			this.ActivationState = RoadCheckpoint.ECheckpointState.Enabled;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x000520E0 File Offset: 0x000502E0
		private void RpcReader___Observers_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(null);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0005210C File Offset: 0x0005030C
		private void RpcWriter___Target_Enable_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000521B4 File Offset: 0x000503B4
		private void RpcReader___Target_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(base.LocalConnection);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x000521DC File Offset: 0x000503DC
		private void RpcWriter___Observers_Disable_2166136261()
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

		// Token: 0x060012EC RID: 4844 RVA: 0x00052288 File Offset: 0x00050488
		public void RpcLogic___Disable_2166136261()
		{
			this.ActivationState = RoadCheckpoint.ECheckpointState.Disabled;
			if (InstanceFinder.IsServer)
			{
				for (int i = 0; i < this.AssignedNPCs.Count; i++)
				{
					(this.AssignedNPCs[i] as PoliceOfficer).UnassignFromCheckpoint();
				}
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000522D0 File Offset: 0x000504D0
		private void RpcReader___Observers_Disable_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x000522FA File Offset: 0x000504FA
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x00052302 File Offset: 0x00050502
		public bool SyncAccessor_<Gate1Open>k__BackingField
		{
			get
			{
				return this.<Gate1Open>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Gate1Open>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Gate1Open>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00052340 File Offset: 0x00050540
		public override bool RoadCheckpoint(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Gate2Open>k__BackingField(this.syncVar___<Gate2Open>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_<Gate2Open>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Gate1Open>k__BackingField(this.syncVar___<Gate1Open>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value2 = PooledReader0.ReadBoolean();
				this.sync___set_value_<Gate1Open>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x000523D6 File Offset: 0x000505D6
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x000523DE File Offset: 0x000505DE
		public bool SyncAccessor_<Gate2Open>k__BackingField
		{
			get
			{
				return this.<Gate2Open>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Gate2Open>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Gate2Open>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0005241C File Offset: 0x0005061C
		protected virtual void dll()
		{
			if (this.EnabledOnStart)
			{
				this.ActivationState = RoadCheckpoint.ECheckpointState.Enabled;
			}
			this.ApplyState();
			if (this.trafficConeOriginalTransforms.Count == 0)
			{
				for (int i = 0; i < this.TrafficCones.Length; i++)
				{
					this.trafficConeOriginalTransforms.Add(this.TrafficCones[i], new Tuple<Vector3, Quaternion>(this.TrafficCones[i].transform.position, this.TrafficCones[i].transform.rotation));
				}
			}
		}

		// Token: 0x040011F5 RID: 4597
		public const float MAX_TIME_OPEN = 15f;

		// Token: 0x040011F7 RID: 4599
		protected RoadCheckpoint.ECheckpointState appliedState;

		// Token: 0x040011FA RID: 4602
		public List<NPC> AssignedNPCs = new List<NPC>();

		// Token: 0x040011FB RID: 4603
		[Header("Settings")]
		public EStealthLevel MaxStealthLevel;

		// Token: 0x040011FC RID: 4604
		public bool OpenForNPCs = true;

		// Token: 0x040011FD RID: 4605
		public bool EnabledOnStart;

		// Token: 0x040011FE RID: 4606
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x040011FF RID: 4607
		public CarStopper Stopper1;

		// Token: 0x04001200 RID: 4608
		public CarStopper Stopper2;

		// Token: 0x04001201 RID: 4609
		public VehicleDetector SearchArea1;

		// Token: 0x04001202 RID: 4610
		public VehicleDetector SearchArea2;

		// Token: 0x04001203 RID: 4611
		public VehicleObstacle VehicleObstacle1;

		// Token: 0x04001204 RID: 4612
		public VehicleObstacle VehicleObstacle2;

		// Token: 0x04001205 RID: 4613
		public VehicleDetector NPCVehicleDetectionArea1;

		// Token: 0x04001206 RID: 4614
		public VehicleDetector NPCVehicleDetectionArea2;

		// Token: 0x04001207 RID: 4615
		public VehicleDetector ImmediateVehicleDetector;

		// Token: 0x04001208 RID: 4616
		public Rigidbody[] TrafficCones;

		// Token: 0x04001209 RID: 4617
		public Transform[] StandPoints;

		// Token: 0x0400120A RID: 4618
		protected Dictionary<Rigidbody, Tuple<Vector3, Quaternion>> trafficConeOriginalTransforms = new Dictionary<Rigidbody, Tuple<Vector3, Quaternion>>();

		// Token: 0x0400120B RID: 4619
		private float timeSinceGate1Open;

		// Token: 0x0400120C RID: 4620
		private bool vehicleDetectedSinceGate1Open;

		// Token: 0x0400120D RID: 4621
		private float timeSinceGate2Open;

		// Token: 0x0400120E RID: 4622
		private bool vehicleDetectedSinceGate2Open;

		// Token: 0x0400120F RID: 4623
		public UnityEvent<Player> onPlayerWalkThrough;

		// Token: 0x04001210 RID: 4624
		public SyncVar<bool> syncVar___<Gate1Open>k__BackingField;

		// Token: 0x04001211 RID: 4625
		public SyncVar<bool> syncVar___<Gate2Open>k__BackingField;

		// Token: 0x04001212 RID: 4626
		private bool dll_Excuted;

		// Token: 0x04001213 RID: 4627
		private bool dll_Excuted;

		// Token: 0x0200034F RID: 847
		public enum ECheckpointState
		{
			// Token: 0x04001215 RID: 4629
			Disabled,
			// Token: 0x04001216 RID: 4630
			Enabled
		}
	}
}
