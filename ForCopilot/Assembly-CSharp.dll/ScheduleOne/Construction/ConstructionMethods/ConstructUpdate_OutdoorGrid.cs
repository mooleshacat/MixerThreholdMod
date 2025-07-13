using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using ScheduleOne.UI.Construction;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200076F RID: 1903
	public class ConstructUpdate_OutdoorGrid : ConstructUpdate_Base
	{
		// Token: 0x06003333 RID: 13107 RVA: 0x000D44E4 File Offset: 0x000D26E4
		protected virtual void Start()
		{
			this.listingPrice = Singleton<ConstructionMenu>.Instance.GetListingPrice(this.ConstructableClass.PrefabID);
			if (this.MovedConstructable == null)
			{
				this.currentRotation = Singleton<ConstructionManager>.Instance.currentProperty.DefaultRotation;
			}
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000D4524 File Offset: 0x000D2724
		protected override void Update()
		{
			base.Update();
			this.CheckRotation();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition && this.AreMetaReqsMet() && !Singleton<ConstructionMenu>.Instance.IsHoveringUI())
			{
				if (base.isMoving)
				{
					this.FinalizeMoveConstructable();
					return;
				}
				this.PlaceNewConstructable();
			}
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000D4578 File Offset: 0x000D2778
		protected override void LateUpdate()
		{
			base.LateUpdate();
			this.validPosition = false;
			this.GhostModel.transform.up = Vector3.up;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(500f, out raycastHit, this.detectionMask, true, 0f))
			{
				this.GhostModel.transform.position = raycastHit.point - this.GhostModel.transform.InverseTransformPoint(this.ConstructableClass.buildPoint.transform.position);
			}
			this.ApplyRotation();
			this.ConstructableClass.CalculateFootprintTileIntersections();
			this.CheckTileIntersections();
			this.UpdateMaterials();
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000D4624 File Offset: 0x000D2824
		protected void CheckRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft))
			{
				this.currentRotation -= 90f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight))
			{
				this.currentRotation += 90f;
			}
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000D465C File Offset: 0x000D285C
		protected void ApplyRotation()
		{
			this.GhostModel.transform.rotation = Quaternion.Inverse(this.ConstructableClass.buildPoint.transform.rotation) * this.GhostModel.transform.rotation;
			this.GhostModel.transform.Rotate(this.ConstructableClass.buildPoint.up, this.currentRotation);
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000D46D0 File Offset: 0x000D28D0
		protected virtual void CheckTileIntersections()
		{
			List<ConstructionManager.WorldIntersection> list = new List<ConstructionManager.WorldIntersection>();
			for (int i = 0; i < this.ConstructableClass.CoordinateFootprintTilePairs.Count; i++)
			{
				for (int j = 0; j < this.ConstructableClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedOutdoorTiles.Count; j++)
				{
					list.Add(new ConstructionManager.WorldIntersection
					{
						footprint = this.ConstructableClass.CoordinateFootprintTilePairs[i].footprintTile,
						tile = this.ConstructableClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedOutdoorTiles[j]
					});
				}
			}
			if (list.Count == 0)
			{
				this.ConstructableClass.SetFootprintTileVisiblity(false);
				return;
			}
			this.ConstructableClass.SetFootprintTileVisiblity(true);
			ConstructionManager.WorldIntersection worldIntersection = this.closestIntersection;
			float num = 100f;
			this.closestIntersection = null;
			for (int k = 0; k < list.Count; k++)
			{
				if (Vector3.Distance(list[k].footprint.transform.position, list[k].tile.transform.position) < num)
				{
					num = Vector3.Distance(list[k].footprint.transform.position, list[k].tile.transform.position);
					this.closestIntersection = list[k];
				}
			}
			List<Vector2> list2 = new List<Vector2>();
			this.GhostModel.transform.position = this.closestIntersection.tile.transform.position + (this.GhostModel.transform.position - this.closestIntersection.footprint.transform.position);
			if (base.isMoving)
			{
				Constructable_GridBased movedConstructable = this.MovedConstructable;
			}
			this.validPosition = true;
			for (int l = 0; l < this.ConstructableClass.CoordinateFootprintTilePairs.Count; l++)
			{
				Coordinate matchedCoordinate = this.closestIntersection.tile.OwnerGrid.GetMatchedCoordinate(this.ConstructableClass.CoordinateFootprintTilePairs[l].footprintTile);
				this.ConstructableClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.Red);
				if (this.closestIntersection.tile.OwnerGrid.GetTile(matchedCoordinate) == null)
				{
					this.validPosition = false;
				}
				else
				{
					list2.Add(new Vector2((float)matchedCoordinate.x, (float)matchedCoordinate.y));
					if (this.closestIntersection.tile.OwnerGrid.IsTileValidAtCoordinate(matchedCoordinate, this.ConstructableClass.CoordinateFootprintTilePairs[l].footprintTile, this.MovedConstructable))
					{
						this.ConstructableClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.White);
					}
					else
					{
						this.validPosition = false;
					}
				}
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000D49E4 File Offset: 0x000D2BE4
		protected void UpdateMaterials()
		{
			Material material = Singleton<BuildManager>.Instance.ghostMaterial_White;
			if (!this.validPosition || !this.AreMetaReqsMet())
			{
				material = Singleton<BuildManager>.Instance.ghostMaterial_Red;
			}
			if (this.currentGhostMaterial != material)
			{
				this.currentGhostMaterial = material;
				Singleton<BuildManager>.Instance.ApplyMaterial(this.GhostModel.gameObject, material, true);
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000D4A43 File Offset: 0x000D2C43
		private bool AreMetaReqsMet()
		{
			return base.isMoving || NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.listingPrice;
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000D4A64 File Offset: 0x000D2C64
		protected virtual Constructable_GridBased PlaceNewConstructable()
		{
			Constructable_GridBased constructable_GridBased = Singleton<ConstructionManager>.Instance.CreateConstructable_GridBased(this.ConstructableClass.PrefabID, this.closestIntersection.tile.OwnerGrid, this.GetOriginCoordinate(), this.currentRotation);
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.ConstructableClass.ConstructableName, -this.listingPrice, 1f, string.Empty);
			if (Singleton<ConstructionManager>.Instance.onNewConstructableBuilt != null)
			{
				Singleton<ConstructionManager>.Instance.onNewConstructableBuilt(constructable_GridBased);
			}
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
			}
			return constructable_GridBased;
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000D4AFC File Offset: 0x000D2CFC
		protected virtual void FinalizeMoveConstructable()
		{
			this.MovedConstructable.RepositionConstructable(this.closestIntersection.tile.OwnerGrid.GUID, this.GetOriginCoordinate(), this.currentRotation);
			Constructable_GridBased movedConstructable = this.MovedConstructable;
			Singleton<ConstructionManager>.Instance.StopMovingConstructable();
			if (Singleton<ConstructionManager>.Instance.onConstructableMoved != null)
			{
				Singleton<ConstructionManager>.Instance.onConstructableMoved(movedConstructable);
			}
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000D4B64 File Offset: 0x000D2D64
		private Vector2 GetOriginCoordinate()
		{
			this.ConstructableClass.OriginFootprint.tileDetector.CheckIntersections(true);
			return new Vector2((float)this.ConstructableClass.OriginFootprint.tileDetector.intersectedOutdoorTiles[0].x, (float)this.ConstructableClass.OriginFootprint.tileDetector.intersectedOutdoorTiles[0].y);
		}

		// Token: 0x04002419 RID: 9241
		[Header("Settings")]
		public LayerMask detectionMask;

		// Token: 0x0400241A RID: 9242
		public Constructable_GridBased ConstructableClass;

		// Token: 0x0400241B RID: 9243
		public Transform GhostModel;

		// Token: 0x0400241C RID: 9244
		protected bool validPosition;

		// Token: 0x0400241D RID: 9245
		public float currentRotation;

		// Token: 0x0400241E RID: 9246
		protected Material currentGhostMaterial;

		// Token: 0x0400241F RID: 9247
		protected ConstructionManager.WorldIntersection closestIntersection;

		// Token: 0x04002420 RID: 9248
		private float listingPrice;
	}
}
