using System;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007FC RID: 2044
	public class Forklift : LandVehicle
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06003714 RID: 14100 RVA: 0x000E7C68 File Offset: 0x000E5E68
		// (set) Token: 0x06003715 RID: 14101 RVA: 0x000E7C70 File Offset: 0x000E5E70
		public float targetForkHeight
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<targetForkHeight>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_targetForkHeight_431000436(value);
				this.RpcLogic___set_targetForkHeight_431000436(value);
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003716 RID: 14102 RVA: 0x000E7C86 File Offset: 0x000E5E86
		// (set) Token: 0x06003717 RID: 14103 RVA: 0x000E7C8E File Offset: 0x000E5E8E
		public float actualForkHeight
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<actualForkHeight>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_actualForkHeight_431000436(value);
				this.RpcLogic___set_actualForkHeight_431000436(value);
			}
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000E7CA4 File Offset: 0x000E5EA4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.Forklift_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000E7CC4 File Offset: 0x000E5EC4
		protected override void Update()
		{
			base.Update();
			if (base.localPlayerIsDriver)
			{
				this.targetForkHeight = this.lastFrameTargetForkHeight;
				int num = 0;
				if (Input.GetKey(KeyCode.UpArrow))
				{
					num++;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					num--;
				}
				this.targetForkHeight = Mathf.Clamp(this.targetForkHeight + (float)num * Time.deltaTime * this.liftMoveRate, 0f, 1f);
			}
			this.lastFrameTargetForkHeight = this.targetForkHeight;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000E7D44 File Offset: 0x000E5F44
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				this.forkRb.isKinematic = false;
				this.joint.targetPosition = new Vector3(0f, Mathf.Lerp(this.lift_MinY, this.lift_MaxY, this.targetForkHeight), 0f);
				Vector3 vector = this.forkRb.transform.position - base.transform.TransformPoint(this.joint.connectedAnchor);
				vector = base.transform.InverseTransformVector(vector);
				this.actualForkHeight = 1f - Mathf.InverseLerp(this.lift_MinY, this.lift_MaxY, vector.y);
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000E7E10 File Offset: 0x000E6010
		protected new virtual void LateUpdate()
		{
			if (!base.localPlayerIsDriver && (!InstanceFinder.IsHost || base.CurrentPlayerOccupancy > 0))
			{
				this.forkRb.isKinematic = true;
				this.forkRb.transform.position = base.transform.TransformPoint(this.joint.connectedAnchor + new Vector3(0f, -Mathf.Lerp(this.lift_MinY, this.lift_MaxY, this.actualForkHeight), 0f));
				this.forkRb.transform.rotation = base.transform.rotation;
			}
			this.steeringWheel.localEulerAngles = new Vector3(0f, base.SyncAccessor_currentSteerAngle * this.steeringWheelAngleMultiplier, 0f);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000E7EF8 File Offset: 0x000E60F8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<actualForkHeight>k__BackingField = new SyncVar<float>(this, 4U, 0, 0, 0.04f, 1, this.<actualForkHeight>k__BackingField);
			this.syncVar___<targetForkHeight>k__BackingField = new SyncVar<float>(this, 3U, 0, 0, -1f, 1, this.<targetForkHeight>k__BackingField);
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_set_targetForkHeight_431000436));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_set_actualForkHeight_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.Forklift));
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000E7FB2 File Offset: 0x000E61B2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<actualForkHeight>k__BackingField.SetRegistered();
			this.syncVar___<targetForkHeight>k__BackingField.SetRegistered();
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000E7FE1 File Offset: 0x000E61E1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000E7FF0 File Offset: 0x000E61F0
		private void RpcWriter___Server_set_targetForkHeight_431000436(float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(value, 0);
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000E80F6 File Offset: 0x000E62F6
		protected void RpcLogic___set_targetForkHeight_431000436(float value)
		{
			this.sync___set_value_<targetForkHeight>k__BackingField(value, true);
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000E8100 File Offset: 0x000E6300
		private void RpcReader___Server_set_targetForkHeight_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_targetForkHeight_431000436(value);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000E8154 File Offset: 0x000E6354
		private void RpcWriter___Server_set_actualForkHeight_431000436(float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(value, 0);
			base.SendServerRpc(18U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000E825A File Offset: 0x000E645A
		protected void RpcLogic___set_actualForkHeight_431000436(float value)
		{
			this.sync___set_value_<actualForkHeight>k__BackingField(value, true);
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000E8264 File Offset: 0x000E6464
		private void RpcReader___Server_set_actualForkHeight_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_actualForkHeight_431000436(value);
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x000E82B8 File Offset: 0x000E64B8
		// (set) Token: 0x06003727 RID: 14119 RVA: 0x000E82C0 File Offset: 0x000E64C0
		public float SyncAccessor_<targetForkHeight>k__BackingField
		{
			get
			{
				return this.<targetForkHeight>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<targetForkHeight>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<targetForkHeight>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000E82FC File Offset: 0x000E64FC
		public override bool Forklift(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<actualForkHeight>k__BackingField(this.syncVar___<actualForkHeight>k__BackingField.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(0);
				this.sync___set_value_<actualForkHeight>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 3U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<targetForkHeight>k__BackingField(this.syncVar___<targetForkHeight>k__BackingField.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(0);
				this.sync___set_value_<targetForkHeight>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003729 RID: 14121 RVA: 0x000E839C File Offset: 0x000E659C
		// (set) Token: 0x0600372A RID: 14122 RVA: 0x000E83A4 File Offset: 0x000E65A4
		public float SyncAccessor_<actualForkHeight>k__BackingField
		{
			get
			{
				return this.<actualForkHeight>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<actualForkHeight>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<actualForkHeight>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x000E83E0 File Offset: 0x000E65E0
		protected override void dll()
		{
			base.Awake();
			Vector3 position = this.forkRb.transform.position;
			Quaternion rotation = this.forkRb.transform.rotation;
			this.forkRb.transform.SetParent(null);
			this.forkRb.transform.position = position;
			this.forkRb.transform.rotation = rotation;
		}

		// Token: 0x04002748 RID: 10056
		[Header("Forklift References")]
		[SerializeField]
		protected Transform steeringWheel;

		// Token: 0x04002749 RID: 10057
		[SerializeField]
		protected Rigidbody forkRb;

		// Token: 0x0400274A RID: 10058
		[SerializeField]
		protected ConfigurableJoint joint;

		// Token: 0x0400274B RID: 10059
		[Header("Forklift settings")]
		[SerializeField]
		protected float steeringWheelAngleMultiplier = 2f;

		// Token: 0x0400274C RID: 10060
		[SerializeField]
		protected float lift_MinY;

		// Token: 0x0400274D RID: 10061
		[SerializeField]
		protected float lift_MaxY;

		// Token: 0x0400274E RID: 10062
		[SerializeField]
		protected float liftMoveRate = 0.5f;

		// Token: 0x04002750 RID: 10064
		private float lastFrameTargetForkHeight;

		// Token: 0x04002752 RID: 10066
		public SyncVar<float> syncVar___<targetForkHeight>k__BackingField;

		// Token: 0x04002753 RID: 10067
		public SyncVar<float> syncVar___<actualForkHeight>k__BackingField;

		// Token: 0x04002754 RID: 10068
		private bool dll_Excuted;

		// Token: 0x04002755 RID: 10069
		private bool dll_Excuted;
	}
}
