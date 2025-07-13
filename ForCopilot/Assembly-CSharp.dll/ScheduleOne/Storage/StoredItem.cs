using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ScheduleOne.Storage
{
	// Token: 0x020008F0 RID: 2288
	public class StoredItem : MonoBehaviour
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x001059E5 File Offset: 0x00103BE5
		// (set) Token: 0x06003DF9 RID: 15865 RVA: 0x001059ED File Offset: 0x00103BED
		public StorableItemInstance item { get; protected set; }

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x001059F6 File Offset: 0x00103BF6
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x001059FE File Offset: 0x00103BFE
		public bool Destroyed { get; private set; }

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x00105A07 File Offset: 0x00103C07
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].tile;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x00105A1C File Offset: 0x00103C1C
		public int FootprintX
		{
			get
			{
				if (this.footprintX == -1)
				{
					this.footprintX = (from c in this.CoordinateFootprintTilePairs
					orderby c.coord.x descending
					select c).FirstOrDefault<CoordinateStorageFootprintTilePair>().coord.x + 1;
				}
				return this.footprintX;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x00105A7C File Offset: 0x00103C7C
		public int FootprintY
		{
			get
			{
				if (this.footprintY == -1)
				{
					this.footprintY = (from c in this.CoordinateFootprintTilePairs
					orderby c.coord.y descending
					select c).FirstOrDefault<CoordinateStorageFootprintTilePair>().coord.y + 1;
				}
				return this.footprintY;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x00105AD9 File Offset: 0x00103CD9
		// (set) Token: 0x06003E00 RID: 15872 RVA: 0x00105AE1 File Offset: 0x00103CE1
		public IStorageEntity parentStorageEntity { get; protected set; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x00105AEA File Offset: 0x00103CEA
		// (set) Token: 0x06003E02 RID: 15874 RVA: 0x00105AF2 File Offset: 0x00103CF2
		public StorageGrid parentGrid { get; protected set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x00105AFB File Offset: 0x00103CFB
		public List<CoordinatePair> CoordinatePairs
		{
			get
			{
				return this.coordinatePairs;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x00105B03 File Offset: 0x00103D03
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x00105B0B File Offset: 0x00103D0B
		public int totalArea
		{
			get
			{
				return this.CoordinateFootprintTilePairs.Count;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x00105B18 File Offset: 0x00103D18
		// (set) Token: 0x06003E07 RID: 15879 RVA: 0x00105B20 File Offset: 0x00103D20
		public bool canBePickedUp { get; protected set; } = true;

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x00105B29 File Offset: 0x00103D29
		// (set) Token: 0x06003E09 RID: 15881 RVA: 0x00105B31 File Offset: 0x00103D31
		public string noPickupReason { get; protected set; } = string.Empty;

		// Token: 0x06003E0A RID: 15882 RVA: 0x00105B3C File Offset: 0x00103D3C
		protected virtual void Awake()
		{
			MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					componentsInChildren[i].enabled = false;
				}
				else
				{
					componentsInChildren[i].shadowCastingMode = ShadowCastingMode.Off;
				}
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00105B80 File Offset: 0x00103D80
		protected virtual void OnValidate()
		{
			if (base.gameObject.layer != LayerMask.NameToLayer("StoredItem"))
			{
				StoredItem.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("StoredItem"));
			}
			if (this.CoordinateFootprintTilePairs.Count == 0)
			{
				Debug.LogWarning("StoredItem (" + base.gameObject.name + ") has no CoordinateFootprintTilePairs!");
			}
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00105BE8 File Offset: 0x00103DE8
		public virtual void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			if (grid == null)
			{
				Console.LogError("InitializeStoredItem: grid is null!", null);
				this.DestroyStoredItem();
				return;
			}
			if (this == null || base.gameObject == null)
			{
				return;
			}
			this.item = _item;
			this.parentGrid = grid;
			this.rotation = _rotation;
			StoredItem.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("StoredItem"));
			this.coordinatePairs = Coordinate.BuildCoordinateMatches(new Coordinate(_originCoordinate), this.FootprintX, this.FootprintY, this.Rotation);
			this.RefreshTransform();
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				StorageTile tile = this.parentGrid.GetTile(this.coordinatePairs[i].coord2);
				if (tile == null)
				{
					string str = "Failed to find tile at ";
					Coordinate coord = this.coordinatePairs[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null) + " when initializing stored item!", null);
					this.DestroyStoredItem();
					return;
				}
				if (tile.occupant != null)
				{
					this.DestroyStoredItem();
					return;
				}
				tile.SetOccupant(this);
				grid.freeTiles.Remove(tile);
			}
			this.intObj = base.GetComponentInChildren<InteractableObject>();
			if (this.intObj != null)
			{
				UnityEngine.Object.Destroy(this.intObj);
			}
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x00105D4C File Offset: 0x00103F4C
		private void RefreshTransform()
		{
			FootprintTile tile = this.GetTile(this.coordinatePairs[0].coord1);
			StorageTile tile2 = this.parentGrid.GetTile(this.coordinatePairs[0].coord2);
			base.transform.rotation = this.parentGrid.transform.rotation * (Quaternion.Inverse(this.buildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.buildPoint.up, this.rotation);
			base.transform.position = tile2.transform.position - (tile.transform.position - base.transform.position);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x00105E28 File Offset: 0x00104028
		protected virtual void InitializeIntObj()
		{
			this.intObj = base.GetComponentInChildren<InteractableObject>();
			if (this.intObj == null)
			{
				this.intObj = base.gameObject.AddComponent<InteractableObject>();
			}
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00105E9C File Offset: 0x0010409C
		public virtual void Destroy_Internal()
		{
			this.Destroyed = true;
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				this.parentGrid.GetTile(this.coordinatePairs[i].coord2).SetOccupant(null);
			}
			if (base.GetComponentInParent<IStorageEntity>() != null)
			{
				base.GetComponentInParent<IStorageEntity>().DereserveItem(this);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00105F07 File Offset: 0x00104107
		public void DestroyStoredItem()
		{
			this.Destroyed = true;
			this.ClearFootprintOccupancy();
			if (this != null && base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x00105F38 File Offset: 0x00104138
		public void ClearFootprintOccupancy()
		{
			if (this.parentGrid == null)
			{
				return;
			}
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				StorageTile tile = this.parentGrid.GetTile(this.coordinatePairs[i].coord2);
				if (!(tile == null))
				{
					tile.SetOccupant(null);
					this.parentGrid.freeTiles.Add(tile);
				}
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x00105FA8 File Offset: 0x001041A8
		public void SetCanBePickedUp(bool _canBePickedUp, string _noPickupReason = "")
		{
			this.canBePickedUp = _canBePickedUp;
			this.noPickupReason = _noPickupReason;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00105FB8 File Offset: 0x001041B8
		public static void SetLayerRecursively(GameObject go, int layerNumber)
		{
			foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					transform.gameObject.layer = layerNumber;
				}
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00106004 File Offset: 0x00104204
		public static List<StoredItem> RemoveReservedItems(List<StoredItem> itemList, Employee allowedReservant)
		{
			return (from x in itemList
			where x.parentStorageEntity.WhoIsReserving(x) == null || x.parentStorageEntity.WhoIsReserving(x) == allowedReservant
			select x).ToList<StoredItem>();
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00106035 File Offset: 0x00104235
		public virtual GameObject CreateGhostModel(ItemInstance _item, Transform parent)
		{
			return UnityEngine.Object.Instantiate<GameObject>(base.gameObject, parent);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00106044 File Offset: 0x00104244
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].tile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00106084 File Offset: 0x00104284
		public void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].tile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x001060C4 File Offset: 0x001042C4
		public FootprintTile GetTile(Coordinate coord)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				if (this.CoordinateFootprintTilePairs[i].coord.Equals(coord))
				{
					return this.CoordinateFootprintTilePairs[i].tile;
				}
			}
			return null;
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00106114 File Offset: 0x00104314
		public virtual void Hovered()
		{
			if (this.canBePickedUp)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.item, 1))
				{
					this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.intObj.SetMessage(string.Concat(new string[]
					{
						"Pick up <color=#",
						ColorUtility.ToHtmlStringRGBA(this.item.LabelDisplayColor),
						">",
						this.item.Name,
						"</color>"
					}));
					return;
				}
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.intObj.SetMessage("Inventory full");
				return;
			}
			else
			{
				if (this.noPickupReason != "")
				{
					this.intObj.SetMessage(this.noPickupReason);
					this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
					return;
				}
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x001061F4 File Offset: 0x001043F4
		public virtual void Interacted()
		{
			if (!this.canBePickedUp)
			{
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.item);
			this.DestroyStoredItem();
		}

		// Token: 0x04002C35 RID: 11317
		[Header("References")]
		public Transform buildPoint;

		// Token: 0x04002C36 RID: 11318
		public List<CoordinateStorageFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateStorageFootprintTilePair>();

		// Token: 0x04002C37 RID: 11319
		private int footprintX = -1;

		// Token: 0x04002C38 RID: 11320
		private int footprintY = -1;

		// Token: 0x04002C3B RID: 11323
		protected InteractableObject intObj;

		// Token: 0x04002C3C RID: 11324
		protected List<CoordinatePair> coordinatePairs = new List<CoordinatePair>();

		// Token: 0x04002C3D RID: 11325
		protected float rotation;

		// Token: 0x04002C3E RID: 11326
		public int xSize;

		// Token: 0x04002C3F RID: 11327
		public int ySize;
	}
}
