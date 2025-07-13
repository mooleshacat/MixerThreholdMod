using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008DF RID: 2271
	public class Pallet : NetworkBehaviour, IStorageEntity
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x00101202 File Offset: 0x000FF402
		public bool isEmpty
		{
			get
			{
				return this._storedItemContainer.childCount == 0;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x00101212 File Offset: 0x000FF412
		protected bool carriedByForklift
		{
			get
			{
				return this.forkliftsInContact.Count > 0;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06003D19 RID: 15641 RVA: 0x00101222 File Offset: 0x000FF422
		public Transform storedItemContainer
		{
			get
			{
				return this._storedItemContainer;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003D1A RID: 15642 RVA: 0x0010122A File Offset: 0x000FF42A
		public Dictionary<StoredItem, Employee> reservedItems
		{
			get
			{
				return this._reservedItems;
			}
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00101232 File Offset: 0x000FF432
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.Pallet_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00101246 File Offset: 0x000FF446
		public override void OnStartServer()
		{
			base.OnStartServer();
			if (this.currentSlot == null)
			{
				this.rb.isKinematic = false;
				this.rb.interpolation = 1;
			}
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00101274 File Offset: 0x000FF474
		[ServerRpc(RequireOwnership = false)]
		protected virtual void SetOwner(NetworkConnection conn)
		{
			this.RpcWriter___Server_SetOwner_328543758(conn);
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x0010128C File Offset: 0x000FF48C
		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			base.OnOwnershipClient(prevOwner);
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				if (this.rb != null)
				{
					this.rb.interpolation = 1;
					this.rb.collisionDetectionMode = 1;
					this.rb.isKinematic = false;
				}
				if (!Pallet.palletsOwnedByLocalPlayer.Contains(this))
				{
					Pallet.palletsOwnedByLocalPlayer.Add(this);
					return;
				}
			}
			else
			{
				if (this.rb != null)
				{
					this.rb.interpolation = 0;
					this.rb.isKinematic = true;
				}
				if (Pallet.palletsOwnedByLocalPlayer.Contains(this))
				{
					Pallet.palletsOwnedByLocalPlayer.Remove(this);
				}
			}
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x00101342 File Offset: 0x000FF542
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendItemsToClient(connection);
			if (this.currentSlot != null)
			{
				this.BindToSlot(connection, this.currentSlot.GUID);
			}
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x00101374 File Offset: 0x000FF574
		private void SendItemsToClient(NetworkConnection connection)
		{
			StoredItem[] componentsInChildren = this._storedItemContainer.GetComponentsInChildren<StoredItem>();
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.CreateStoredItem(connection, componentsInChildren[i].item, storageGrids.IndexOf(componentsInChildren[i].parentGrid), componentsInChildren[i].CoordinatePairs[0].coord2, componentsInChildren[i].Rotation, "", false);
			}
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x001013E8 File Offset: 0x000FF5E8
		public virtual void DestroyPallet()
		{
			base.Despawn(null);
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00101404 File Offset: 0x000FF604
		protected virtual void Update()
		{
			this.timeSinceSlotCheck += Time.deltaTime;
			if (this.currentSlot == null)
			{
				this.timeBoundToSlot = 0f;
				return;
			}
			this.timeBoundToSlot += Time.deltaTime;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00101444 File Offset: 0x000FF644
		protected virtual void FixedUpdate()
		{
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				if (this.carriedByForklift)
				{
					if (this.currentSlot != null && this.timeBoundToSlot > 1f)
					{
						Console.Log("Exiting", null);
						this.ExitSlot_Server();
					}
				}
				else if (this.currentSlot == null && this.timeSinceSlotCheck >= 0.5f)
				{
					this.timeSinceSlotCheck = 0f;
					Collider[] array = Physics.OverlapSphere(base.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Pallet"), 2);
					for (int i = 0; i < array.Length; i++)
					{
						PalletSlot componentInParent = array[i].gameObject.GetComponentInParent<PalletSlot>();
						if (componentInParent != null && componentInParent.occupant == null)
						{
							this.BindToSlot_Server(componentInParent.GUID);
							break;
						}
					}
				}
				if (base.transform.position.y < -20f && this.currentSlot == null)
				{
					if (this.rb != null)
					{
						this.rb.velocity = Vector3.zero;
						this.rb.angularVelocity = Vector3.zero;
					}
					float num = 0f;
					if (MapHeightSampler.Sample(base.transform.position.x, out num, base.transform.position.z))
					{
						this.SetPosition(new Vector3(base.transform.position.x, num + 3f, base.transform.position.z));
					}
					else
					{
						this.SetPosition(MapHeightSampler.ResetPosition);
					}
				}
			}
			this.UpdateOwnership();
			this.forkliftsInContact.Clear();
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00101613 File Offset: 0x000FF813
		private void SetPosition(Vector3 position)
		{
			base.transform.position = position;
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00101624 File Offset: 0x000FF824
		private void UpdateOwnership()
		{
			if (this.forkliftsInContact.Count == 0)
			{
				if (base.IsOwner && !InstanceFinder.IsHost)
				{
					base.NetworkObject.SetLocalOwnership(null);
					this.SetOwner(null);
					return;
				}
			}
			else
			{
				NetworkConnection owner = this.forkliftsInContact[0].Owner;
				if (base.Owner != owner && owner == Player.Local.Connection)
				{
					base.NetworkObject.SetLocalOwnership(Player.Local.Connection);
					this.SetOwner(Player.Local.Connection);
				}
			}
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x001016B8 File Offset: 0x000FF8B8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void BindToSlot_Server(Guid slotGuid)
		{
			this.RpcWriter___Server_BindToSlot_Server_1272046255(slotGuid);
			this.RpcLogic___BindToSlot_Server_1272046255(slotGuid);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x001016D0 File Offset: 0x000FF8D0
		[ObserversRpc]
		[TargetRpc]
		private void BindToSlot(NetworkConnection conn, Guid slotGuid)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_BindToSlot_454078614(conn, slotGuid);
			}
			else
			{
				this.RpcWriter___Target_BindToSlot_454078614(conn, slotGuid);
			}
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00101703 File Offset: 0x000FF903
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void ExitSlot_Server()
		{
			this.RpcWriter___Server_ExitSlot_Server_2166136261();
			this.RpcLogic___ExitSlot_Server_2166136261();
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00101714 File Offset: 0x000FF914
		[ObserversRpc]
		private void ExitSlot()
		{
			this.RpcWriter___Observers_ExitSlot_2166136261();
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00101728 File Offset: 0x000FF928
		public void TriggerStay(Collider other)
		{
			Forklift forklift = other.gameObject.GetComponentInParent<Forklift>();
			if (forklift == null)
			{
				ForkliftFork componentInParent = other.gameObject.GetComponentInParent<ForkliftFork>();
				if (componentInParent != null)
				{
					forklift = componentInParent.forklift;
				}
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
			{
				return;
			}
			if (forklift != null && !this.forkliftsInContact.Contains(forklift))
			{
				this.forkliftsInContact.Add(forklift);
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x001017A1 File Offset: 0x000FF9A1
		public List<StoredItem> GetStoredItems()
		{
			return new List<StoredItem>(this.storedItemContainer.GetComponentsInChildren<StoredItem>());
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x001017B3 File Offset: 0x000FF9B3
		public List<StorageGrid> GetStorageGrids()
		{
			return new List<StorageGrid>
			{
				this.storageGrid
			};
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x001017C8 File Offset: 0x000FF9C8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void CreateStoredItem(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
				this.RpcLogic___CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
			}
			else
			{
				this.RpcWriter___Target_CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
			}
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00101845 File Offset: 0x000FFA45
		[ServerRpc(RequireOwnership = false)]
		private void CreateStoredItem_Server(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
		{
			this.RpcWriter___Server_CreateStoredItem_Server_1890711751(data, gridIndex, originCoord, rotation, jobID);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00101864 File Offset: 0x000FFA64
		[ObserversRpc(RunLocally = true)]
		public void DestroyStoredItem(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			this.RpcWriter___Observers_DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x0010189D File Offset: 0x000FFA9D
		[ServerRpc(RequireOwnership = false)]
		private void DestroyStoredItem_Server(int gridIndex, Coordinate coord, string jobID)
		{
			this.RpcWriter___Server_DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001018F4 File Offset: 0x000FFAF4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetOwner_328543758));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_BindToSlot_Server_1272046255));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_BindToSlot_454078614));
			base.RegisterTargetRpc(3U, new ClientRpcDelegate(this.RpcReader___Target_BindToSlot_454078614));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_ExitSlot_Server_2166136261));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ExitSlot_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_CreateStoredItem_913707843));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_CreateStoredItem_913707843));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_CreateStoredItem_Server_1890711751));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyStoredItem_3261517793));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_DestroyStoredItem_Server_3952619116));
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00101A0F File Offset: 0x000FFC0F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00101A22 File Offset: 0x000FFC22
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00101A30 File Offset: 0x000FFC30
		private void RpcWriter___Server_SetOwner_328543758(NetworkConnection conn)
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00101AD8 File Offset: 0x000FFCD8
		protected virtual void RpcLogic___SetOwner_328543758(NetworkConnection conn)
		{
			Console.Log("Setting pallet owner to: " + conn.ClientId.ToString(), null);
			if (base.Owner != null && Player.GetPlayer(base.Owner) != null)
			{
				Player.GetPlayer(base.Owner).objectsTemporarilyOwnedByPlayer.Remove(base.NetworkObject);
			}
			if (conn != null && Player.GetPlayer(conn) != null)
			{
				Player.GetPlayer(conn).objectsTemporarilyOwnedByPlayer.Add(base.NetworkObject);
			}
			base.NetworkObject.GiveOwnership(conn);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00101B78 File Offset: 0x000FFD78
		private void RpcReader___Server_SetOwner_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwner_328543758(conn2);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00101BAC File Offset: 0x000FFDAC
		private void RpcWriter___Server_BindToSlot_Server_1272046255(Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00101C53 File Offset: 0x000FFE53
		public void RpcLogic___BindToSlot_Server_1272046255(Guid slotGuid)
		{
			this.BindToSlot(null, slotGuid);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00101C60 File Offset: 0x000FFE60
		private void RpcReader___Server_BindToSlot_Server_1272046255(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BindToSlot_Server_1272046255(slotGuid);
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00101CA0 File Offset: 0x000FFEA0
		private void RpcWriter___Observers_BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00101D58 File Offset: 0x000FFF58
		private void RpcLogic___BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
		{
			this.currentSlotGUID = slotGuid;
			this.currentSlot = GUIDManager.GetObject<PalletSlot>(slotGuid);
			if (this.currentSlot == null)
			{
				this.currentSlotGUID = Guid.Empty;
				Console.LogWarning("BindToSlot called but slotGuid is not valid", null);
				return;
			}
			this.currentSlot.SetOccupant(this);
			this.networkTransform.enabled = false;
			UnityEngine.Object.Destroy(this.rb);
			base.transform.SetParent(this.currentSlot.transform);
			base.transform.position = this.currentSlot.transform.position + this.currentSlot.transform.up * 0.1f;
			Vector3 vector = this.currentSlot.transform.forward;
			if (Vector3.Angle(base.transform.forward, -this.currentSlot.transform.forward) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = -this.currentSlot.transform.forward;
			}
			if (Vector3.Angle(base.transform.forward, this.currentSlot.transform.right) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = this.currentSlot.transform.right;
			}
			if (Vector3.Angle(base.transform.forward, -this.currentSlot.transform.right) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = -this.currentSlot.transform.right;
			}
			base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			base.transform.localEulerAngles = new Vector3(0f, base.transform.localEulerAngles.y, 0f);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00101F44 File Offset: 0x00100144
		private void RpcReader___Observers_BindToSlot_454078614(PooledReader PooledReader0, Channel channel)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___BindToSlot_454078614(null, slotGuid);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00101F78 File Offset: 0x00100178
		private void RpcWriter___Target_BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendTargetRpc(3U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00102030 File Offset: 0x00100230
		private void RpcReader___Target_BindToSlot_454078614(PooledReader PooledReader0, Channel channel)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___BindToSlot_454078614(base.LocalConnection, slotGuid);
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00102068 File Offset: 0x00100268
		private void RpcWriter___Server_ExitSlot_Server_2166136261()
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
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00102102 File Offset: 0x00100302
		public void RpcLogic___ExitSlot_Server_2166136261()
		{
			this.ExitSlot();
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x0010210C File Offset: 0x0010030C
		private void RpcReader___Server_ExitSlot_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ExitSlot_Server_2166136261();
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x0010213C File Offset: 0x0010033C
		private void RpcWriter___Observers_ExitSlot_2166136261()
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
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x001021E8 File Offset: 0x001003E8
		private void RpcLogic___ExitSlot_2166136261()
		{
			if (this.currentSlot == null)
			{
				return;
			}
			this.currentSlot.SetOccupant(null);
			base.transform.SetParent(null);
			if (this.rb == null)
			{
				this.rb = base.gameObject.AddComponent<Rigidbody>();
			}
			this.rb.mass = this.rb_Mass;
			this.rb.drag = this.rb_Drag;
			this.rb.angularDrag = this.rb_AngularDrag;
			this.rb.interpolation = 1;
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				Console.Log("Exit slot, owner", null);
				this.rb.isKinematic = false;
				this.rb.collisionDetectionMode = 1;
			}
			else
			{
				Console.Log("Exit slot, not owner", null);
				this.rb.isKinematic = true;
				this.rb.interpolation = 0;
			}
			this.networkTransform.enabled = true;
			this.currentSlotGUID = default(Guid);
			this.currentSlot = null;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x001022FC File Offset: 0x001004FC
		private void RpcReader___Observers_ExitSlot_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ExitSlot_2166136261();
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x0010231C File Offset: 0x0010051C
		private void RpcWriter___Observers_CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
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
			writer.WriteStorableItemInstance(item);
			writer.WriteInt32(gridIndex, 1);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, 0);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00102420 File Offset: 0x00100620
		public void RpcLogic___CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
		{
			if (jobID != "")
			{
				if (this.completedJobs.Contains(jobID))
				{
					return;
				}
			}
			else
			{
				jobID = Guid.NewGuid().ToString();
			}
			this.completedJobs.Add(jobID);
			UnityEngine.Object.Instantiate<StoredItem>(item.StoredItem, this.storedItemContainer).GetComponent<StoredItem>();
			if (network)
			{
				this.CreateStoredItem_Server(item, gridIndex, originCoord, rotation, jobID);
			}
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00102498 File Offset: 0x00100698
		private void RpcReader___Observers_CreateStoredItem_913707843(PooledReader PooledReader0, Channel channel)
		{
			StorableItemInstance item = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(1);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(0);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_913707843(null, item, gridIndex, originCoord, rotation, jobID, network);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00102534 File Offset: 0x00100734
		private void RpcWriter___Target_CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
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
			writer.WriteStorableItemInstance(item);
			writer.WriteInt32(gridIndex, 1);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, 0);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendTargetRpc(7U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00102634 File Offset: 0x00100834
		private void RpcReader___Target_CreateStoredItem_913707843(PooledReader PooledReader0, Channel channel)
		{
			StorableItemInstance item = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(1);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(0);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_913707843(base.LocalConnection, item, gridIndex, originCoord, rotation, jobID, network);
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x001026CC File Offset: 0x001008CC
		private void RpcWriter___Server_CreateStoredItem_Server_1890711751(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
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
			writer.WriteStorableItemInstance(data);
			writer.WriteInt32(gridIndex, 1);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, 0);
			writer.WriteString(jobID);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x001027B1 File Offset: 0x001009B1
		private void RpcLogic___CreateStoredItem_Server_1890711751(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
		{
			this.CreateStoredItem(null, data, gridIndex, originCoord, rotation, jobID, false);
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x001027C4 File Offset: 0x001009C4
		private void RpcReader___Server_CreateStoredItem_Server_1890711751(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			StorableItemInstance data = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(1);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(0);
			string jobID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_Server_1890711751(data, gridIndex, originCoord, rotation, jobID);
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x00102844 File Offset: 0x00100A44
		private void RpcWriter___Observers_DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
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
			writer.WriteInt32(gridIndex, 1);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00102928 File Offset: 0x00100B28
		public void RpcLogic___DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			if (jobID != "")
			{
				if (this.completedJobs.Contains(jobID))
				{
					return;
				}
			}
			else
			{
				jobID = Guid.NewGuid().ToString();
			}
			this.completedJobs.Add(jobID);
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			if (gridIndex > storageGrids.Count)
			{
				Console.LogError("DestroyStoredItem: grid index out of range", null);
				return;
			}
			if (storageGrids[gridIndex].GetTile(coord) == null)
			{
				Console.LogError("DestroyStoredItem: no tile found at " + ((coord != null) ? coord.ToString() : null), null);
				return;
			}
			storageGrids[gridIndex].GetTile(coord).occupant.Destroy_Internal();
			if (network)
			{
				this.DestroyStoredItem_Server(gridIndex, coord, jobID);
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x001029E8 File Offset: 0x00100BE8
		private void RpcReader___Observers_DestroyStoredItem_3261517793(PooledReader PooledReader0, Channel channel)
		{
			int gridIndex = PooledReader0.ReadInt32(1);
			Coordinate coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00102A5C File Offset: 0x00100C5C
		private void RpcWriter___Server_DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
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
			writer.WriteInt32(gridIndex, 1);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00102B22 File Offset: 0x00100D22
		private void RpcLogic___DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
		{
			this.DestroyStoredItem(gridIndex, coord, jobID, false);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00102B30 File Offset: 0x00100D30
		private void RpcReader___Server_DestroyStoredItem_Server_3952619116(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int gridIndex = PooledReader0.ReadInt32(1);
			Coordinate coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00102B88 File Offset: 0x00100D88
		protected virtual void dll()
		{
			this.rb_Mass = this.rb.mass;
			this.rb_Drag = this.rb.drag;
			this.rb_AngularDrag = this.rb.angularDrag;
		}

		// Token: 0x04002BE3 RID: 11235
		public static List<Pallet> palletsOwnedByLocalPlayer = new List<Pallet>();

		// Token: 0x04002BE4 RID: 11236
		public static int sizeX = 6;

		// Token: 0x04002BE5 RID: 11237
		public static int sizeY = 6;

		// Token: 0x04002BE6 RID: 11238
		[Header("Reference")]
		public Transform _storedItemContainer;

		// Token: 0x04002BE7 RID: 11239
		public Rigidbody rb;

		// Token: 0x04002BE8 RID: 11240
		public StorageGrid storageGrid;

		// Token: 0x04002BE9 RID: 11241
		public NetworkTransform networkTransform;

		// Token: 0x04002BEA RID: 11242
		protected List<Forklift> forkliftsInContact = new List<Forklift>();

		// Token: 0x04002BEB RID: 11243
		public Guid currentSlotGUID;

		// Token: 0x04002BEC RID: 11244
		private PalletSlot currentSlot;

		// Token: 0x04002BED RID: 11245
		private float timeSinceSlotCheck;

		// Token: 0x04002BEE RID: 11246
		private float timeBoundToSlot;

		// Token: 0x04002BEF RID: 11247
		private float rb_Mass;

		// Token: 0x04002BF0 RID: 11248
		private float rb_Drag;

		// Token: 0x04002BF1 RID: 11249
		private float rb_AngularDrag;

		// Token: 0x04002BF2 RID: 11250
		protected Dictionary<StoredItem, Employee> _reservedItems = new Dictionary<StoredItem, Employee>();

		// Token: 0x04002BF3 RID: 11251
		private List<string> completedJobs = new List<string>();

		// Token: 0x04002BF4 RID: 11252
		private bool dll_Excuted;

		// Token: 0x04002BF5 RID: 11253
		private bool dll_Excuted;
	}
}
