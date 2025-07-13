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
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C3C RID: 3132
	public class OldMixingStation : GridItem, IUsable, IItemSlotOwner
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x00170E2C File Offset: 0x0016F02C
		// (set) Token: 0x06005740 RID: 22336 RVA: 0x00170E34 File Offset: 0x0016F034
		public bool IsOpen { get; private set; }

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x00170E3D File Offset: 0x0016F03D
		// (set) Token: 0x06005742 RID: 22338 RVA: 0x00170E45 File Offset: 0x0016F045
		public MixOperation CurrentMixOperation { get; set; }

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x00170E4E File Offset: 0x0016F04E
		// (set) Token: 0x06005744 RID: 22340 RVA: 0x00170E56 File Offset: 0x0016F056
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x00170E5F File Offset: 0x0016F05F
		// (set) Token: 0x06005746 RID: 22342 RVA: 0x00170E67 File Offset: 0x0016F067
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

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06005747 RID: 22343 RVA: 0x00170E71 File Offset: 0x0016F071
		// (set) Token: 0x06005748 RID: 22344 RVA: 0x00170E79 File Offset: 0x0016F079
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

		// Token: 0x06005749 RID: 22345 RVA: 0x00170E84 File Offset: 0x0016F084
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.OldMixingStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x00170EA3 File Offset: 0x0016F0A3
		protected override void Start()
		{
			base.Start();
			if (!this.isGhost)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x00170ED9 File Offset: 0x0016F0D9
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x00170EE9 File Offset: 0x0016F0E9
		public override void DestroyItem(bool callOnServer = true)
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			base.DestroyItem(callOnServer);
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x00170F18 File Offset: 0x0016F118
		private void MinPass()
		{
			if (InstanceFinder.IsServer && this.CurrentMixOperation != null)
			{
				this.UpdateScreen();
			}
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x00170F30 File Offset: 0x0016F130
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetMixOperation(NetworkConnection conn, MixOperation operation)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetMixOperation_719366965(conn, operation);
				this.RpcLogic___SetMixOperation_719366965(conn, operation);
			}
			else
			{
				this.RpcWriter___Target_SetMixOperation_719366965(conn, operation);
			}
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x00170F71 File Offset: 0x0016F171
		private void EnableScreen()
		{
			this.UpdateScreen();
			this.ScreenCanvas.enabled = true;
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x00170F85 File Offset: 0x0016F185
		private void UpdateScreen()
		{
			MixOperation currentMixOperation = this.CurrentMixOperation;
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x00170F8E File Offset: 0x0016F18E
		private void DisableScreen()
		{
			this.ScreenCanvas.enabled = false;
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x000045B1 File Offset: 0x000027B1
		public void CompleteMixOperation()
		{
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x00170F9C File Offset: 0x0016F19C
		public bool DoesOutputHaveSpace(StationRecipe recipe)
		{
			StorableItemInstance productInstance = recipe.GetProductInstance(this.GetIngredients());
			return this.OutputSlot.GetCapacityForItem(productInstance, false) >= 1;
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x00170FCC File Offset: 0x0016F1CC
		public List<ItemInstance> GetIngredients()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			if (this.ProductSlot.ItemInstance != null)
			{
				list.Add(this.ProductSlot.ItemInstance);
			}
			if (this.MixerSlot.ItemInstance != null)
			{
				list.Add(this.MixerSlot.ItemInstance);
			}
			return list;
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x0017101C File Offset: 0x0016F21C
		public void Open()
		{
			this.IsOpen = true;
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001710B8 File Offset: 0x0016F2B8
		public void Close()
		{
			this.IsOpen = false;
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x00171130 File Offset: 0x0016F330
		public void Hovered()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage("Use " + base.ItemInstance.Name);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x0017118A File Offset: 0x0016F38A
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001711A7 File Offset: 0x0016F3A7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001711D0 File Offset: 0x0016F3D0
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x0017122F File Offset: 0x0016F42F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x0017124D File Offset: 0x0016F44D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x0017126B File Offset: 0x0016F46B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001712A4 File Offset: 0x0016F4A4
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x00171323 File Offset: 0x0016F523
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x0017134C File Offset: 0x0016F54C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetSlotFilter_Internal(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
			else
			{
				this.RpcWriter___Target_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x001713AB File Offset: 0x0016F5AB
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x001713C1 File Offset: 0x0016F5C1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001713D8 File Offset: 0x0016F5D8
		public override BuildableItemData GetBaseData()
		{
			return new MixingStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.ProductSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.MixerSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.CurrentMixOperation, 0);
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x00171478 File Offset: 0x0016F678
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.OldMixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.OldMixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_SetMixOperation_719366965));
			base.RegisterTargetRpc(9U, new ClientRpcDelegate(this.RpcReader___Target_SetMixOperation_719366965));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(14U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterServerRpc(21U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.OldMixingStation));
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0017165D File Offset: 0x0016F85D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.OldMixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.OldMixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0017168C File Offset: 0x0016F88C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x0017169C File Offset: 0x0016F89C
		private void RpcWriter___Observers_SetMixOperation_719366965(NetworkConnection conn, MixOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x00171754 File Offset: 0x0016F954
		public void RpcLogic___SetMixOperation_719366965(NetworkConnection conn, MixOperation operation)
		{
			MixOperation currentMixOperation = this.CurrentMixOperation;
			this.CurrentMixOperation = operation;
			if (operation != null)
			{
				if (currentMixOperation == null)
				{
					this.Animation.Play("Mixing station start");
					this.MachineSound.StartAudio();
					this.StartSound.Play();
				}
				this.EnableScreen();
				return;
			}
			if (currentMixOperation != null)
			{
				this.Animation.Play("Mixing station end");
				this.MachineSound.StopAudio();
				this.StopSound.Play();
			}
			this.DisableScreen();
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001717D4 File Offset: 0x0016F9D4
		private void RpcReader___Observers_SetMixOperation_719366965(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_719366965(null, operation);
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x00171810 File Offset: 0x0016FA10
		private void RpcWriter___Target_SetMixOperation_719366965(NetworkConnection conn, MixOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			base.SendTargetRpc(9U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x001718C8 File Offset: 0x0016FAC8
		private void RpcReader___Target_SetMixOperation_719366965(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_719366965(base.LocalConnection, operation);
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x00171900 File Offset: 0x0016FB00
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001719C6 File Offset: 0x0016FBC6
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001719D4 File Offset: 0x0016FBD4
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x00171A3C File Offset: 0x0016FC3C
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x00171B03 File Offset: 0x0016FD03
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x00171B30 File Offset: 0x0016FD30
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x00171B88 File Offset: 0x0016FD88
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x00171C50 File Offset: 0x0016FE50
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x00171CA4 File Offset: 0x0016FEA4
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendServerRpc(13U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x00171D62 File Offset: 0x0016FF62
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x00171D6C File Offset: 0x0016FF6C
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x00171DC8 File Offset: 0x0016FFC8
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendObserversRpc(14U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x00171E95 File Offset: 0x00170095
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x00171EAC File Offset: 0x001700AC
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600577E RID: 22398 RVA: 0x00171F04 File Offset: 0x00170104
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600577F RID: 22399 RVA: 0x00171FE4 File Offset: 0x001701E4
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x00171FF4 File Offset: 0x001701F4
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x0017207C File Offset: 0x0017027C
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(16U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x0017215D File Offset: 0x0017035D
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x0017218C File Offset: 0x0017038C
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x00172208 File Offset: 0x00170408
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x001722EC File Offset: 0x001704EC
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x00172360 File Offset: 0x00170560
		private void RpcWriter___Server_SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendServerRpc(18U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x00172426 File Offset: 0x00170626
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x00172450 File Offset: 0x00170650
		private void RpcReader___Server_SetSlotFilter_527532783(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_527532783(conn2, itemSlotIndex, filter);
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x001724B8 File Offset: 0x001706B8
		private void RpcWriter___Observers_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x00172580 File Offset: 0x00170780
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x00172598 File Offset: 0x00170798
		private void RpcReader___Observers_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(null, itemSlotIndex, filter);
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x001725EC File Offset: 0x001707EC
		private void RpcWriter___Target_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendTargetRpc(20U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x001726B4 File Offset: 0x001708B4
		private void RpcReader___Target_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(base.LocalConnection, itemSlotIndex, filter);
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x0017270C File Offset: 0x0017090C
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
			base.SendServerRpc(21U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x001727B3 File Offset: 0x001709B3
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x001727BC File Offset: 0x001709BC
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

		// Token: 0x06005791 RID: 22417 RVA: 0x001727FC File Offset: 0x001709FC
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
			base.SendServerRpc(22U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x001728A3 File Offset: 0x00170AA3
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x001728AC File Offset: 0x00170AAC
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

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06005794 RID: 22420 RVA: 0x001728EA File Offset: 0x00170AEA
		// (set) Token: 0x06005795 RID: 22421 RVA: 0x001728F2 File Offset: 0x00170AF2
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

		// Token: 0x06005796 RID: 22422 RVA: 0x00172930 File Offset: 0x00170B30
		public override bool OldMixingStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
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
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06005797 RID: 22423 RVA: 0x001729C6 File Offset: 0x00170BC6
		// (set) Token: 0x06005798 RID: 22424 RVA: 0x001729CE File Offset: 0x00170BCE
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

		// Token: 0x06005799 RID: 22425 RVA: 0x00172A0C File Offset: 0x00170C0C
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.ProductSlot.AddFilter(new ItemFilter_UnpackagedProduct());
				this.ProductSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.ProductSlot, false);
				ItemSlot productSlot = this.ProductSlot;
				productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.MixerSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
				{
					EItemCategory.Ingredient
				}));
				this.MixerSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.MixerSlot, false);
				ItemSlot mixerSlot = this.MixerSlot;
				mixerSlot.onItemDataChanged = (Action)Delegate.Combine(mixerSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				ItemSlot outputSlot = this.OutputSlot;
				outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
			}
		}

		// Token: 0x04004052 RID: 16466
		public int MIX_TIME_PER_ITEM = 15;

		// Token: 0x04004053 RID: 16467
		public int MIN_MIX_TIME = 30;

		// Token: 0x04004056 RID: 16470
		public ItemSlot ProductSlot;

		// Token: 0x04004057 RID: 16471
		public ItemSlot MixerSlot;

		// Token: 0x04004058 RID: 16472
		public ItemSlot OutputSlot;

		// Token: 0x0400405C RID: 16476
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x0400405D RID: 16477
		public Transform CameraPosition;

		// Token: 0x0400405E RID: 16478
		public StorageVisualizer InputVisuals;

		// Token: 0x0400405F RID: 16479
		public StorageVisualizer OutputVisuals;

		// Token: 0x04004060 RID: 16480
		public Animation Animation;

		// Token: 0x04004061 RID: 16481
		[Header("Screen")]
		public Canvas ScreenCanvas;

		// Token: 0x04004062 RID: 16482
		public Image OutputIcon;

		// Token: 0x04004063 RID: 16483
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x04004064 RID: 16484
		public TextMeshProUGUI ProgressLabel;

		// Token: 0x04004065 RID: 16485
		[Header("Sounds")]
		public StartLoopStopAudio MachineSound;

		// Token: 0x04004066 RID: 16486
		public AudioSourceController StartSound;

		// Token: 0x04004067 RID: 16487
		public AudioSourceController StopSound;

		// Token: 0x04004068 RID: 16488
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04004069 RID: 16489
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x0400406A RID: 16490
		private bool dll_Excuted;

		// Token: 0x0400406B RID: 16491
		private bool dll_Excuted;
	}
}
