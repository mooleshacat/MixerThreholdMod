using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000769 RID: 1897
	public abstract class OptionListFeature : Feature
	{
		// Token: 0x06003313 RID: 13075 RVA: 0x000D4015 File Offset: 0x000D2215
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Construction.Features.OptionListFeature_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000D402C File Offset: 0x000D222C
		public override FI_Base CreateInterface(Transform parent)
		{
			FI_OptionList component = UnityEngine.Object.Instantiate<GameObject>(this.featureInterfacePrefab, parent).GetComponent<FI_OptionList>();
			component.Initialize(this, this.GetOptions());
			component.onSelectionChanged.AddListener(new UnityAction<int>(this.SelectOption));
			component.onSelectionPurchased.AddListener(new UnityAction<int>(this.PurchaseOption));
			return component;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000D4087 File Offset: 0x000D2287
		public override void Default()
		{
			this.PurchaseOption(this.defaultOptionIndex);
		}

		// Token: 0x06003316 RID: 13078
		protected abstract List<FI_OptionList.Option> GetOptions();

		// Token: 0x06003317 RID: 13079 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void SelectOption(int optionIndex)
		{
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000D4095 File Offset: 0x000D2295
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(int colorIndex)
		{
			this.RpcWriter___Server_SetData_3316948804(colorIndex);
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000D40AB File Offset: 0x000D22AB
		private void ReceiveData()
		{
			this.SelectOption(this.SyncAccessor_ownedOptionIndex);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000D40B9 File Offset: 0x000D22B9
		public virtual void PurchaseOption(int optionIndex)
		{
			this.SetData(optionIndex);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000D40CC File Offset: 0x000D22CC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___ownedOptionIndex = new SyncVar<int>(this, 0U, 0, 0, -1f, 0, this.ownedOptionIndex);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetData_3316948804));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Construction.Features.OptionListFeature));
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000D4144 File Offset: 0x000D2344
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___ownedOptionIndex.SetRegistered();
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000D4168 File Offset: 0x000D2368
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000D4178 File Offset: 0x000D2378
		private void RpcWriter___Server_SetData_3316948804(int colorIndex)
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
			writer.WriteInt32(colorIndex, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000D4224 File Offset: 0x000D2424
		protected virtual void RpcLogic___SetData_3316948804(int colorIndex)
		{
			if (!base.IsSpawned)
			{
				this.SelectOption(colorIndex);
				return;
			}
			this.sync___set_value_ownedOptionIndex(colorIndex, true);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000D4240 File Offset: 0x000D2440
		private void RpcReader___Server_SetData_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int colorIndex = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06003322 RID: 13090 RVA: 0x000D4283 File Offset: 0x000D2483
		// (set) Token: 0x06003323 RID: 13091 RVA: 0x000D428B File Offset: 0x000D248B
		public int SyncAccessor_ownedOptionIndex
		{
			get
			{
				return this.ownedOptionIndex;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.ownedOptionIndex = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___ownedOptionIndex.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000D42C8 File Offset: 0x000D24C8
		public override bool OptionListFeature(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_ownedOptionIndex(this.syncVar___ownedOptionIndex.GetValue(true), true);
				return true;
			}
			int value = PooledReader0.ReadInt32(1);
			this.sync___set_value_ownedOptionIndex(value, Boolean2);
			return true;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000D431F File Offset: 0x000D251F
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x04002411 RID: 9233
		[Header("Option list feature settings")]
		public int defaultOptionIndex;

		// Token: 0x04002412 RID: 9234
		[SyncVar]
		public int ownedOptionIndex;

		// Token: 0x04002413 RID: 9235
		public SyncVar<int> syncVar___ownedOptionIndex;

		// Token: 0x04002414 RID: 9236
		private bool dll_Excuted;

		// Token: 0x04002415 RID: 9237
		private bool dll_Excuted;
	}
}
