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
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x0200066B RID: 1643
	public class ProceduralGridItem : BuildableItem
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000B1B9B File Offset: 0x000AFD9B
		public int FootprintXSize
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.x descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.x + 1;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000B1BD8 File Offset: 0x000AFDD8
		public int FootprintYSize
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.y descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.y + 1;
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000B1C15 File Offset: 0x000AFE15
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.ProceduralGridItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000B1C29 File Offset: 0x000AFE29
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.Initialized && base.LocallyBuilt)
			{
				base.StartCoroutine(this.<OnStartClient>g__WaitForDataSend|10_0());
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000B1C50 File Offset: 0x000AFE50
		protected override void SendInitToClient(NetworkConnection conn)
		{
			this.InitializeProceduralGridItem(conn, base.ItemInstance, this.SyncAccessor_Rotation, this.SyncAccessor_footprintTileMatches, base.GUID.ToString());
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000B1C8A File Offset: 0x000AFE8A
		[ServerRpc(RequireOwnership = false)]
		public void SendProceduralGridItemData(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.RpcWriter___Server_SendProceduralGridItemData_638911643(instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000B1CA4 File Offset: 0x000AFEA4
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public virtual void InitializeProceduralGridItem(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
				this.RpcLogic___InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
			}
			else
			{
				this.RpcWriter___Target_InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
			}
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000B1D0C File Offset: 0x000AFF0C
		public virtual void InitializeProceduralGridItem(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			if (_footprintTileMatches.Count == 0)
			{
				Console.LogError(base.gameObject.name + " initialized with zero footprint tile matches!", null);
				return;
			}
			this.SetProceduralGridData(_rotation, _footprintTileMatches);
			NetworkObject tileParent = _footprintTileMatches[0].tileParent;
			if (tileParent == null)
			{
				Console.LogError("Base object is null for " + base.gameObject.name, null);
				return;
			}
			Property property = this.GetProperty(tileParent.transform);
			if (property == null)
			{
				Console.LogError("Failed to find property from base " + tileParent.gameObject.name, null);
				return;
			}
			base.InitializeBuildableItem(instance, GUID, property.PropertyCode);
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000B1DB8 File Offset: 0x000AFFB8
		protected virtual void SetProceduralGridData(int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches)
		{
			this.sync___set_value_Rotation(_rotation, true);
			this.sync___set_value_footprintTileMatches(_footprintTileMatches, true);
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				_footprintTileMatches[i].tile.AddOccupant(this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[i].coord), this);
			}
			if (base.NetworkObject.IsSpawned)
			{
				base.transform.SetParent(this.SyncAccessor_footprintTileMatches[0].tile.ParentBuildableItem.transform.parent);
				this.RefreshTransform();
				return;
			}
			base.StartCoroutine(this.<SetProceduralGridData>g__Routine|15_0());
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000B1E68 File Offset: 0x000B0068
		private void RefreshTransform()
		{
			ProceduralTile tile = this.SyncAccessor_footprintTileMatches[0].tile;
			base.transform.forward = tile.transform.forward;
			base.transform.Rotate(tile.transform.up, (float)this.SyncAccessor_Rotation);
			base.transform.position = tile.transform.position - (this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[0].coord).transform.position - base.transform.position);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000B1F0C File Offset: 0x000B010C
		private void ClearPositionData()
		{
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				this.SyncAccessor_footprintTileMatches[i].tile.RemoveOccupant(this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[i].coord), this);
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000B1F60 File Offset: 0x000B0160
		public override void DestroyItem(bool callOnServer = true)
		{
			this.ClearPositionData();
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000B1F6F File Offset: 0x000B016F
		protected override Property GetProperty(Transform searchTransform = null)
		{
			if (searchTransform != null && searchTransform.GetComponent<GridItem>() != null)
			{
				return searchTransform.GetComponent<GridItem>().ParentProperty;
			}
			return base.GetProperty(searchTransform);
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000B1F9C File Offset: 0x000B019C
		public virtual void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000B1FDC File Offset: 0x000B01DC
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000B201C File Offset: 0x000B021C
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

		// Token: 0x06002B00 RID: 11008 RVA: 0x000B206C File Offset: 0x000B026C
		public override BuildableItemData GetBaseData()
		{
			FootprintMatchData[] array = new FootprintMatchData[this.SyncAccessor_footprintTileMatches.Count];
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				string tileOwnerGUID = ((IGUIDRegisterable)this.SyncAccessor_footprintTileMatches[i].tileParent.GetComponent<BuildableItem>()).GUID.ToString();
				int tileIndex = this.SyncAccessor_footprintTileMatches[i].tileIndex;
				Vector2 footprintCoordinate = new Vector2((float)this.SyncAccessor_footprintTileMatches[i].coord.x, (float)this.SyncAccessor_footprintTileMatches[i].coord.y);
				array[i] = new FootprintMatchData(tileOwnerGUID, tileIndex, footprintCoordinate);
			}
			return new ProceduralGridItemData(base.GUID, base.ItemInstance, 50, this.SyncAccessor_Rotation, array);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000B215C File Offset: 0x000B035C
		[CompilerGenerated]
		private IEnumerator <OnStartClient>g__WaitForDataSend|10_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SendProceduralGridItemData(base.ItemInstance, this.SyncAccessor_Rotation, this.SyncAccessor_footprintTileMatches, base.GUID.ToString());
			yield break;
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000B216B File Offset: 0x000B036B
		[CompilerGenerated]
		private IEnumerator <SetProceduralGridData>g__Routine|15_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			base.transform.SetParent(this.SyncAccessor_footprintTileMatches[0].tile.ParentBuildableItem.transform.parent);
			this.RefreshTransform();
			yield break;
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000B217C File Offset: 0x000B037C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___footprintTileMatches = new SyncVar<List<CoordinateProceduralTilePair>>(this, 1U, 0, 0, -1f, 0, this.footprintTileMatches);
			this.syncVar___Rotation = new SyncVar<int>(this, 0U, 0, 0, -1f, 0, this.Rotation);
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendProceduralGridItemData_638911643));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_InitializeProceduralGridItem_3164718044));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_InitializeProceduralGridItem_3164718044));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.EntityFramework.ProceduralGridItem));
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000B224D File Offset: 0x000B044D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___footprintTileMatches.SetRegistered();
			this.syncVar___Rotation.SetRegistered();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000B227C File Offset: 0x000B047C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000B228C File Offset: 0x000B048C
		private void RpcWriter___Server_SendProceduralGridItemData_638911643(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, 1);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000B235F File Offset: 0x000B055F
		public void RpcLogic___SendProceduralGridItemData_638911643(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.InitializeProceduralGridItem(null, instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000B2370 File Offset: 0x000B0570
		private void RpcReader___Server_SendProceduralGridItemData_638911643(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(1);
			List<CoordinateProceduralTilePair> list = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendProceduralGridItemData_638911643(instance, rotation, list, guid);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000B23DC File Offset: 0x000B05DC
		private void RpcWriter___Target_InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, 1);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000B24BD File Offset: 0x000B06BD
		public virtual void RpcLogic___InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.InitializeProceduralGridItem(instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000B24CC File Offset: 0x000B06CC
		private void RpcReader___Target_InitializeProceduralGridItem_3164718044(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(1);
			List<CoordinateProceduralTilePair> list = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InitializeProceduralGridItem_3164718044(base.LocalConnection, instance, rotation, list, guid);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000B253C File Offset: 0x000B073C
		private void RpcWriter___Observers_InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, 1);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000B2620 File Offset: 0x000B0820
		private void RpcReader___Observers_InitializeProceduralGridItem_3164718044(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(1);
			List<CoordinateProceduralTilePair> list = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___InitializeProceduralGridItem_3164718044(null, instance, rotation, list, guid);
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000B2694 File Offset: 0x000B0894
		// (set) Token: 0x06002B12 RID: 11026 RVA: 0x000B269C File Offset: 0x000B089C
		public int SyncAccessor_Rotation
		{
			get
			{
				return this.Rotation;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.Rotation = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___Rotation.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000B26D8 File Offset: 0x000B08D8
		public override bool ProceduralGridItem(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_footprintTileMatches(this.syncVar___footprintTileMatches.GetValue(true), true);
					return true;
				}
				List<CoordinateProceduralTilePair> value = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_footprintTileMatches(value, Boolean2);
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
					this.sync___set_value_Rotation(this.syncVar___Rotation.GetValue(true), true);
					return true;
				}
				int value2 = PooledReader0.ReadInt32(1);
				this.sync___set_value_Rotation(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000B2773 File Offset: 0x000B0973
		// (set) Token: 0x06002B15 RID: 11029 RVA: 0x000B277B File Offset: 0x000B097B
		public List<CoordinateProceduralTilePair> SyncAccessor_footprintTileMatches
		{
			get
			{
				return this.footprintTileMatches;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.footprintTileMatches = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___footprintTileMatches.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000B27B7 File Offset: 0x000B09B7
		protected override void dll()
		{
			base.Awake();
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x04001F3A RID: 7994
		[Header("Grid item data")]
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04001F3B RID: 7995
		public ProceduralTile.EProceduralTileType ProceduralTileType;

		// Token: 0x04001F3C RID: 7996
		[SyncVar]
		[HideInInspector]
		public int Rotation;

		// Token: 0x04001F3D RID: 7997
		[SyncVar]
		[HideInInspector]
		public List<CoordinateProceduralTilePair> footprintTileMatches = new List<CoordinateProceduralTilePair>();

		// Token: 0x04001F3E RID: 7998
		public SyncVar<int> syncVar___Rotation;

		// Token: 0x04001F3F RID: 7999
		public SyncVar<List<CoordinateProceduralTilePair>> syncVar___footprintTileMatches;

		// Token: 0x04001F40 RID: 8000
		private bool dll_Excuted;

		// Token: 0x04001F41 RID: 8001
		private bool dll_Excuted;

		// Token: 0x0200066C RID: 1644
		public class FootprintTileMatch
		{
			// Token: 0x04001F42 RID: 8002
			public FootprintTile footprint;

			// Token: 0x04001F43 RID: 8003
			public ProceduralTile matchedTile;
		}
	}
}
