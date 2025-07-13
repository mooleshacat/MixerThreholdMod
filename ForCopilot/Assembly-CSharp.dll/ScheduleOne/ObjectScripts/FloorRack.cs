using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.EntityFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF4 RID: 3060
	public class FloorRack : GridItem, IProceduralTileContainer
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x0015A38C File Offset: 0x0015858C
		public List<ProceduralTile> ProceduralTiles
		{
			get
			{
				return this.procTiles;
			}
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x0015A394 File Offset: 0x00158594
		public virtual void UpdateLegVisibility()
		{
			this.CockAndBalls(this.leg_BottomLeft.gameObject, this.obs_BottomLeft, -1, -1);
			this.CockAndBalls(this.leg_BottomRight.gameObject, this.obs_BottomRight, 1, -1);
			this.CockAndBalls(this.leg_TopLeft.gameObject, this.obs_TopLeft, -1, 1);
			this.CockAndBalls(this.leg_TopRight.gameObject, this.obs_TopRight, 1, 1);
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x0015A408 File Offset: 0x00158608
		protected void CockAndBalls(GameObject leg, CornerObstacle obs, int xOffset, int yOffset)
		{
			FloorRack x = null;
			FloorRack x2 = null;
			FloorRack x3 = null;
			Coordinate coord = new Coordinate(this.CoordinatePairs[0].coord2.x + xOffset, this.CoordinatePairs[0].coord2.y + yOffset);
			if (base.OwnerGrid.GetTile(coord) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants) != null)
			{
				x = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants);
			}
			Coordinate coord2 = new Coordinate(this.CoordinatePairs[0].coord2.x + xOffset, this.CoordinatePairs[0].coord2.y);
			if (base.OwnerGrid.GetTile(coord2) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord2).BuildableOccupants) != null)
			{
				x2 = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord2).BuildableOccupants);
			}
			Coordinate coord3 = new Coordinate(this.CoordinatePairs[0].coord2.x, this.CoordinatePairs[0].coord2.y + yOffset);
			if (base.OwnerGrid.GetTile(coord3) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord3).BuildableOccupants) != null)
			{
				x3 = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord3).BuildableOccupants);
			}
			bool flag = true;
			if ((!(x2 != null) || !(x3 != null) || !(x != null)) && x == null && (!(x2 != null) || !(x3 == null)) && x2 == null)
			{
				x3 != null;
			}
			leg.gameObject.SetActive(flag);
			obs.obstacleEnabled = flag;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x0015A604 File Offset: 0x00158804
		private FloorRack GetFloorRackFromOccupants(List<GridItem> occs)
		{
			for (int i = 0; i < occs.Count; i++)
			{
				if (occs[i] is FloorRack)
				{
					return occs[i] as FloorRack;
				}
			}
			return null;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x0015A640 File Offset: 0x00158840
		public List<FloorRack> GetSurroundingRacks()
		{
			List<FloorRack> list = new List<FloorRack>();
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					if (i != 0 || j != 0)
					{
						Coordinate coord = new Coordinate(this.CoordinatePairs[0].coord2.x + i, this.CoordinatePairs[0].coord2.y + j);
						if (base.OwnerGrid.GetTile(coord) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants) != null)
						{
							list.Add(this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x0015A708 File Offset: 0x00158908
		public override bool CanShareTileWith(List<GridItem> obstacles)
		{
			for (int i = 0; i < obstacles.Count; i++)
			{
				if (obstacles[i] is FloorRack)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x0015A738 File Offset: 0x00158938
		public override bool CanBeDestroyed(out string reason)
		{
			bool flag = false;
			foreach (ProceduralTile proceduralTile in this.procTiles)
			{
				if (proceduralTile.Occupants.Count > 0 || proceduralTile.OccupantTiles.Count > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				reason = base.ItemInstance.Name + " is supporting another item";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x0015A7CC File Offset: 0x001589CC
		public override void DestroyItem(bool callOnServer = true)
		{
			for (int i = 0; i < this.CoordinatePairs.Count; i++)
			{
				base.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).RemoveOccupant(this, base.GetFootprintTile(this.CoordinatePairs[i].coord1));
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x0015A82F File Offset: 0x00158A2F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x0015A848 File Offset: 0x00158A48
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x0015A861 File Offset: 0x00158A61
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x0015A86F File Offset: 0x00158A6F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003D6D RID: 15725
		[Header("References")]
		public Transform leg_BottomLeft;

		// Token: 0x04003D6E RID: 15726
		public Transform leg_BottomRight;

		// Token: 0x04003D6F RID: 15727
		public Transform leg_TopLeft;

		// Token: 0x04003D70 RID: 15728
		public Transform leg_TopRight;

		// Token: 0x04003D71 RID: 15729
		public CornerObstacle obs_BottomLeft;

		// Token: 0x04003D72 RID: 15730
		public CornerObstacle obs_BottomRight;

		// Token: 0x04003D73 RID: 15731
		public CornerObstacle obs_TopLeft;

		// Token: 0x04003D74 RID: 15732
		public CornerObstacle obs_TopRight;

		// Token: 0x04003D75 RID: 15733
		public List<ProceduralTile> procTiles;

		// Token: 0x04003D76 RID: 15734
		private bool dll_Excuted;

		// Token: 0x04003D77 RID: 15735
		private bool dll_Excuted;
	}
}
