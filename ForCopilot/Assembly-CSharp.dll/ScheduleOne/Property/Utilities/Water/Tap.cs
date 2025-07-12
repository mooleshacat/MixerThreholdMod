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
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property.Utilities.Water
{
	// Token: 0x0200085F RID: 2143
	public class Tap : NetworkBehaviour, IUsable
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003A23 RID: 14883 RVA: 0x000F6029 File Offset: 0x000F4229
		// (set) Token: 0x06003A24 RID: 14884 RVA: 0x000F6031 File Offset: 0x000F4231
		public bool IsHeldOpen
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<IsHeldOpen>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<IsHeldOpen>k__BackingField(value, true);
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003A25 RID: 14885 RVA: 0x000F603B File Offset: 0x000F423B
		public float ActualFlowRate
		{
			get
			{
				return 6f * this.tapFlow;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x000F6049 File Offset: 0x000F4249
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x000F6051 File Offset: 0x000F4251
		public NetworkObject NPCUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<NPCUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<NPCUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x000F605B File Offset: 0x000F425B
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x000F6063 File Offset: 0x000F4263
		public NetworkObject PlayerUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x000F606D File Offset: 0x000F426D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Utilities.Water.Tap_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x000F6084 File Offset: 0x000F4284
		protected virtual void LateUpdate()
		{
			float num = 2f;
			if (this.IsHeldOpen)
			{
				this.tapFlow = Mathf.Clamp(this.tapFlow + Time.deltaTime * num, 0f, 1f);
			}
			else
			{
				this.tapFlow = Mathf.Clamp(this.tapFlow - Time.deltaTime * num, 0f, 1f);
			}
			this.UpdateTapVisuals();
			this.UpdateWaterSound();
			if (!this.intObjSetThisFrame)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			this.intObjSetThisFrame = false;
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x000F610E File Offset: 0x000F430E
		public void SetInteractableObject(string message, InteractableObject.EInteractableState state)
		{
			this.intObjSetThisFrame = true;
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(state);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x000F6130 File Offset: 0x000F4330
		protected void UpdateTapVisuals()
		{
			this.Handle.transform.localEulerAngles = new Vector3(0f, -this.tapFlow * 360f, 0f);
			if (this.tapFlow > 0f)
			{
				this.WaterParticles.main.startSize = new ParticleSystem.MinMaxCurve(0.075f * this.tapFlow, 0.1f * this.tapFlow);
				if (!this.WaterParticles.isPlaying)
				{
					this.WaterParticles.Play();
					return;
				}
			}
			else if (this.WaterParticles.isPlaying)
			{
				this.WaterParticles.Stop();
			}
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000F61D8 File Offset: 0x000F43D8
		protected void UpdateWaterSound()
		{
			if (this.tapFlow > 0.01f)
			{
				this.WaterRunningSound.VolumeMultiplier = this.tapFlow;
				if (!this.WaterRunningSound.isPlaying)
				{
					this.WaterRunningSound.Play();
					return;
				}
			}
			else if (this.WaterRunningSound.isPlaying)
			{
				this.WaterRunningSound.Stop();
			}
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000F6234 File Offset: 0x000F4434
		public void Hovered()
		{
			if (this.CanInteract())
			{
				this.IntObj.SetMessage("Fill watering can");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000F6267 File Offset: 0x000F4467
		public void Interacted()
		{
			if (!this.CanInteract())
			{
				return;
			}
			HotbarSlot equippedSlot = PlayerSingleton<PlayerInventory>.Instance.equippedSlot;
			new FillWateringCan(this, ((equippedSlot != null) ? equippedSlot.ItemInstance : null) as WateringCanInstance);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x000F6294 File Offset: 0x000F4494
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x000F62B5 File Offset: 0x000F44B5
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x000F62CB File Offset: 0x000F44CB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetHeldOpen(bool open)
		{
			this.RpcWriter___Server_SetHeldOpen_1140765316(open);
			this.RpcLogic___SetHeldOpen_1140765316(open);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x000F62E4 File Offset: 0x000F44E4
		protected virtual bool CanInteract()
		{
			HotbarSlot equippedSlot = PlayerSingleton<PlayerInventory>.Instance.equippedSlot;
			ItemInstance itemInstance = (equippedSlot != null) ? equippedSlot.ItemInstance : null;
			if (itemInstance == null)
			{
				return false;
			}
			WateringCanInstance wateringCanInstance = itemInstance as WateringCanInstance;
			return wateringCanInstance != null && wateringCanInstance.CurrentFillAmount < 15f && !((IUsable)this).IsInUse;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x000F6333 File Offset: 0x000F4533
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendWateringCanModel(string ID)
		{
			this.RpcWriter___Server_SendWateringCanModel_3615296227(ID);
			this.RpcLogic___SendWateringCanModel_3615296227(ID);
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x000F6349 File Offset: 0x000F4549
		[ObserversRpc(RunLocally = true)]
		private void CreateWateringCanModel(string ID)
		{
			this.RpcWriter___Observers_CreateWateringCanModel_3615296227(ID);
			this.RpcLogic___CreateWateringCanModel_3615296227(ID);
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x000F635F File Offset: 0x000F455F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendClearWateringCanModelModel()
		{
			this.RpcWriter___Server_SendClearWateringCanModelModel_2166136261();
			this.RpcLogic___SendClearWateringCanModelModel_2166136261();
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x000F636D File Offset: 0x000F456D
		[ObserversRpc(RunLocally = true)]
		private void ClearWateringCanModel()
		{
			this.RpcWriter___Observers_ClearWateringCanModel_2166136261();
			this.RpcLogic___ClearWateringCanModel_2166136261();
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x000F637C File Offset: 0x000F457C
		public GameObject CreateWateringCanModel_Local(string ID, bool force = false)
		{
			if (this.wateringCanModel != null && !force)
			{
				return null;
			}
			WateringCanDefinition wateringCanDefinition = Registry.GetItem(ID) as WateringCanDefinition;
			if (wateringCanDefinition == null)
			{
				Console.LogWarning("CreateWateringCanModel_Local: WateringCanDefinition not found", null);
				return null;
			}
			this.wateringCanModel = UnityEngine.Object.Instantiate<GameObject>(wateringCanDefinition.FunctionalWateringCanPrefab, base.transform);
			this.wateringCanModel.transform.position = this.WateringCamPos.position;
			this.wateringCanModel.GetComponent<Rigidbody>().position = this.WateringCamPos.position;
			this.wateringCanModel.transform.rotation = this.WateringCamPos.rotation;
			this.wateringCanModel.GetComponent<Rigidbody>().rotation = this.WateringCamPos.rotation;
			this.wateringCanModel.GetComponent<FunctionalWateringCan>().enabled = false;
			return this.wateringCanModel;
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x000F6458 File Offset: 0x000F4658
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			this.syncVar___<IsHeldOpen>k__BackingField = new SyncVar<bool>(this, 0U, 1, 0, -1f, 0, this.<IsHeldOpen>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetHeldOpen_1140765316));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendWateringCanModel_3615296227));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_CreateWateringCanModel_3615296227));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendClearWateringCanModelModel_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ClearWateringCanModel_2166136261));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Property.Utilities.Water.Tap));
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x000F65AA File Offset: 0x000F47AA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
			this.syncVar___<IsHeldOpen>k__BackingField.SetRegistered();
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x000F65DE File Offset: 0x000F47DE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x000F65EC File Offset: 0x000F47EC
		private void RpcWriter___Server_SetPlayerUser_3323014238(NetworkObject playerObject)
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
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000F6694 File Offset: 0x000F4894
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000F66E8 File Offset: 0x000F48E8
		private void RpcReader___Server_SetPlayerUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x000F6728 File Offset: 0x000F4928
		private void RpcWriter___Server_SetNPCUser_3323014238(NetworkObject npcObject)
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
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x000F67CF File Offset: 0x000F49CF
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x000F67D8 File Offset: 0x000F49D8
		private void RpcReader___Server_SetNPCUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000F6818 File Offset: 0x000F4A18
		private void RpcWriter___Server_SetHeldOpen_1140765316(bool open)
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
			writer.WriteBoolean(open);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000F68BF File Offset: 0x000F4ABF
		public void RpcLogic___SetHeldOpen_1140765316(bool open)
		{
			if (open && !this.IsHeldOpen)
			{
				this.SqueakSound.Play();
			}
			this.IsHeldOpen = open;
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000F68E0 File Offset: 0x000F4AE0
		private void RpcReader___Server_SetHeldOpen_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool open = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetHeldOpen_1140765316(open);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x000F6920 File Offset: 0x000F4B20
		private void RpcWriter___Server_SendWateringCanModel_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x000F69C7 File Offset: 0x000F4BC7
		public void RpcLogic___SendWateringCanModel_3615296227(string ID)
		{
			this.CreateWateringCanModel(ID);
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x000F69D0 File Offset: 0x000F4BD0
		private void RpcReader___Server_SendWateringCanModel_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWateringCanModel_3615296227(id);
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x000F6A10 File Offset: 0x000F4C10
		private void RpcWriter___Observers_CreateWateringCanModel_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x000F6AC6 File Offset: 0x000F4CC6
		private void RpcLogic___CreateWateringCanModel_3615296227(string ID)
		{
			this.wateringCanModel = this.CreateWateringCanModel_Local(ID, false);
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x000F6AD8 File Offset: 0x000F4CD8
		private void RpcReader___Observers_CreateWateringCanModel_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateWateringCanModel_3615296227(id);
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x000F6B14 File Offset: 0x000F4D14
		private void RpcWriter___Server_SendClearWateringCanModelModel_2166136261()
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
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x000F6BAE File Offset: 0x000F4DAE
		public void RpcLogic___SendClearWateringCanModelModel_2166136261()
		{
			this.ClearWateringCanModel();
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x000F6BB8 File Offset: 0x000F4DB8
		private void RpcReader___Server_SendClearWateringCanModelModel_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendClearWateringCanModelModel_2166136261();
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x000F6BE8 File Offset: 0x000F4DE8
		private void RpcWriter___Observers_ClearWateringCanModel_2166136261()
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x000F6C91 File Offset: 0x000F4E91
		private void RpcLogic___ClearWateringCanModel_2166136261()
		{
			if (this.wateringCanModel != null)
			{
				UnityEngine.Object.Destroy(this.wateringCanModel);
				this.wateringCanModel = null;
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x000F6CB4 File Offset: 0x000F4EB4
		private void RpcReader___Observers_ClearWateringCanModel_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearWateringCanModel_2166136261();
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x000F6CDE File Offset: 0x000F4EDE
		// (set) Token: 0x06003A54 RID: 14932 RVA: 0x000F6CE6 File Offset: 0x000F4EE6
		public bool SyncAccessor_<IsHeldOpen>k__BackingField
		{
			get
			{
				return this.<IsHeldOpen>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<IsHeldOpen>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<IsHeldOpen>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x000F6D24 File Offset: 0x000F4F24
		public override bool Tap(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value2, Boolean2);
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
					this.sync___set_value_<IsHeldOpen>k__BackingField(this.syncVar___<IsHeldOpen>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value3 = PooledReader0.ReadBoolean();
				this.sync___set_value_<IsHeldOpen>k__BackingField(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x000F6DFE File Offset: 0x000F4FFE
		// (set) Token: 0x06003A57 RID: 14935 RVA: 0x000F6E06 File Offset: 0x000F5006
		public NetworkObject SyncAccessor_<NPCUserObject>k__BackingField
		{
			get
			{
				return this.<NPCUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<NPCUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<NPCUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x000F6E42 File Offset: 0x000F5042
		// (set) Token: 0x06003A59 RID: 14937 RVA: 0x000F6E4A File Offset: 0x000F504A
		public NetworkObject SyncAccessor_<PlayerUserObject>k__BackingField
		{
			get
			{
				return this.<PlayerUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x000F6E86 File Offset: 0x000F5086
		private void dll()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x040029DB RID: 10715
		public const float MaxFlowRate = 6f;

		// Token: 0x040029DD RID: 10717
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x040029DE RID: 10718
		public Transform CameraPos;

		// Token: 0x040029DF RID: 10719
		public Transform WateringCamPos;

		// Token: 0x040029E0 RID: 10720
		public Collider HandleCollider;

		// Token: 0x040029E1 RID: 10721
		public Transform Handle;

		// Token: 0x040029E2 RID: 10722
		public Clickable HandleClickable;

		// Token: 0x040029E3 RID: 10723
		public ParticleSystem WaterParticles;

		// Token: 0x040029E4 RID: 10724
		public AudioSourceController SqueakSound;

		// Token: 0x040029E5 RID: 10725
		public AudioSourceController WaterRunningSound;

		// Token: 0x040029E8 RID: 10728
		private float tapFlow;

		// Token: 0x040029E9 RID: 10729
		private GameObject wateringCanModel;

		// Token: 0x040029EA RID: 10730
		private bool intObjSetThisFrame;

		// Token: 0x040029EB RID: 10731
		public SyncVar<bool> syncVar___<IsHeldOpen>k__BackingField;

		// Token: 0x040029EC RID: 10732
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x040029ED RID: 10733
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x040029EE RID: 10734
		private bool dll_Excuted;

		// Token: 0x040029EF RID: 10735
		private bool dll_Excuted;
	}
}
