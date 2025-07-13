using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Trash
{
	// Token: 0x02000872 RID: 2162
	public class TrashManager : NetworkSingleton<TrashManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x000F9EEC File Offset: 0x000F80EC
		public string SaveFolderName
		{
			get
			{
				return "Trash";
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x000F9EEC File Offset: 0x000F80EC
		public string SaveFileName
		{
			get
			{
				return "Trash";
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x000F9EF3 File Offset: 0x000F80F3
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x000F9EFB File Offset: 0x000F80FB
		// (set) Token: 0x06003B23 RID: 15139 RVA: 0x000F9F03 File Offset: 0x000F8103
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x000F9F0C File Offset: 0x000F810C
		// (set) Token: 0x06003B25 RID: 15141 RVA: 0x000F9F14 File Offset: 0x000F8114
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x000F9F1D File Offset: 0x000F811D
		// (set) Token: 0x06003B27 RID: 15143 RVA: 0x000F9F25 File Offset: 0x000F8125
		public bool HasChanged { get; set; } = true;

		// Token: 0x06003B28 RID: 15144 RVA: 0x000F9F2E File Offset: 0x000F812E
		protected override void Start()
		{
			base.Start();
			this.InitializeSaveable();
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000F9F3C File Offset: 0x000F813C
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			Console.Log("Sending " + this.trashItems.Count.ToString() + " trash items to new player", null);
			foreach (TrashItem trashItem in this.trashItems)
			{
				this.CreateTrashItem(connection, trashItem.ID, trashItem.transform.position, trashItem.transform.rotation, Vector3.zero, null, trashItem.GUID.ToString(), false);
			}
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000F9FF8 File Offset: 0x000F81F8
		public void ReplicateTransformData(TrashItem trash)
		{
			this.SendTransformData(trash.GUID.ToString(), trash.transform.position, trash.transform.rotation, trash.Rigidbody.velocity, Player.Local.LocalConnection);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000FA04A File Offset: 0x000F824A
		[ServerRpc(RequireOwnership = false)]
		private void SendTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.RpcWriter___Server_SendTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000FA068 File Offset: 0x000F8268
		[ObserversRpc]
		private void ReceiveTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.RpcWriter___Observers_ReceiveTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x000FA090 File Offset: 0x000F8290
		public TrashItem CreateTrashItem(string id, Vector3 posiiton, Quaternion rotation, Vector3 initialVelocity = default(Vector3), string guid = "", bool startKinematic = false)
		{
			if (guid == "")
			{
				guid = Guid.NewGuid().ToString();
			}
			this.SendTrashItem(id, posiiton, rotation, initialVelocity, Player.Local.LocalConnection, guid, false);
			return this.CreateAndReturnTrashItem(id, posiiton, rotation, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000FA0E8 File Offset: 0x000F82E8
		[ServerRpc(RequireOwnership = false)]
		private void SendTrashItem(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			this.RpcWriter___Server_SendTrashItem_478112418(id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000FA118 File Offset: 0x000F8318
		[ObserversRpc]
		[TargetRpc]
		private void CreateTrashItem(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateTrashItem_2385526393(conn, id, position, rotation, initialVelocity, sender, guid, startKinematic);
			}
			else
			{
				this.RpcWriter___Target_CreateTrashItem_2385526393(conn, id, position, rotation, initialVelocity, sender, guid, startKinematic);
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000FA17C File Offset: 0x000F837C
		private TrashItem CreateAndReturnTrashItem(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, string guid, bool startKinematic)
		{
			TrashItem trashPrefab = this.GetTrashPrefab(id);
			if (trashPrefab == null)
			{
				Debug.LogError("Trash item with ID " + id + " not found.");
				return null;
			}
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(guid)))
			{
				return null;
			}
			trashPrefab.Draggable.CreateCoM = false;
			trashPrefab.GetComponent<PhysicsDamageable>().ForceMultiplier = this.TrashForceMultiplier;
			TrashItem trashItem = UnityEngine.Object.Instantiate<TrashItem>(trashPrefab, position, rotation, NetworkSingleton<GameManager>.Instance.Temp);
			trashItem.SetGUID(new Guid(guid));
			if (!startKinematic)
			{
				trashItem.SetContinuousCollisionDetection();
			}
			if (initialVelocity != default(Vector3))
			{
				trashItem.SetVelocity(initialVelocity);
			}
			this.trashItems.Add(trashItem);
			this.HasChanged = true;
			return trashItem;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000FA238 File Offset: 0x000F8438
		public TrashItem CreateTrashBag(string id, Vector3 posiiton, Quaternion rotation, TrashContentData content, Vector3 initialVelocity = default(Vector3), string guid = "", bool startKinematic = false)
		{
			if (guid == "")
			{
				guid = Guid.NewGuid().ToString();
			}
			this.SendTrashBag(id, posiiton, rotation, content, initialVelocity, Player.Local.LocalConnection, guid, false);
			return this.CreateAndReturnTrashBag(id, posiiton, rotation, content, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000FA294 File Offset: 0x000F8494
		[ServerRpc(RequireOwnership = false)]
		private void SendTrashBag(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			this.RpcWriter___Server_SendTrashBag_3965031115(id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000FA2C8 File Offset: 0x000F84C8
		[ObserversRpc]
		[TargetRpc]
		private void CreateTrashBag(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateTrashBag_680856992(conn, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
			}
			else
			{
				this.RpcWriter___Target_CreateTrashBag_680856992(conn, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
			}
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000FA334 File Offset: 0x000F8534
		private TrashItem CreateAndReturnTrashBag(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, string guid, bool startKinematic)
		{
			TrashBag trashBag = this.GetTrashPrefab(id) as TrashBag;
			if (trashBag == null)
			{
				Debug.LogError("Trash item with ID " + id + " not found.");
				return null;
			}
			TrashBag trashBag2 = UnityEngine.Object.Instantiate<TrashBag>(trashBag, position, rotation, NetworkSingleton<GameManager>.Instance.Temp);
			trashBag2.SetGUID(new Guid(guid));
			trashBag2.LoadContent(content);
			if (!startKinematic)
			{
				trashBag2.SetContinuousCollisionDetection();
			}
			if (initialVelocity != default(Vector3))
			{
				trashBag2.SetVelocity(initialVelocity);
			}
			this.trashItems.Add(trashBag2);
			this.HasChanged = true;
			return trashBag2;
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000FA3D0 File Offset: 0x000F85D0
		public void DestroyAllTrash()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			List<TrashItem> list = new List<TrashItem>();
			list.AddRange(this.trashItems);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].DestroyTrash();
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000FA414 File Offset: 0x000F8614
		public void DestroyTrash(TrashItem trash)
		{
			this.SendDestroyTrash(trash.GUID.ToString());
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000FA43B File Offset: 0x000F863B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendDestroyTrash(string guid)
		{
			this.RpcWriter___Server_SendDestroyTrash_3615296227(guid);
			this.RpcLogic___SendDestroyTrash_3615296227(guid);
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000FA454 File Offset: 0x000F8654
		[ObserversRpc(RunLocally = true)]
		private void DestroyTrash(string guid)
		{
			this.RpcWriter___Observers_DestroyTrash_3615296227(guid);
			this.RpcLogic___DestroyTrash_3615296227(guid);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000FA478 File Offset: 0x000F8678
		public TrashItem GetTrashPrefab(string id)
		{
			return this.TrashPrefabs.FirstOrDefault((TrashItem t) => t.ID == id);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x000FA4AC File Offset: 0x000F86AC
		public TrashItem GetRandomGeneratableTrashPrefab()
		{
			float maxInclusive = this.GenerateableTrashItems.Sum((TrashManager.TrashItemData t) => t.GenerationChance);
			float num = UnityEngine.Random.Range(0f, maxInclusive);
			foreach (TrashManager.TrashItemData trashItemData in this.GenerateableTrashItems)
			{
				if (num < trashItemData.GenerationChance)
				{
					return trashItemData.Item;
				}
				num -= trashItemData.GenerationChance;
			}
			return this.GenerateableTrashItems[this.GenerateableTrashItems.Length - 1].Item;
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000FA53C File Offset: 0x000F873C
		public virtual string GetSaveString()
		{
			List<ScheduleOne.Persistence.Datas.TrashItemData> list = new List<ScheduleOne.Persistence.Datas.TrashItemData>();
			int num = 0;
			while (num < this.trashItems.Count && num < 2000)
			{
				list.Add(this.trashItems[num].GetData());
				num++;
			}
			List<TrashGeneratorData> list2 = new List<TrashGeneratorData>();
			foreach (TrashGenerator trashGenerator in TrashGenerator.AllGenerators)
			{
				if (trashGenerator.ShouldSave())
				{
					list2.Add(trashGenerator.GetSaveData());
				}
			}
			return new TrashData(list.ToArray(), list2.ToArray()).GetJson(true);
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000FA654 File Offset: 0x000F8854
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendTransformData_2990100769));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveTransformData_2990100769));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendTrashItem_478112418));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_CreateTrashItem_2385526393));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_CreateTrashItem_2385526393));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendTrashBag_3965031115));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_CreateTrashBag_680856992));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_CreateTrashBag_680856992));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendDestroyTrash_3615296227));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyTrash_3615296227));
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000FA75E File Offset: 0x000F895E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x000FA777 File Offset: 0x000F8977
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000FA788 File Offset: 0x000F8988
		private void RpcWriter___Server_SendTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(velocity);
			writer.WriteNetworkConnection(sender);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000FA868 File Offset: 0x000F8A68
		private void RpcLogic___SendTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.ReceiveTransformData(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000FA878 File Offset: 0x000F8A78
		private void RpcReader___Server_SendTransformData_2990100769(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 velocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000FA8F4 File Offset: 0x000F8AF4
		private void RpcWriter___Observers_ReceiveTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(velocity);
			writer.WriteNetworkConnection(sender);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000FA9E4 File Offset: 0x000F8BE4
		private void RpcLogic___ReceiveTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			TrashItem @object = GUIDManager.GetObject<TrashItem>(new Guid(guid));
			if (@object == null)
			{
				return;
			}
			@object.transform.position = position;
			@object.transform.rotation = rotation;
			@object.Rigidbody.velocity = velocity;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000FAA38 File Offset: 0x000F8C38
		private void RpcReader___Observers_ReceiveTransformData_2990100769(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 velocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x000FAAB4 File Offset: 0x000F8CB4
		private void RpcWriter___Server_SendTrashItem_478112418(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x000FABB0 File Offset: 0x000F8DB0
		private void RpcLogic___SendTrashItem_478112418(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (this.trashItems.Count >= 2000)
			{
				this.trashItems[UnityEngine.Random.Range(0, this.trashItems.Count)].DestroyTrash();
			}
			this.CreateTrashItem(null, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x000FAC04 File Offset: 0x000F8E04
		private void RpcReader___Server_SendTrashItem_478112418(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTrashItem_478112418(id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x000FACA0 File Offset: 0x000F8EA0
		private void RpcWriter___Observers_CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x000FADA9 File Offset: 0x000F8FA9
		private void RpcLogic___CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			this.CreateAndReturnTrashItem(id, position, rotation, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x000FADC8 File Offset: 0x000F8FC8
		private void RpcReader___Observers_CreateTrashItem_2385526393(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashItem_2385526393(null, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x000FAE68 File Offset: 0x000F9068
		private void RpcWriter___Target_CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x000FAF70 File Offset: 0x000F9170
		private void RpcReader___Target_CreateTrashItem_2385526393(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashItem_2385526393(base.LocalConnection, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x000FB014 File Offset: 0x000F9214
		private void RpcWriter___Server_SendTrashBag_3965031115(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x000FB11C File Offset: 0x000F931C
		private void RpcLogic___SendTrashBag_3965031115(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (this.trashItems.Count >= 2000)
			{
				this.trashItems[UnityEngine.Random.Range(0, this.trashItems.Count)].DestroyTrash();
			}
			this.CreateTrashBag(null, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x000FB170 File Offset: 0x000F9370
		private void RpcReader___Server_SendTrashBag_3965031115(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			TrashContentData content = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTrashBag_3965031115(id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x000FB220 File Offset: 0x000F9420
		private void RpcWriter___Observers_CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x000FB336 File Offset: 0x000F9536
		private void RpcLogic___CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			this.CreateAndReturnTrashBag(id, position, rotation, content, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x000FB358 File Offset: 0x000F9558
		private void RpcReader___Observers_CreateTrashBag_680856992(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			TrashContentData content = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashBag_680856992(null, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x000FB408 File Offset: 0x000F9608
		private void RpcWriter___Target_CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendTargetRpc(7U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x000FB520 File Offset: 0x000F9720
		private void RpcReader___Target_CreateTrashBag_680856992(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			TrashContentData content = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashBag_680856992(base.LocalConnection, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x000FB5D4 File Offset: 0x000F97D4
		private void RpcWriter___Server_SendDestroyTrash_3615296227(string guid)
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
			writer.WriteString(guid);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x000FB67B File Offset: 0x000F987B
		private void RpcLogic___SendDestroyTrash_3615296227(string guid)
		{
			this.DestroyTrash(guid);
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x000FB684 File Offset: 0x000F9884
		private void RpcReader___Server_SendDestroyTrash_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDestroyTrash_3615296227(guid);
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x000FB6C4 File Offset: 0x000F98C4
		private void RpcWriter___Observers_DestroyTrash_3615296227(string guid)
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
			writer.WriteString(guid);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x000FB77C File Offset: 0x000F997C
		private void RpcLogic___DestroyTrash_3615296227(string guid)
		{
			TrashItem @object = GUIDManager.GetObject<TrashItem>(new Guid(guid));
			if (@object == null)
			{
				return;
			}
			this.trashItems.Remove(@object);
			GUIDManager.DeregisterObject(@object);
			if (@object.onDestroyed != null)
			{
				@object.onDestroyed(@object);
			}
			@object.Deinitialize();
			UnityEngine.Object.Destroy(@object.gameObject);
			this.HasChanged = true;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000FB7E0 File Offset: 0x000F99E0
		private void RpcReader___Observers_DestroyTrash_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DestroyTrash_3615296227(guid);
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x000FB81B File Offset: 0x000F9A1B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002A51 RID: 10833
		public const int TRASH_ITEM_LIMIT = 2000;

		// Token: 0x04002A52 RID: 10834
		public TrashItem[] TrashPrefabs;

		// Token: 0x04002A53 RID: 10835
		public TrashItem TrashBagPrefab;

		// Token: 0x04002A54 RID: 10836
		public TrashManager.TrashItemData[] GenerateableTrashItems;

		// Token: 0x04002A55 RID: 10837
		private List<TrashItem> trashItems = new List<TrashItem>();

		// Token: 0x04002A56 RID: 10838
		public float TrashForceMultiplier = 0.3f;

		// Token: 0x04002A57 RID: 10839
		private TrashLoader loader = new TrashLoader();

		// Token: 0x04002A5B RID: 10843
		private List<string> writtenItemFiles = new List<string>();

		// Token: 0x04002A5C RID: 10844
		private bool dll_Excuted;

		// Token: 0x04002A5D RID: 10845
		private bool dll_Excuted;

		// Token: 0x02000873 RID: 2163
		[Serializable]
		public class TrashItemData
		{
			// Token: 0x04002A5E RID: 10846
			public TrashItem Item;

			// Token: 0x04002A5F RID: 10847
			[Range(0f, 1f)]
			public float GenerationChance = 0.5f;
		}
	}
}
