using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007CD RID: 1997
	public class BuildUpdate_StoredItem : BuildUpdate_Base
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x000E379C File Offset: 0x000E199C
		protected virtual void Update()
		{
			this.CheckRotation();
			if (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.mouseUpSinceStart = true;
				this.mouseUpSincePlace = true;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition && this.mouseUpSinceStart)
			{
				this.Place();
			}
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000E37D8 File Offset: 0x000E19D8
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.ghostModel.transform.up = Vector3.up;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0f))
			{
				this.ghostModel.transform.position = raycastHit.point - this.ghostModel.transform.InverseTransformPoint(this.storedItemClass.buildPoint.transform.position);
			}
			else
			{
				this.ghostModel.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.storedItemHoldDistance;
			}
			this.ApplyRotation();
			this.storedItemClass.CalculateFootprintTileIntersections();
			this.CheckGridIntersections();
			if (this.validPosition)
			{
				this.positionDuringLastValidPosition = this.ghostModel.transform.position;
			}
			else if (this.mouseUpSincePlace)
			{
				Vector3 position = this.ghostModel.transform.position;
				float d = 0.0625f;
				this.ghostModel.transform.position = position + this.ghostModel.transform.right * d;
				this.storedItemClass.CalculateFootprintTileIntersections();
				this.CheckGridIntersections();
				if (!this.validPosition)
				{
					this.ghostModel.transform.position = position - this.ghostModel.transform.right * d;
					this.storedItemClass.CalculateFootprintTileIntersections();
					this.CheckGridIntersections();
					if (!this.validPosition)
					{
						this.ghostModel.transform.position = position + this.ghostModel.transform.forward * d;
						this.storedItemClass.CalculateFootprintTileIntersections();
						this.CheckGridIntersections();
						if (!this.validPosition)
						{
							this.ghostModel.transform.position = position - this.ghostModel.transform.forward * d;
							this.storedItemClass.CalculateFootprintTileIntersections();
							this.CheckGridIntersections();
							if (!this.validPosition)
							{
								this.ghostModel.transform.position = position;
								this.storedItemClass.CalculateFootprintTileIntersections();
								this.CheckGridIntersections();
							}
						}
					}
				}
			}
			this.UpdateMaterials();
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000E3A40 File Offset: 0x000E1C40
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

		// Token: 0x06003621 RID: 13857 RVA: 0x000E3A78 File Offset: 0x000E1C78
		protected void ApplyRotation()
		{
			this.ghostModel.transform.rotation = Quaternion.Inverse(this.storedItemClass.buildPoint.transform.rotation) * this.ghostModel.transform.rotation;
			this.ghostModel.transform.Rotate(this.storedItemClass.buildPoint.up, this.currentRotation);
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000E3AEC File Offset: 0x000E1CEC
		protected virtual void CheckGridIntersections()
		{
			List<BuildUpdate_StoredItem.StorageTileIntersection> list = new List<BuildUpdate_StoredItem.StorageTileIntersection>();
			for (int i = 0; i < this.storedItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				for (int j = 0; j < this.storedItemClass.CoordinateFootprintTilePairs[i].tile.tileDetector.intersectedStorageTiles.Count; j++)
				{
					list.Add(new BuildUpdate_StoredItem.StorageTileIntersection
					{
						footprintTile = this.storedItemClass.CoordinateFootprintTilePairs[i].tile,
						storageTile = this.storedItemClass.CoordinateFootprintTilePairs[i].tile.tileDetector.intersectedStorageTiles[j]
					});
				}
			}
			if (list.Count == 0)
			{
				this.storedItemClass.SetFootprintTileVisiblity(false);
				this.bestIntersection = null;
				return;
			}
			this.storedItemClass.SetFootprintTileVisiblity(true);
			float num = 100f;
			this.bestIntersection = null;
			for (int k = 0; k < list.Count; k++)
			{
				if (this.bestIntersection == null || Vector3.Distance(list[k].footprintTile.transform.position, list[k].storageTile.transform.position) < num)
				{
					num = Vector3.Distance(list[k].footprintTile.transform.position, list[k].storageTile.transform.position);
					this.bestIntersection = list[k];
				}
			}
			if (this.bestIntersection != null && this.bestIntersection.storageTile.GetComponentInParent<Pallet>())
			{
				Vector3 vector = this.bestIntersection.storageTile.transform.forward;
				if (Vector3.Angle(base.transform.forward, -this.bestIntersection.storageTile.transform.forward) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = -this.bestIntersection.storageTile.transform.forward;
				}
				if (Vector3.Angle(base.transform.forward, this.bestIntersection.storageTile.transform.right) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = this.bestIntersection.storageTile.transform.right;
				}
				if (Vector3.Angle(base.transform.forward, -this.bestIntersection.storageTile.transform.right) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = -this.bestIntersection.storageTile.transform.right;
				}
				this.ghostModel.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
				this.ghostModel.transform.Rotate(this.storedItemClass.buildPoint.up, this.currentRotation);
			}
			this.ghostModel.transform.position = this.bestIntersection.storageTile.transform.position - (this.bestIntersection.footprintTile.transform.position - this.ghostModel.transform.position);
			this.validPosition = this.bestIntersection.storageTile.ownerGrid.IsItemPositionValid(this.bestIntersection.storageTile, this.bestIntersection.footprintTile, this.storedItemClass);
			for (int l = 0; l < this.storedItemClass.CoordinateFootprintTilePairs.Count; l++)
			{
				Coordinate matchedCoordinate = this.bestIntersection.storageTile.ownerGrid.GetMatchedCoordinate(this.storedItemClass.CoordinateFootprintTilePairs[l].tile);
				CoordinateStorageFootprintTilePair coordinateStorageFootprintTilePair = this.storedItemClass.CoordinateFootprintTilePairs[l];
				if (this.bestIntersection.storageTile.ownerGrid.IsGridPositionValid(matchedCoordinate, this.storedItemClass.CoordinateFootprintTilePairs[l].tile))
				{
					this.storedItemClass.CoordinateFootprintTilePairs[l].tile.tileAppearance.SetColor(ETileColor.White);
				}
				else
				{
					this.storedItemClass.CoordinateFootprintTilePairs[l].tile.tileAppearance.SetColor(ETileColor.Red);
				}
			}
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000E3F68 File Offset: 0x000E2168
		protected void UpdateMaterials()
		{
			Material material = Singleton<BuildManager>.Instance.ghostMaterial_White;
			if (!this.validPosition)
			{
				material = Singleton<BuildManager>.Instance.ghostMaterial_Red;
			}
			if (this.currentGhostMaterial != material)
			{
				this.currentGhostMaterial = material;
				Singleton<BuildManager>.Instance.ApplyMaterial(this.ghostModel, material, true);
			}
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000E3FBC File Offset: 0x000E21BC
		protected virtual void Place()
		{
			float rotation = Vector3.SignedAngle(this.bestIntersection.storageTile.ownerGrid.transform.forward, this.storedItemClass.buildPoint.forward, this.bestIntersection.storageTile.ownerGrid.transform.up);
			StorableItemInstance item = this.itemInstance.GetCopy(1) as StorableItemInstance;
			Singleton<BuildManager>.Instance.CreateStoredItem(item, this.bestIntersection.storageTile.ownerGrid.GetComponentInParent<IStorageEntity>(), this.bestIntersection.storageTile.ownerGrid, this.GetOriginCoordinate(), rotation);
			this.mouseUpSincePlace = false;
			this.PostPlace();
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000E4069 File Offset: 0x000E2269
		protected virtual void PostPlace()
		{
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000E407C File Offset: 0x000E227C
		protected Vector2 GetOriginCoordinate()
		{
			this.storedItemClass.OriginFootprint.tileDetector.CheckIntersections(true);
			return new Vector2((float)this.storedItemClass.OriginFootprint.tileDetector.intersectedStorageTiles[0].x, (float)this.storedItemClass.OriginFootprint.tileDetector.intersectedStorageTiles[0].y);
		}

		// Token: 0x0400264E RID: 9806
		public StorableItemInstance itemInstance;

		// Token: 0x0400264F RID: 9807
		public GameObject ghostModel;

		// Token: 0x04002650 RID: 9808
		public StoredItem storedItemClass;

		// Token: 0x04002651 RID: 9809
		protected BuildUpdate_StoredItem.StorageTileIntersection bestIntersection;

		// Token: 0x04002652 RID: 9810
		[Header("Settings")]
		public float detectionRange = 6f;

		// Token: 0x04002653 RID: 9811
		public LayerMask detectionMask;

		// Token: 0x04002654 RID: 9812
		public float storedItemHoldDistance = 2f;

		// Token: 0x04002655 RID: 9813
		public float currentRotation;

		// Token: 0x04002656 RID: 9814
		protected bool validPosition;

		// Token: 0x04002657 RID: 9815
		protected Material currentGhostMaterial;

		// Token: 0x04002658 RID: 9816
		protected bool mouseUpSinceStart;

		// Token: 0x04002659 RID: 9817
		protected bool mouseUpSincePlace = true;

		// Token: 0x0400265A RID: 9818
		private Vector3 positionDuringLastValidPosition = Vector3.zero;

		// Token: 0x020007CE RID: 1998
		public class StorageTileIntersection
		{
			// Token: 0x0400265B RID: 9819
			public FootprintTile footprintTile;

			// Token: 0x0400265C RID: 9820
			public StorageTile storageTile;
		}
	}
}
