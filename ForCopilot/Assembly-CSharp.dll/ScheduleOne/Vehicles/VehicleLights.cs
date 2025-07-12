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
using ScheduleOne.Vehicles.AI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000814 RID: 2068
	public class VehicleLights : NetworkBehaviour
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600383D RID: 14397 RVA: 0x000ED195 File Offset: 0x000EB395
		// (set) Token: 0x0600383E RID: 14398 RVA: 0x000ED19D File Offset: 0x000EB39D
		public bool headLightsOn
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<headLightsOn>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true, RequireOwnership = false)]
			set
			{
				this.RpcWriter___Server_set_headLightsOn_1140765316(value);
				this.RpcLogic___set_headLightsOn_1140765316(value);
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000ED1B3 File Offset: 0x000EB3B3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.VehicleLights_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000ED1C8 File Offset: 0x000EB3C8
		protected virtual void Update()
		{
			if (this.vehicle.localPlayerIsDriver && this.hasHeadLights && GameInput.GetButtonDown(GameInput.ButtonCode.ToggleLights))
			{
				this.headLightsOn = !this.headLightsOn;
				if (this.headLightsOn)
				{
					if (this.onHeadlightsOn != null)
					{
						this.onHeadlightsOn.Invoke();
						return;
					}
				}
				else if (this.onHeadlightsOff != null)
				{
					this.onHeadlightsOff.Invoke();
				}
			}
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000ED234 File Offset: 0x000EB434
		protected virtual void FixedUpdate()
		{
			this.reverseLightsOn = this.vehicle.isReversing;
			if (this.agent == null || !this.agent.AutoDriving)
			{
				this.brakeLightsOn = this.vehicle.brakesApplied;
				return;
			}
			this.brakesAppliedHistory.Add(this.vehicle.brakesApplied);
			if (this.brakesAppliedHistory.Count > 60)
			{
				this.brakesAppliedHistory.RemoveAt(0);
			}
			int num = 0;
			for (int i = 0; i < this.brakesAppliedHistory.Count; i++)
			{
				if (this.brakesAppliedHistory[i])
				{
					num++;
				}
			}
			this.brakeLightsOn = ((float)num / (float)this.brakesAppliedHistory.Count > 0.2f);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000ED2F8 File Offset: 0x000EB4F8
		protected virtual void LateUpdate()
		{
			if (this.hasHeadLights && this.headLightsOn != this.headLightsApplied)
			{
				if (this.headLightsOn)
				{
					this.headLightsApplied = true;
					for (int i = 0; i < this.headLightMeshes.Length; i++)
					{
						this.headLightMeshes[i].material = this.headlightMat_On;
					}
					for (int j = 0; j < this.headLightSources.Length; j++)
					{
						this.headLightSources[j].Enabled = true;
					}
				}
				else
				{
					this.headLightsApplied = false;
					for (int k = 0; k < this.headLightMeshes.Length; k++)
					{
						this.headLightMeshes[k].material = this.headLightMat_Off;
					}
					for (int l = 0; l < this.headLightSources.Length; l++)
					{
						this.headLightSources[l].Enabled = false;
					}
				}
			}
			if (this.hasBrakeLights && this.brakeLightsOn != this.brakeLightsApplied)
			{
				if (this.brakeLightsOn)
				{
					this.brakeLightsApplied = true;
					for (int m = 0; m < this.brakeLightMeshes.Length; m++)
					{
						this.brakeLightMeshes[m].material = this.brakeLightMat_On;
					}
					if (this.vehicle.localPlayerIsInVehicle)
					{
						for (int n = 0; n < this.brakeLightSources.Length; n++)
						{
							this.brakeLightSources[n].enabled = true;
						}
					}
				}
				else
				{
					this.brakeLightsApplied = false;
					for (int num = 0; num < this.brakeLightMeshes.Length; num++)
					{
						this.brakeLightMeshes[num].material = this.brakeLightMat_Off;
					}
					for (int num2 = 0; num2 < this.brakeLightSources.Length; num2++)
					{
						this.brakeLightSources[num2].enabled = false;
					}
				}
			}
			if (this.hasReverseLights && this.reverseLightsOn != this.reverseLightsApplied)
			{
				if (this.reverseLightsOn)
				{
					this.reverseLightsApplied = true;
					for (int num3 = 0; num3 < this.reverseLightMeshes.Length; num3++)
					{
						this.reverseLightMeshes[num3].material = this.reverseLightMat_On;
					}
					if (this.vehicle.localPlayerIsInVehicle)
					{
						for (int num4 = 0; num4 < this.reverseLightSources.Length; num4++)
						{
							this.reverseLightSources[num4].enabled = true;
						}
						return;
					}
				}
				else
				{
					this.reverseLightsApplied = false;
					for (int num5 = 0; num5 < this.reverseLightMeshes.Length; num5++)
					{
						this.reverseLightMeshes[num5].material = this.reverseLightMat_Off;
					}
					for (int num6 = 0; num6 < this.reverseLightSources.Length; num6++)
					{
						this.reverseLightSources[num6].enabled = false;
					}
				}
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000ED5B0 File Offset: 0x000EB7B0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<headLightsOn>k__BackingField = new SyncVar<bool>(this, 0U, 1, 0, 0.25f, 1, this.<headLightsOn>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_headLightsOn_1140765316));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.VehicleLights));
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000ED622 File Offset: 0x000EB822
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<headLightsOn>k__BackingField.SetRegistered();
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000ED640 File Offset: 0x000EB840
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000ED650 File Offset: 0x000EB850
		private void RpcWriter___Server_set_headLightsOn_1140765316(bool value)
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
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000ED6F7 File Offset: 0x000EB8F7
		public void RpcLogic___set_headLightsOn_1140765316(bool value)
		{
			this.sync___set_value_<headLightsOn>k__BackingField(value, true);
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000ED704 File Offset: 0x000EB904
		private void RpcReader___Server_set_headLightsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_headLightsOn_1140765316(value);
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600384A RID: 14410 RVA: 0x000ED742 File Offset: 0x000EB942
		// (set) Token: 0x0600384B RID: 14411 RVA: 0x000ED74A File Offset: 0x000EB94A
		public bool SyncAccessor_<headLightsOn>k__BackingField
		{
			get
			{
				return this.<headLightsOn>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<headLightsOn>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<headLightsOn>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000ED788 File Offset: 0x000EB988
		public override bool VehicleLights(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<headLightsOn>k__BackingField(this.syncVar___<headLightsOn>k__BackingField.GetValue(true), true);
				return true;
			}
			bool value = PooledReader0.ReadBoolean();
			this.sync___set_value_<headLightsOn>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000ED7DA File Offset: 0x000EB9DA
		protected virtual void dll()
		{
			this.agent = base.GetComponent<VehicleAgent>();
		}

		// Token: 0x04002806 RID: 10246
		public LandVehicle vehicle;

		// Token: 0x04002807 RID: 10247
		[Header("Headlights")]
		public bool hasHeadLights;

		// Token: 0x04002808 RID: 10248
		public MeshRenderer[] headLightMeshes;

		// Token: 0x04002809 RID: 10249
		public OptimizedLight[] headLightSources;

		// Token: 0x0400280A RID: 10250
		public Material headlightMat_On;

		// Token: 0x0400280B RID: 10251
		public Material headLightMat_Off;

		// Token: 0x0400280D RID: 10253
		protected bool headLightsApplied;

		// Token: 0x0400280E RID: 10254
		[Header("Brake lights")]
		public bool hasBrakeLights;

		// Token: 0x0400280F RID: 10255
		public MeshRenderer[] brakeLightMeshes;

		// Token: 0x04002810 RID: 10256
		public Light[] brakeLightSources;

		// Token: 0x04002811 RID: 10257
		public Material brakeLightMat_On;

		// Token: 0x04002812 RID: 10258
		public Material brakeLightMat_Off;

		// Token: 0x04002813 RID: 10259
		public Material brakeLightMat_Ambient;

		// Token: 0x04002814 RID: 10260
		protected bool brakeLightsOn;

		// Token: 0x04002815 RID: 10261
		protected bool brakeLightsApplied = true;

		// Token: 0x04002816 RID: 10262
		[Header("Reverse lights")]
		public bool hasReverseLights;

		// Token: 0x04002817 RID: 10263
		public MeshRenderer[] reverseLightMeshes;

		// Token: 0x04002818 RID: 10264
		public Light[] reverseLightSources;

		// Token: 0x04002819 RID: 10265
		public Material reverseLightMat_On;

		// Token: 0x0400281A RID: 10266
		public Material reverseLightMat_Off;

		// Token: 0x0400281B RID: 10267
		protected bool reverseLightsOn;

		// Token: 0x0400281C RID: 10268
		protected bool reverseLightsApplied = true;

		// Token: 0x0400281D RID: 10269
		public UnityEvent onHeadlightsOn;

		// Token: 0x0400281E RID: 10270
		public UnityEvent onHeadlightsOff;

		// Token: 0x0400281F RID: 10271
		private List<bool> brakesAppliedHistory = new List<bool>();

		// Token: 0x04002820 RID: 10272
		private VehicleAgent agent;

		// Token: 0x04002821 RID: 10273
		public SyncVar<bool> syncVar___<headLightsOn>k__BackingField;

		// Token: 0x04002822 RID: 10274
		private bool dll_Excuted;

		// Token: 0x04002823 RID: 10275
		private bool dll_Excuted;
	}
}
