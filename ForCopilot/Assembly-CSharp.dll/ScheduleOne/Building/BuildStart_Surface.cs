using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007C5 RID: 1989
	public class BuildStart_Surface : BuildStart_Base
	{
		// Token: 0x060035F3 RID: 13811 RVA: 0x000E1C84 File Offset: 0x000DFE84
		public override void StartBuilding(ItemInstance itemInstance)
		{
			SurfaceItem surfaceItem = this.CreateGhostModel(itemInstance.Definition as BuildableItemDefinition);
			if (surfaceItem == null)
			{
				return;
			}
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("building");
			base.gameObject.GetComponent<BuildUpdate_Surface>().GhostModel = surfaceItem.gameObject;
			base.gameObject.GetComponent<BuildUpdate_Surface>().BuildableItemClass = surfaceItem;
			base.gameObject.GetComponent<BuildUpdate_Surface>().ItemInstance = itemInstance;
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000E1D00 File Offset: 0x000DFF00
		protected virtual SurfaceItem CreateGhostModel(BuildableItemDefinition itemDefinition)
		{
			itemDefinition.BuiltItem.isGhost = true;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemDefinition.BuiltItem.gameObject, base.transform);
			itemDefinition.BuiltItem.isGhost = false;
			SurfaceItem component = gameObject.GetComponent<SurfaceItem>();
			if (component == null)
			{
				Console.LogWarning("CreateGhostModel: asset path is not a SurfaceItem!", null);
				return null;
			}
			component.enabled = false;
			component.isGhost = true;
			Singleton<BuildManager>.Instance.DisableColliders(gameObject);
			Singleton<BuildManager>.Instance.DisableNavigation(gameObject);
			Singleton<BuildManager>.Instance.DisableNetworking(gameObject);
			Singleton<BuildManager>.Instance.DisableCanvases(gameObject);
			ActivateDuringBuild[] componentsInChildren = gameObject.GetComponentsInChildren<ActivateDuringBuild>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(true);
			}
			return component;
		}
	}
}
