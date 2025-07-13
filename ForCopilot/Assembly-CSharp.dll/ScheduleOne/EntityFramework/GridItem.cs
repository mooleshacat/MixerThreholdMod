using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000663 RID: 1635
	public class GridItem : BuildableItem
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000B090F File Offset: 0x000AEB0F
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].footprintTile;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000B0922 File Offset: 0x000AEB22
		public int FootprintX
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.x descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.x + 1;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000B095F File Offset: 0x000AEB5F
		public int FootprintY
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.y descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.y + 1;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x000B099C File Offset: 0x000AEB9C
		// (set) Token: 0x06002AA4 RID: 10916 RVA: 0x000B09A4 File Offset: 0x000AEBA4
		public Grid OwnerGrid { get; protected set; }

		// Token: 0x06002AA5 RID: 10917 RVA: 0x000B09AD File Offset: 0x000AEBAD
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.GridItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000B09C1 File Offset: 0x000AEBC1
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.Initialized && base.LocallyBuilt)
			{
				base.StartCoroutine(this.<OnStartClient>g__WaitForDataSend|18_0());
			}
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x000B09E8 File Offset: 0x000AEBE8
		protected override void SendInitToClient(NetworkConnection conn)
		{
			this.InitializeGridItem(conn, base.ItemInstance, this.OwnerGridGUID.ToString(), this.OriginCoordinate, this.Rotation, base.GUID.ToString());
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000B0A33 File Offset: 0x000AEC33
		[ServerRpc(RequireOwnership = false)]
		public void SendGridItemData(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.RpcWriter___Server_SendGridItemData_2821640832(instance, gridGUID, originCoordinate, rotation, GUID);
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000B0A50 File Offset: 0x000AEC50
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public virtual void InitializeGridItem(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
				this.RpcLogic___InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
			}
			else
			{
				this.RpcWriter___Target_InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000B0AC1 File Offset: 0x000AECC1
		public virtual void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (base.Initialized)
			{
				return;
			}
			base.InitializeBuildableItem(instance, GUID, this.GetProperty(grid.transform).PropertyCode);
			this.SetGridData(grid.GUID, originCoordinate, rotation);
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000B0AF8 File Offset: 0x000AECF8
		protected void SetGridData(Guid gridGUID, Vector2 originCoordinate, int rotation)
		{
			Grid @object = GUIDManager.GetObject<Grid>(gridGUID);
			if (@object == null)
			{
				Console.LogError("InitializeConstructable_GridBased: grid is null", null);
				this.DestroyItem(true);
				return;
			}
			this.OwnerGridGUID = gridGUID;
			this.OwnerGrid = @object;
			this.OriginCoordinate = originCoordinate;
			this.Rotation = this.ValidateRotation(rotation);
			this.ProcessGridData();
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000B0B50 File Offset: 0x000AED50
		private int ValidateRotation(int rotation)
		{
			if (float.IsNaN((float)rotation) || float.IsInfinity((float)rotation))
			{
				Console.LogWarning("Invalid rotation value: " + rotation.ToString() + " resetting to 0", null);
				return 0;
			}
			if (rotation != 0 && rotation != 90 && rotation != 180 && rotation != 270)
			{
				return Mathf.RoundToInt((float)(rotation / 90)) * 90;
			}
			return rotation;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000B0BB4 File Offset: 0x000AEDB4
		private void ProcessGridData()
		{
			this.OwnerGrid = GUIDManager.GetObject<Grid>(this.OwnerGridGUID);
			if (this.OwnerGrid == null)
			{
				Console.LogWarning("GridItem OwnerGrid is null", null);
				return;
			}
			base.ParentProperty = this.GetProperty(this.OwnerGrid.transform);
			if (base.NetworkObject.IsSpawned)
			{
				base.transform.SetParent(this.OwnerGrid.Container);
			}
			else
			{
				base.StartCoroutine(this.<ProcessGridData>g__Routine|25_0());
			}
			List<CoordinatePair> list = Coordinate.BuildCoordinateMatches(new Coordinate(this.OriginCoordinate), this.FootprintX, this.FootprintY, (float)this.Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.OwnerGrid.GetTile(list[i].coord2) == null)
				{
					string str = "ReceiveData: grid does not contain tile at ";
					Coordinate coord = list[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null), null);
					this.DestroyItem(true);
					return;
				}
			}
			this.ClearPositionData();
			this.CoordinatePairs.AddRange(list);
			this.RefreshTransform();
			for (int j = 0; j < this.CoordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.CoordinatePairs[j].coord2).AddOccupant(this, this.GetFootprintTile(this.CoordinatePairs[j].coord1));
				this.GetFootprintTile(this.CoordinatePairs[j].coord1).Initialize(this.OwnerGrid.GetTile(this.CoordinatePairs[j].coord2));
			}
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000B0D5C File Offset: 0x000AEF5C
		private void RefreshTransform()
		{
			base.transform.rotation = this.OwnerGrid.transform.rotation * (Quaternion.Inverse(this.BuildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.BuildPoint.up, (float)this.Rotation);
			base.transform.position = this.OwnerGrid.GetTile(this.CoordinatePairs[0].coord2).transform.position - (this.OriginFootprint.transform.position - base.transform.position);
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000B0E24 File Offset: 0x000AF024
		private void ClearPositionData()
		{
			if (this.OwnerGrid != null)
			{
				for (int i = 0; i < this.CoordinatePairs.Count; i++)
				{
					this.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).RemoveOccupant(this, this.GetFootprintTile(this.CoordinatePairs[i].coord1));
				}
			}
			this.CoordinatePairs.Clear();
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000B0E99 File Offset: 0x000AF099
		public override void DestroyItem(bool callOnServer = true)
		{
			this.ClearPositionData();
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x000B0EA8 File Offset: 0x000AF0A8
		public virtual void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000B0EE8 File Offset: 0x000AF0E8
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000B0F28 File Offset: 0x000AF128
		public FootprintTile GetFootprintTile(Coordinate coord)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				if (this.CoordinateFootprintTilePairs[i].coord.Equals(coord))
				{
					return this.CoordinateFootprintTilePairs[i].footprintTile;
				}
			}
			return null;
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x000B0F78 File Offset: 0x000AF178
		public Tile GetParentTileAtFootprintCoordinate(Coordinate footprintCoord)
		{
			return this.OwnerGrid.GetTile(this.CoordinatePairs.Find((CoordinatePair x) => x.coord1 == footprintCoord).coord2);
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x000B0FBC File Offset: 0x000AF1BC
		public virtual bool CanShareTileWith(List<GridItem> obstacles)
		{
			for (int i = 0; i < obstacles.Count; i++)
			{
				if (!(obstacles[i] is FloorRack))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x000B0FEB File Offset: 0x000AF1EB
		public override BuildableItemData GetBaseData()
		{
			return new GridItemData(base.GUID, base.ItemInstance, 0, this.OwnerGrid, this.OriginCoordinate, this.Rotation);
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000B102F File Offset: 0x000AF22F
		[CompilerGenerated]
		private IEnumerator <OnStartClient>g__WaitForDataSend|18_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SendGridItemData(base.ItemInstance, this.OwnerGridGUID.ToString(), this.OriginCoordinate, this.Rotation, base.GUID.ToString());
			yield break;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000B104B File Offset: 0x000AF24B
		[CompilerGenerated]
		private IEnumerator <ProcessGridData>g__Routine|25_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			base.transform.SetParent(this.OwnerGrid.Container);
			yield break;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000B105C File Offset: 0x000AF25C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendGridItemData_2821640832));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_InitializeGridItem_1883577149));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_InitializeGridItem_1883577149));
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000B10C5 File Offset: 0x000AF2C5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000B10DE File Offset: 0x000AF2DE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000B10EC File Offset: 0x000AF2EC
		private void RpcWriter___Server_SendGridItemData_2821640832(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
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
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, 1);
			writer.WriteString(GUID);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000B11CC File Offset: 0x000AF3CC
		public void RpcLogic___SendGridItemData_2821640832(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.InitializeGridItem(null, instance, gridGUID, originCoordinate, rotation, GUID);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000B11DC File Offset: 0x000AF3DC
		private void RpcReader___Server_SendGridItemData_2821640832(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(1);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendGridItemData_2821640832(instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000B1258 File Offset: 0x000AF458
		private void RpcWriter___Target_InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
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
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, 1);
			writer.WriteString(GUID);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000B1346 File Offset: 0x000AF546
		public virtual void RpcLogic___InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.InitializeGridItem(instance, GUIDManager.GetObject<Grid>(new Guid(gridGUID)), originCoordinate, rotation, GUID);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000B1360 File Offset: 0x000AF560
		private void RpcReader___Target_InitializeGridItem_1883577149(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(1);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InitializeGridItem_1883577149(base.LocalConnection, instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000B13E0 File Offset: 0x000AF5E0
		private void RpcWriter___Observers_InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
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
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, 1);
			writer.WriteString(GUID);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000B14D0 File Offset: 0x000AF6D0
		private void RpcReader___Observers_InitializeGridItem_1883577149(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(1);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___InitializeGridItem_1883577149(null, instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000B1555 File Offset: 0x000AF755
		protected override void dll()
		{
			base.Awake();
			this.BoundingCollider.isTrigger = true;
			this.BoundingCollider.gameObject.layer = LayerMask.NameToLayer("Invisible");
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x04001F1E RID: 7966
		[Header("Grid item data")]
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04001F1F RID: 7967
		public GridItem.EGridType GridType;

		// Token: 0x04001F21 RID: 7969
		public Guid OwnerGridGUID;

		// Token: 0x04001F22 RID: 7970
		public Vector2 OriginCoordinate;

		// Token: 0x04001F23 RID: 7971
		public int Rotation;

		// Token: 0x04001F24 RID: 7972
		public List<CoordinatePair> CoordinatePairs = new List<CoordinatePair>();

		// Token: 0x04001F25 RID: 7973
		private bool dll_Excuted;

		// Token: 0x04001F26 RID: 7974
		private bool dll_Excuted;

		// Token: 0x02000664 RID: 1636
		public enum EGridType
		{
			// Token: 0x04001F28 RID: 7976
			All,
			// Token: 0x04001F29 RID: 7977
			IndoorOnly,
			// Token: 0x04001F2A RID: 7978
			OutdoorOnly
		}
	}
}
