using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Building;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000670 RID: 1648
	public class SurfaceItem : BuildableItem
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000B290B File Offset: 0x000B0B0B
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x000B2913 File Offset: 0x000B0B13
		public Surface ParentSurface { get; protected set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000B291C File Offset: 0x000B0B1C
		public float RotationIncrement { get; } = 45f;

		// Token: 0x06002B2B RID: 11051 RVA: 0x000B2924 File Offset: 0x000B0B24
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.SurfaceItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000B2938 File Offset: 0x000B0B38
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.Initialized && base.LocallyBuilt)
			{
				base.StartCoroutine(this.<OnStartClient>g__WaitForDataSend|12_0());
			}
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000B2960 File Offset: 0x000B0B60
		protected override void SendInitToClient(NetworkConnection conn)
		{
			this.InitializeSurfaceItem(conn, base.ItemInstance, base.GUID.ToString(), this.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000B29B3 File Offset: 0x000B0BB3
		[ServerRpc(RequireOwnership = false)]
		public void SendSurfaceItemData(ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			this.RpcWriter___Server_SendSurfaceItemData_2652836379(instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000B29D0 File Offset: 0x000B0BD0
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public virtual void InitializeSurfaceItem(NetworkConnection conn, ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_InitializeSurfaceItem_2932264618(conn, instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
				this.RpcLogic___InitializeSurfaceItem_2932264618(conn, instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
			}
			else
			{
				this.RpcWriter___Target_InitializeSurfaceItem_2932264618(conn, instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
			}
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000B2A44 File Offset: 0x000B0C44
		public virtual void InitializeSurfaceItem(ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			this.SetTransformData(parentSurfaceGUID, relativePosition, relativeRotation);
			if (this.ParentSurface == null)
			{
				this.DestroyItem(false);
				return;
			}
			Property parentProperty = this.ParentSurface.ParentProperty;
			if (parentProperty == null)
			{
				Console.LogError("Failed to find parent property for " + base.gameObject.name, null);
				return;
			}
			base.InitializeBuildableItem(instance, GUID, parentProperty.PropertyCode);
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000B2AB4 File Offset: 0x000B0CB4
		protected virtual void SetTransformData(string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			Surface @object = GUIDManager.GetObject<Surface>(new Guid(parentSurfaceGUID));
			if (@object == null)
			{
				Console.LogError("Failed to find parent surface for " + base.gameObject.name, null);
				return;
			}
			this.ParentSurface = @object;
			this.RelativePosition = relativePosition;
			this.RelativeRotation = relativeRotation;
			base.transform.position = @object.transform.TransformPoint(relativePosition);
			base.transform.rotation = @object.transform.rotation * relativeRotation;
			if (base.NetworkObject.IsSpawned)
			{
				base.transform.SetParent(this.ParentSurface.Container.transform);
				return;
			}
			base.StartCoroutine(this.<SetTransformData>g__Routine|17_0());
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000B2B70 File Offset: 0x000B0D70
		protected override Property GetProperty(Transform searchTransform = null)
		{
			return base.GetProperty(searchTransform);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000B2B7C File Offset: 0x000B0D7C
		public override BuildableItemData GetBaseData()
		{
			return new SurfaceItemData(base.GUID, base.ItemInstance, 25, this.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000B2C18 File Offset: 0x000B0E18
		[CompilerGenerated]
		private IEnumerator <OnStartClient>g__WaitForDataSend|12_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SendSurfaceItemData(base.ItemInstance, base.GUID.ToString(), this.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation);
			yield break;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000B2C27 File Offset: 0x000B0E27
		[CompilerGenerated]
		private IEnumerator <SetTransformData>g__Routine|17_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			base.transform.SetParent(this.ParentSurface.Container.transform);
			yield break;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000B2C38 File Offset: 0x000B0E38
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.SurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.SurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendSurfaceItemData_2652836379));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_InitializeSurfaceItem_2932264618));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_InitializeSurfaceItem_2932264618));
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000B2CA1 File Offset: 0x000B0EA1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.SurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.SurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000B2CBA File Offset: 0x000B0EBA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000B2CC8 File Offset: 0x000B0EC8
		private void RpcWriter___Server_SendSurfaceItemData_2652836379(ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentSurfaceGUID);
			writer.WriteVector3(relativePosition);
			writer.WriteQuaternion(relativeRotation, 1);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000B2DA8 File Offset: 0x000B0FA8
		public void RpcLogic___SendSurfaceItemData_2652836379(ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			this.InitializeSurfaceItem(null, instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000B2DB8 File Offset: 0x000B0FB8
		private void RpcReader___Server_SendSurfaceItemData_2652836379(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentSurfaceGUID = PooledReader0.ReadString();
			Vector3 relativePosition = PooledReader0.ReadVector3();
			Quaternion relativeRotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendSurfaceItemData_2652836379(instance, guid, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000B2E34 File Offset: 0x000B1034
		private void RpcWriter___Target_InitializeSurfaceItem_2932264618(NetworkConnection conn, ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentSurfaceGUID);
			writer.WriteVector3(relativePosition);
			writer.WriteQuaternion(relativeRotation, 1);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000B2F22 File Offset: 0x000B1122
		public virtual void RpcLogic___InitializeSurfaceItem_2932264618(NetworkConnection conn, ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
		{
			this.InitializeSurfaceItem(instance, GUID, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000B2F34 File Offset: 0x000B1134
		private void RpcReader___Target_InitializeSurfaceItem_2932264618(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentSurfaceGUID = PooledReader0.ReadString();
			Vector3 relativePosition = PooledReader0.ReadVector3();
			Quaternion relativeRotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InitializeSurfaceItem_2932264618(base.LocalConnection, instance, guid, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000B2FB4 File Offset: 0x000B11B4
		private void RpcWriter___Observers_InitializeSurfaceItem_2932264618(NetworkConnection conn, ItemInstance instance, string GUID, string parentSurfaceGUID, Vector3 relativePosition, Quaternion relativeRotation)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentSurfaceGUID);
			writer.WriteVector3(relativePosition);
			writer.WriteQuaternion(relativeRotation, 1);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000B30A4 File Offset: 0x000B12A4
		private void RpcReader___Observers_InitializeSurfaceItem_2932264618(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentSurfaceGUID = PooledReader0.ReadString();
			Vector3 relativePosition = PooledReader0.ReadVector3();
			Quaternion relativeRotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___InitializeSurfaceItem_2932264618(null, instance, guid, parentSurfaceGUID, relativePosition, relativeRotation);
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000B3129 File Offset: 0x000B1329
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x04001F4E RID: 8014
		[Header("Settings")]
		public List<Surface.ESurfaceType> ValidSurfaceTypes = new List<Surface.ESurfaceType>
		{
			Surface.ESurfaceType.Wall,
			Surface.ESurfaceType.Roof
		};

		// Token: 0x04001F4F RID: 8015
		public bool AllowRotation = true;

		// Token: 0x04001F51 RID: 8017
		protected Vector3 RelativePosition = Vector3.zero;

		// Token: 0x04001F52 RID: 8018
		protected Quaternion RelativeRotation = Quaternion.identity;

		// Token: 0x04001F53 RID: 8019
		private bool dll_Excuted;

		// Token: 0x04001F54 RID: 8020
		private bool dll_Excuted;
	}
}
