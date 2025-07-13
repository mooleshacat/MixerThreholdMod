using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007C3 RID: 1987
	public class BuildStart_ProceduralGrid : BuildStart_Base
	{
		// Token: 0x060035ED RID: 13805 RVA: 0x000E1A4C File Offset: 0x000DFC4C
		public override void StartBuilding(ItemInstance itemInstance)
		{
			ProceduralGridItem proceduralGridItem = this.CreateGhostModel(itemInstance.Definition as BuildableItemDefinition);
			if (proceduralGridItem == null)
			{
				return;
			}
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			ProceduralGridItem component = proceduralGridItem.GetComponent<ProceduralGridItem>();
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().GhostModel = proceduralGridItem.gameObject;
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().ItemClass = component;
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().ItemInstance = itemInstance;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("building");
			for (int i = 0; i < component.CoordinateFootprintTilePairs.Count; i++)
			{
				component.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.tileDetectionMode = ETileDetectionMode.ProceduralTile;
			}
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000E1B00 File Offset: 0x000DFD00
		protected virtual ProceduralGridItem CreateGhostModel(BuildableItemDefinition itemDefinition)
		{
			itemDefinition.BuiltItem.isGhost = true;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemDefinition.BuiltItem.gameObject, base.transform);
			itemDefinition.BuiltItem.isGhost = false;
			ProceduralGridItem component = gameObject.GetComponent<ProceduralGridItem>();
			if (component == null)
			{
				Console.LogWarning("CreateGhostModel: asset path is not a BuildableItem!", null);
				return null;
			}
			component.enabled = false;
			component.isGhost = true;
			Singleton<BuildManager>.Instance.DisableColliders(gameObject);
			Singleton<BuildManager>.Instance.DisableNavigation(gameObject);
			Singleton<BuildManager>.Instance.DisableNetworking(gameObject);
			component.SetFootprintTileVisiblity(false);
			return component;
		}
	}
}
