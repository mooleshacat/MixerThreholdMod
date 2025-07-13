using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000452 RID: 1106
	public class PotData : GridItemData
	{
		// Token: 0x06001689 RID: 5769 RVA: 0x000642D4 File Offset: 0x000624D4
		public PotData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, string soilID, float soilLevel, int remainingSoilUses, float waterLevel, string[] appliedAdditives, PlantData plantData) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.SoilID = soilID;
			this.SoilLevel = soilLevel;
			this.RemainingSoilUses = remainingSoilUses;
			this.WaterLevel = waterLevel;
			this.AppliedAdditives = appliedAdditives;
			this.PlantData = plantData;
		}

		// Token: 0x0400149D RID: 5277
		public string SoilID;

		// Token: 0x0400149E RID: 5278
		public float SoilLevel;

		// Token: 0x0400149F RID: 5279
		public int RemainingSoilUses;

		// Token: 0x040014A0 RID: 5280
		public float WaterLevel;

		// Token: 0x040014A1 RID: 5281
		public string[] AppliedAdditives;

		// Token: 0x040014A2 RID: 5282
		public PlantData PlantData;
	}
}
