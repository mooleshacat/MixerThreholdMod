using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000815 RID: 2069
	public class VehicleManager : NetworkSingleton<VehicleManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600384E RID: 14414 RVA: 0x000ED7E8 File Offset: 0x000EB9E8
		public string SaveFolderName
		{
			get
			{
				return "OwnedVehicles";
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600384F RID: 14415 RVA: 0x000ED7E8 File Offset: 0x000EB9E8
		public string SaveFileName
		{
			get
			{
				return "OwnedVehicles";
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003850 RID: 14416 RVA: 0x000ED7EF File Offset: 0x000EB9EF
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003851 RID: 14417 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x000ED7F7 File Offset: 0x000EB9F7
		// (set) Token: 0x06003853 RID: 14419 RVA: 0x000ED7FF File Offset: 0x000EB9FF
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x000ED808 File Offset: 0x000EBA08
		// (set) Token: 0x06003855 RID: 14421 RVA: 0x000ED810 File Offset: 0x000EBA10
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x000ED819 File Offset: 0x000EBA19
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x000ED821 File Offset: 0x000EBA21
		public bool HasChanged { get; set; } = true;

		// Token: 0x06003858 RID: 14424 RVA: 0x000ED82A File Offset: 0x000EBA2A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.VehicleManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000ED83E File Offset: 0x000EBA3E
		[ServerRpc(RequireOwnership = false)]
		public void SpawnVehicle(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			this.RpcWriter___Server_SpawnVehicle_3323115898(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000ED858 File Offset: 0x000EBA58
		public LandVehicle SpawnAndReturnVehicle(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			LandVehicle vehiclePrefab = this.GetVehiclePrefab(vehicleCode);
			if (vehiclePrefab == null)
			{
				Console.LogError("SpawnVehicle: '" + vehicleCode + "' is not a valid vehicle code!", null);
				return null;
			}
			LandVehicle component = UnityEngine.Object.Instantiate<GameObject>(vehiclePrefab.gameObject).GetComponent<LandVehicle>();
			component.transform.position = position;
			component.transform.rotation = rotation;
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			component.SetIsPlayerOwned(null, playerOwned);
			if (playerOwned)
			{
				this.PlayerOwnedVehicles.Add(component);
			}
			return component;
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000ED8EC File Offset: 0x000EBAEC
		public LandVehicle GetVehiclePrefab(string vehicleCode)
		{
			return this.VehiclePrefabs.Find((LandVehicle x) => x.VehicleCode.ToLower() == vehicleCode.ToLower());
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000ED91D File Offset: 0x000EBB1D
		public LandVehicle SpawnAndLoadVehicle(VehicleData data, string path, bool playerOwned)
		{
			LandVehicle landVehicle = this.SpawnAndReturnVehicle(data.VehicleCode, data.Position, data.Rotation, playerOwned);
			landVehicle.Load(data, path);
			return landVehicle;
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x000ED940 File Offset: 0x000EBB40
		public void LoadVehicle(VehicleData data, string path)
		{
			LandVehicle @object = GUIDManager.GetObject<LandVehicle>(new Guid(data.GUID));
			if (@object == null)
			{
				Console.LogError("LoadVehicle: Vehicle not found with GUID " + data.GUID, null);
				return;
			}
			@object.Load(data, path);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x000ED988 File Offset: 0x000EBB88
		public virtual string GetSaveString()
		{
			List<VehicleData> list = new List<VehicleData>();
			for (int i = 0; i < this.PlayerOwnedVehicles.Count; i++)
			{
				list.Add(this.PlayerOwnedVehicles[i].GetVehicleData());
			}
			return new VehicleCollectionData(list.ToArray()).GetJson(true);
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x000ED9DC File Offset: 0x000EBBDC
		public void SpawnLoanSharkVehicle(Vector3 position, Quaternion rot)
		{
			LandVehicle landVehicle = NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle("shitbox", position, rot, true);
			this.EnableLoanSharkVisuals(landVehicle.NetworkObject);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000EDA08 File Offset: 0x000EBC08
		[ObserversRpc(RunLocally = true)]
		private void EnableLoanSharkVisuals(NetworkObject veh)
		{
			this.RpcWriter___Observers_EnableLoanSharkVisuals_3323014238(veh);
			this.RpcLogic___EnableLoanSharkVisuals_3323014238(veh);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000EDA7C File Offset: 0x000EBC7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SpawnVehicle_3323115898));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_EnableLoanSharkVisuals_3323014238));
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000EDACE File Offset: 0x000EBCCE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x000EDAE7 File Offset: 0x000EBCE7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x000EDAF8 File Offset: 0x000EBCF8
		private void RpcWriter___Server_SpawnVehicle_3323115898(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
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
			writer.WriteString(vehicleCode);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteBoolean(playerOwned);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x000EDBCB File Offset: 0x000EBDCB
		public void RpcLogic___SpawnVehicle_3323115898(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			this.SpawnAndReturnVehicle(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x000EDBDC File Offset: 0x000EBDDC
		private void RpcReader___Server_SpawnVehicle_3323115898(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string vehicleCode = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SpawnVehicle_3323115898(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000EDC48 File Offset: 0x000EBE48
		private void RpcWriter___Observers_EnableLoanSharkVisuals_3323014238(NetworkObject veh)
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
			writer.WriteNetworkObject(veh);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x000EDCFE File Offset: 0x000EBEFE
		private void RpcLogic___EnableLoanSharkVisuals_3323014238(NetworkObject veh)
		{
			if (veh == null)
			{
				Console.LogWarning("Vehicle not found", null);
				return;
			}
			veh.GetComponent<LoanSharkCarVisuals>().Configure(true, true);
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000EDD24 File Offset: 0x000EBF24
		private void RpcReader___Observers_EnableLoanSharkVisuals_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject veh = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableLoanSharkVisuals_3323014238(veh);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000EDD5F File Offset: 0x000EBF5F
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002824 RID: 10276
		public List<LandVehicle> AllVehicles = new List<LandVehicle>();

		// Token: 0x04002825 RID: 10277
		[Header("Vehicles")]
		public List<LandVehicle> VehiclePrefabs = new List<LandVehicle>();

		// Token: 0x04002826 RID: 10278
		public List<LandVehicle> PlayerOwnedVehicles = new List<LandVehicle>();

		// Token: 0x04002827 RID: 10279
		private VehiclesLoader loader = new VehiclesLoader();

		// Token: 0x0400282B RID: 10283
		private bool dll_Excuted;

		// Token: 0x0400282C RID: 10284
		private bool dll_Excuted;
	}
}
