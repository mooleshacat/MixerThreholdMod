using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000957 RID: 2391
	public class Equippable_Trimmers : Equippable_Viewmodel
	{
		// Token: 0x06004084 RID: 16516 RVA: 0x00110D3C File Offset: 0x0010EF3C
		protected override void Update()
		{
			base.Update();
			if (Singleton<TaskManager>.Instance.currentTask != null)
			{
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				return;
			}
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				Pot componentInParent = raycastHit.collider.GetComponentInParent<Pot>();
				if (componentInParent != null)
				{
					string message;
					if (componentInParent.IsReadyForHarvest(out message))
					{
						if (componentInParent.PlayerUserObject != null)
						{
							componentInParent.ConfigureInteraction("In use by other player", InteractableObject.EInteractableState.Invalid, false);
							return;
						}
						if (componentInParent.NPCUserObject != null)
						{
							componentInParent.ConfigureInteraction("In use by workers", InteractableObject.EInteractableState.Invalid, false);
							return;
						}
						componentInParent.ConfigureInteraction("Harvest", InteractableObject.EInteractableState.Default, true);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							new HarvestPlant(componentInParent, this.CanClickAndDrag, this.SoundLoopPrefab);
							return;
						}
					}
					else
					{
						componentInParent.ConfigureInteraction(message, InteractableObject.EInteractableState.Invalid, false);
					}
				}
			}
		}

		// Token: 0x04002DF5 RID: 11765
		public bool CanClickAndDrag;

		// Token: 0x04002DF6 RID: 11766
		public AudioSourceController SoundLoopPrefab;
	}
}
