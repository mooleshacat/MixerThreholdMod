using System;
using System.Collections.Generic;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007CB RID: 1995
	public class BuildUpdate_ProceduralGrid : BuildUpdate_Base
	{
		// Token: 0x06003613 RID: 13843 RVA: 0x000E2FD4 File Offset: 0x000E11D4
		protected virtual void Update()
		{
			this.CheckRotation();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition)
			{
				this.Place();
			}
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000E2FF4 File Offset: 0x000E11F4
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.GhostModel.transform.up = Vector3.up;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, true, 0f))
			{
				this.GhostModel.transform.position = raycastHit.point - this.GhostModel.transform.InverseTransformPoint(this.ItemClass.BuildPoint.transform.position);
			}
			else
			{
				this.GhostModel.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.ItemClass.HoldDistance;
				if (this.ItemClass.MidAirCenterPoint != null)
				{
					this.GhostModel.transform.position += -this.GhostModel.transform.InverseTransformPoint(this.ItemClass.MidAirCenterPoint.transform.position);
				}
			}
			this.ApplyRotation();
			this.CheckGridIntersections();
			this.UpdateMaterials();
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000E3130 File Offset: 0x000E1330
		protected void CheckRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft) && !GameInput.IsTyping)
			{
				this.currentRotation -= 90f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight) && !GameInput.IsTyping)
			{
				this.currentRotation += 90f;
			}
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000E3184 File Offset: 0x000E1384
		protected void ApplyRotation()
		{
			this.GhostModel.transform.rotation = Quaternion.Inverse(this.ItemClass.BuildPoint.transform.rotation) * this.GhostModel.transform.rotation;
			ProceduralTile nearbyProcTile = this.GetNearbyProcTile();
			float num = this.currentRotation;
			if (nearbyProcTile != null)
			{
				num += nearbyProcTile.transform.eulerAngles.y;
			}
			this.GhostModel.transform.Rotate(this.ItemClass.BuildPoint.up, num);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000E321C File Offset: 0x000E141C
		protected virtual void CheckGridIntersections()
		{
			this.ItemClass.CalculateFootprintTileIntersections();
			List<BuildUpdate_ProceduralGrid.Intersection> list = new List<BuildUpdate_ProceduralGrid.Intersection>();
			for (int i = 0; i < this.ItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				for (int j = 0; j < this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedProceduralTiles.Count; j++)
				{
					list.Add(new BuildUpdate_ProceduralGrid.Intersection
					{
						footprintTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile,
						procTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedProceduralTiles[j]
					});
				}
			}
			if (list.Count == 0)
			{
				this.ItemClass.SetFootprintTileVisiblity(false);
				return;
			}
			this.ItemClass.SetFootprintTileVisiblity(true);
			float num = 100f;
			this.bestIntersection = null;
			for (int k = 0; k < list.Count; k++)
			{
				if (Vector3.Distance(list[k].footprintTile.transform.position, list[k].procTile.transform.position) < num)
				{
					num = Vector3.Distance(list[k].footprintTile.transform.position, list[k].procTile.transform.position);
					this.bestIntersection = list[k];
				}
			}
			this.validPosition = true;
			this.GhostModel.transform.position = this.bestIntersection.procTile.transform.position - (this.bestIntersection.footprintTile.transform.position - this.GhostModel.transform.position);
			this.ItemClass.CalculateFootprintTileIntersections();
			for (int l = 0; l < this.ItemClass.CoordinateFootprintTilePairs.Count; l++)
			{
				bool flag = false;
				ProceduralTile closestProceduralTile = this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileDetector.GetClosestProceduralTile();
				if (this.IsMatchValid(this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile, closestProceduralTile))
				{
					flag = true;
				}
				if (flag)
				{
					this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.White);
				}
				else
				{
					this.validPosition = false;
					this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.Red);
				}
			}
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000E34CC File Offset: 0x000E16CC
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
				Singleton<BuildManager>.Instance.ApplyMaterial(this.GhostModel, material, true);
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000E3520 File Offset: 0x000E1720
		private bool IsMatchValid(FootprintTile footprintTile, ProceduralTile matchedTile)
		{
			return !(footprintTile == null) && !(matchedTile == null) && (Vector3.Distance(matchedTile.transform.position, footprintTile.transform.position) < 0.01f && matchedTile.Occupants.Count == 0 && matchedTile.TileType == this.ItemClass.ProceduralTileType);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000E3588 File Offset: 0x000E1788
		protected void Place()
		{
			List<CoordinateProceduralTilePair> list = new List<CoordinateProceduralTilePair>();
			for (int i = 0; i < this.ItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				bool flag = false;
				ProceduralTile closestProceduralTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.GetClosestProceduralTile();
				if (this.IsMatchValid(this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile, closestProceduralTile))
				{
					flag = true;
				}
				if (!flag)
				{
					Console.LogWarning("Invalid placement!", null);
					return;
				}
				NetworkObject networkObject = closestProceduralTile.ParentBuildableItem.NetworkObject;
				int tileIndex = (closestProceduralTile.ParentBuildableItem as IProceduralTileContainer).ProceduralTiles.IndexOf(closestProceduralTile);
				list.Add(new CoordinateProceduralTilePair
				{
					coord = this.ItemClass.CoordinateFootprintTilePairs[i].coord,
					tileParent = networkObject,
					tileIndex = tileIndex
				});
			}
			float f = Vector3.SignedAngle(list[0].tile.transform.forward, this.GhostModel.transform.forward, list[0].tile.transform.up);
			Singleton<BuildManager>.Instance.CreateProceduralGridItem(this.ItemInstance.GetCopy(1), Mathf.RoundToInt(f), list, "");
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			Singleton<BuildManager>.Instance.PlayBuildSound((this.ItemInstance.Definition as BuildableItemDefinition).BuildSoundType, this.GhostModel.transform.position);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000E3728 File Offset: 0x000E1928
		private ProceduralTile GetNearbyProcTile()
		{
			Collider[] array = Physics.OverlapSphere(this.GhostModel.transform.position, 1f, this.detectionMask);
			for (int i = 0; i < array.Length; i++)
			{
				ProceduralTile component = array[i].GetComponent<ProceduralTile>();
				if (component != null)
				{
					return component;
				}
			}
			return null;
		}

		// Token: 0x04002642 RID: 9794
		public GameObject GhostModel;

		// Token: 0x04002643 RID: 9795
		public ProceduralGridItem ItemClass;

		// Token: 0x04002644 RID: 9796
		public ItemInstance ItemInstance;

		// Token: 0x04002645 RID: 9797
		[Header("Settings")]
		public float detectionRange = 6f;

		// Token: 0x04002646 RID: 9798
		public LayerMask detectionMask;

		// Token: 0x04002647 RID: 9799
		public float rotation_Smoothing = 5f;

		// Token: 0x04002648 RID: 9800
		protected float currentRotation;

		// Token: 0x04002649 RID: 9801
		protected bool validPosition;

		// Token: 0x0400264A RID: 9802
		protected Material currentGhostMaterial;

		// Token: 0x0400264B RID: 9803
		protected BuildUpdate_ProceduralGrid.Intersection bestIntersection;

		// Token: 0x020007CC RID: 1996
		public class Intersection
		{
			// Token: 0x0400264C RID: 9804
			public FootprintTile footprintTile;

			// Token: 0x0400264D RID: 9805
			public ProceduralTile procTile;
		}
	}
}
