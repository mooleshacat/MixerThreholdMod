using System;
using System.Collections;
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
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x0200096C RID: 2412
	public class Constructable_GridBased : Constructable
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06004137 RID: 16695 RVA: 0x001138AC File Offset: 0x00111AAC
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].footprintTile;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06004138 RID: 16696 RVA: 0x001138BF File Offset: 0x00111ABF
		public int FootprintX
		{
			get
			{
				return this.CoordinateFootprintTilePairs[this.CoordinateFootprintTilePairs.Count - 1].coord.x + 1;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x001138E5 File Offset: 0x00111AE5
		public int FootprintY
		{
			get
			{
				return this.CoordinateFootprintTilePairs[this.CoordinateFootprintTilePairs.Count - 1].coord.y + 1;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool hasWaterSupply
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x0011390B File Offset: 0x00111B0B
		public PowerNode PowerNode
		{
			get
			{
				return this.powerNode;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x00113913 File Offset: 0x00111B13
		public bool isPowered
		{
			get
			{
				return this.AlwaysPowered || this.powerNode.isConnectedToPower;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600413D RID: 16701 RVA: 0x0011392A File Offset: 0x00111B2A
		// (set) Token: 0x0600413E RID: 16702 RVA: 0x00113932 File Offset: 0x00111B32
		public Grid OwnerGrid { get; protected set; }

		// Token: 0x0600413F RID: 16703 RVA: 0x0011393C File Offset: 0x00111B3C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ConstructableScripts.Constructable_GridBased_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x0011395B File Offset: 0x00111B5B
		public override void OnStartServer()
		{
			base.OnStartServer();
			Console.Log("On start server", null);
			this.GenerateGridGUIDs();
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00113974 File Offset: 0x00111B74
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			Console.Log("On spawn server", null);
			if (!connection.IsLocalClient)
			{
				Console.Log("Sending thingys", null);
				this.SetGridGUIDs(connection, this.GetGridGUIDs());
			}
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x001139A8 File Offset: 0x00111BA8
		public override void OnStartNetwork()
		{
			base.OnStartNetwork();
			Console.Log("OnStartNetwork", null);
			this.ReceiveData();
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x001139C1 File Offset: 0x00111BC1
		public virtual void InitializeConstructable_GridBased(Grid grid, Vector2 originCoordinate, float rotation)
		{
			this.SetData(grid.GUID, originCoordinate, rotation);
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x001139D4 File Offset: 0x00111BD4
		private void ReceiveData()
		{
			if (base.IsStatic)
			{
				return;
			}
			Console.Log("Constructable received data", null);
			this.OwnerGrid = GUIDManager.GetObject<Grid>(this.SyncAccessor_OwnerGridGUID);
			bool flag = false;
			if (base.NetworkObject.IsSpawned)
			{
				this.SetParent(this.OwnerGrid.Container);
				flag = true;
			}
			List<CoordinatePair> list = Coordinate.BuildCoordinateMatches(new Coordinate(this.SyncAccessor_OriginCoordinate), this.FootprintX, this.FootprintY, this.SyncAccessor_Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.OwnerGrid.GetTile(list[i].coord2) == null)
				{
					string str = "InitializeConstructable_GridBased: grid does not contain tile at ";
					Coordinate coord = list[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null), null);
					this.DestroyConstructable(true);
					return;
				}
			}
			this.ClearPositionData();
			this.coordinatePairs.AddRange(list);
			this.RefreshTransform();
			for (int j = 0; j < this.coordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[j].coord2).AddOccupant(this, this.GetFootprintTile(this.coordinatePairs[j].coord1));
			}
			if (!flag)
			{
				base.StartCoroutine(this.<ReceiveData>g__Routine|36_0());
			}
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00113B22 File Offset: 0x00111D22
		private void SetParent(Transform parent)
		{
			base.transform.SetParent(parent);
			this.ContentContainer.SetParent(parent);
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00113B3C File Offset: 0x00111D3C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			this.RpcWriter___Server_SetData_810381718(gridGUID, originCoordinate, rotation);
			this.RpcLogic___SetData_810381718(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00113B6D File Offset: 0x00111D6D
		public virtual void RepositionConstructable(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			this.SetData(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00113B78 File Offset: 0x00111D78
		private void RefreshTransform()
		{
			base.transform.rotation = this.OwnerGrid.transform.rotation * (Quaternion.Inverse(this.buildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.buildPoint.up, this.SyncAccessor_Rotation);
			base.transform.position = this.OwnerGrid.GetTile(this.coordinatePairs[0].coord2).transform.position - (this.OriginFootprint.transform.position - base.transform.position);
			this.ContentContainer.transform.position = base.transform.position;
			this.ContentContainer.transform.rotation = base.transform.rotation;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00113C74 File Offset: 0x00111E74
		private void ClearPositionData()
		{
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[i].coord2).RemoveOccupant(this, this.GetFootprintTile(this.coordinatePairs[i].coord1));
			}
			this.coordinatePairs.Clear();
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00113CDC File Offset: 0x00111EDC
		public override void DestroyConstructable(bool callOnServer = true)
		{
			Grid[] componentsInChildren = base.gameObject.GetComponentsInChildren<Grid>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DestroyGrid();
			}
			for (int j = 0; j < this.coordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[j].coord2).RemoveOccupant(this, this.GetFootprintTile(this.coordinatePairs[j].coord1));
			}
			base.DestroyConstructable(callOnServer);
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00113D64 File Offset: 0x00111F64
		private void GenerateGridGUIDs()
		{
			for (int i = 0; i < this.Grids.Length; i++)
			{
				((IGUIDRegisterable)this.Grids[i]).SetGUID(GUIDManager.GenerateUniqueGUID());
				Console.LogError("Generated GRID GUID: " + this.Grids[i].GUID.ToString(), null);
			}
			Console.Log("Sending GRID GUIDs", null);
			this.SetGridGUIDs(null, this.GetGridGUIDs());
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00113DDC File Offset: 0x00111FDC
		private string[] GetGridGUIDs()
		{
			string[] array = new string[this.Grids.Length];
			for (int i = 0; i < this.Grids.Length; i++)
			{
				array[i] = this.Grids[i].GUID.ToString();
			}
			return array;
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00113E2C File Offset: 0x0011202C
		[ObserversRpc]
		[TargetRpc]
		protected void SetGridGUIDs(NetworkConnection target, string[] guids)
		{
			if (target == null)
			{
				this.RpcWriter___Observers_SetGridGUIDs_2890081366(target, guids);
			}
			else
			{
				this.RpcWriter___Target_SetGridGUIDs_2890081366(target, guids);
			}
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x00113E60 File Offset: 0x00112060
		public override void SetInvisible()
		{
			base.SetInvisible();
			if (this.PowerNode != null)
			{
				for (int i = 0; i < this.PowerNode.connections.Count; i++)
				{
					this.PowerNode.connections[i].SetVisible(false);
				}
			}
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00113EB4 File Offset: 0x001120B4
		public override void RestoreVisibility()
		{
			base.RestoreVisibility();
			if (this.PowerNode != null)
			{
				for (int i = 0; i < this.PowerNode.connections.Count; i++)
				{
					this.PowerNode.connections[i].SetVisible(true);
				}
			}
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x00113F08 File Offset: 0x00112108
		public virtual void SetRoofVisible(bool vis)
		{
			if (this.roofVisible == vis)
			{
				return;
			}
			this.roofVisible = vis;
			if (this.roofVisible)
			{
				using (List<GameObject>.Enumerator enumerator = this.roofObjectsForVisibility.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						if (this.originalRoofLayers.ContainsKey(gameObject))
						{
							gameObject.layer = this.originalRoofLayers[gameObject];
						}
						else
						{
							gameObject.layer = LayerMask.NameToLayer("Default");
						}
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.roofObjectsForVisibility)
			{
				if (gameObject2.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					if (this.originalRoofLayers.ContainsKey(gameObject2))
					{
						this.originalRoofLayers[gameObject2] = gameObject2.layer;
					}
					else
					{
						this.originalRoofLayers.Add(gameObject2, gameObject2.layer);
					}
				}
				gameObject2.layer = LayerMask.NameToLayer("Invisible");
			}
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x00114048 File Offset: 0x00112248
		public void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00114088 File Offset: 0x00112288
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x001140C8 File Offset: 0x001122C8
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

		// Token: 0x06004154 RID: 16724 RVA: 0x00114118 File Offset: 0x00112318
		public List<FootprintTile> GetFootprintTiles()
		{
			List<FootprintTile> list = new List<FootprintTile>();
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				list.Add(this.CoordinateFootprintTilePairs[i].footprintTile);
			}
			return list;
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00114194 File Offset: 0x00112394
		[CompilerGenerated]
		private IEnumerator <ReceiveData>g__Routine|36_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SetParent(this.OwnerGrid.Container);
			yield break;
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x001141A4 File Offset: 0x001123A4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___Rotation = new SyncVar<float>(this, 2U, 0, 0, -1f, 0, this.Rotation);
			this.syncVar___OriginCoordinate = new SyncVar<Vector2>(this, 1U, 0, 0, -1f, 0, this.OriginCoordinate);
			this.syncVar___OwnerGridGUID = new SyncVar<Guid>(this, 0U, 0, 0, -1f, 0, this.OwnerGridGUID);
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetData_810381718));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetGridGUIDs_2890081366));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetGridGUIDs_2890081366));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ConstructableScripts.Constructable_GridBased));
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x001142A0 File Offset: 0x001124A0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___Rotation.SetRegistered();
			this.syncVar___OriginCoordinate.SetRegistered();
			this.syncVar___OwnerGridGUID.SetRegistered();
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x001142DA File Offset: 0x001124DA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x001142E8 File Offset: 0x001124E8
		private void RpcWriter___Server_SetData_810381718(Guid gridGUID, Vector2 originCoordinate, float rotation)
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
			writer.WriteGuidAllocated(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteSingle(rotation, 0);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x001143B0 File Offset: 0x001125B0
		protected virtual void RpcLogic___SetData_810381718(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			Console.Log("SetData", null);
			Grid @object = GUIDManager.GetObject<Grid>(gridGUID);
			if (@object == null)
			{
				Console.LogError("InitializeConstructable_GridBased: grid is null", null);
				this.DestroyConstructable(true);
				return;
			}
			this.sync___set_value_OwnerGridGUID(gridGUID, true);
			this.OwnerGrid = @object;
			this.sync___set_value_OriginCoordinate(originCoordinate, true);
			this.sync___set_value_Rotation(rotation, true);
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0011440C File Offset: 0x0011260C
		private void RpcReader___Server_SetData_810381718(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Guid gridGUID = PooledReader0.ReadGuid();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_810381718(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00114474 File Offset: 0x00112674
		private void RpcWriter___Observers_SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
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
			writer.Write___System.String[]FishNet.Serializing.Generated(guids);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0011452C File Offset: 0x0011272C
		protected void RpcLogic___SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
		{
			Console.Log("Setting GRID GUIDs", null);
			for (int i = 0; i < guids.Length; i++)
			{
				((IGUIDRegisterable)this.Grids[i]).SetGUID(new Guid(guids[i]));
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00114568 File Offset: 0x00112768
		private void RpcReader___Observers_SetGridGUIDs_2890081366(PooledReader PooledReader0, Channel channel)
		{
			string[] guids = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGridGUIDs_2890081366(null, guids);
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0011459C File Offset: 0x0011279C
		private void RpcWriter___Target_SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
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
			writer.Write___System.String[]FishNet.Serializing.Generated(guids);
			base.SendTargetRpc(4U, writer, channel, 0, target, false, true);
			writer.Store();
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00114654 File Offset: 0x00112854
		private void RpcReader___Target_SetGridGUIDs_2890081366(PooledReader PooledReader0, Channel channel)
		{
			string[] guids = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGridGUIDs_2890081366(base.LocalConnection, guids);
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06004163 RID: 16739 RVA: 0x0011468B File Offset: 0x0011288B
		// (set) Token: 0x06004164 RID: 16740 RVA: 0x00114693 File Offset: 0x00112893
		public Guid SyncAccessor_OwnerGridGUID
		{
			get
			{
				return this.OwnerGridGUID;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.OwnerGridGUID = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___OwnerGridGUID.SetValue(value, value);
				}
			}
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x001146D0 File Offset: 0x001128D0
		public override bool Constructable_GridBased(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_Rotation(this.syncVar___Rotation.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(0);
				this.sync___set_value_Rotation(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_OriginCoordinate(this.syncVar___OriginCoordinate.GetValue(true), true);
					return true;
				}
				Vector2 value2 = PooledReader0.ReadVector2();
				this.sync___set_value_OriginCoordinate(value2, Boolean2);
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
					this.sync___set_value_OwnerGridGUID(this.syncVar___OwnerGridGUID.GetValue(true), true);
					return true;
				}
				Guid value3 = PooledReader0.ReadGuid();
				this.sync___set_value_OwnerGridGUID(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x001147AF File Offset: 0x001129AF
		// (set) Token: 0x06004167 RID: 16743 RVA: 0x001147B7 File Offset: 0x001129B7
		public Vector2 SyncAccessor_OriginCoordinate
		{
			get
			{
				return this.OriginCoordinate;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.OriginCoordinate = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___OriginCoordinate.SetValue(value, value);
				}
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x001147F3 File Offset: 0x001129F3
		// (set) Token: 0x06004169 RID: 16745 RVA: 0x001147FB File Offset: 0x001129FB
		public float SyncAccessor_Rotation
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

		// Token: 0x0600416A RID: 16746 RVA: 0x00114838 File Offset: 0x00112A38
		protected override void dll()
		{
			base.Awake();
			bool isStatic = base.IsStatic;
			if (this.Grids.Length != base.GetComponentsInChildren<Grid>().Length)
			{
				Console.LogWarning(string.Concat(new string[]
				{
					base.gameObject.name,
					": Grids array length does not match number of child grids! (Grids array length: ",
					this.Grids.Length.ToString(),
					", child grids: ",
					base.GetComponentsInChildren<Grid>().Length.ToString(),
					")"
				}), null);
			}
		}

		// Token: 0x04002E88 RID: 11912
		[Header("Grid Based Constructable References")]
		public Transform buildPoint;

		// Token: 0x04002E89 RID: 11913
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04002E8A RID: 11914
		public Transform ContentContainer;

		// Token: 0x04002E8B RID: 11915
		public Grid[] Grids;

		// Token: 0x04002E8C RID: 11916
		public List<GameObject> roofObjectsForVisibility = new List<GameObject>();

		// Token: 0x04002E8D RID: 11917
		[Header("Power")]
		[SerializeField]
		protected bool AlwaysPowered;

		// Token: 0x04002E8E RID: 11918
		[SerializeField]
		protected PowerNode powerNode;

		// Token: 0x04002E8F RID: 11919
		[HideInInspector]
		public bool isGhost;

		// Token: 0x04002E90 RID: 11920
		protected bool dataChangedThisFrame;

		// Token: 0x04002E92 RID: 11922
		[SyncVar]
		public Guid OwnerGridGUID;

		// Token: 0x04002E93 RID: 11923
		[SyncVar]
		public Vector2 OriginCoordinate;

		// Token: 0x04002E94 RID: 11924
		[SyncVar]
		public float Rotation;

		// Token: 0x04002E95 RID: 11925
		public List<CoordinatePair> coordinatePairs = new List<CoordinatePair>();

		// Token: 0x04002E96 RID: 11926
		private Dictionary<GameObject, LayerMask> originalRoofLayers = new Dictionary<GameObject, LayerMask>();

		// Token: 0x04002E97 RID: 11927
		protected bool roofVisible = true;

		// Token: 0x04002E98 RID: 11928
		public SyncVar<Guid> syncVar___OwnerGridGUID;

		// Token: 0x04002E99 RID: 11929
		public SyncVar<Vector2> syncVar___OriginCoordinate;

		// Token: 0x04002E9A RID: 11930
		public SyncVar<float> syncVar___Rotation;

		// Token: 0x04002E9B RID: 11931
		private bool dll_Excuted;

		// Token: 0x04002E9C RID: 11932
		private bool dll_Excuted;
	}
}
