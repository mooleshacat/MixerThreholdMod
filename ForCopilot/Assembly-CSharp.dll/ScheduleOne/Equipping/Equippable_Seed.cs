using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000954 RID: 2388
	public class Equippable_Seed : Equippable_Viewmodel
	{
		// Token: 0x06004070 RID: 16496 RVA: 0x0011079C File Offset: 0x0010E99C
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
					if (componentInParent.CanAcceptSeed(out message))
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
						componentInParent.ConfigureInteraction("Sow seed", InteractableObject.EInteractableState.Default, false);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							this.StartSowSeedTask(componentInParent);
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

		// Token: 0x06004071 RID: 16497 RVA: 0x0011086C File Offset: 0x0010EA6C
		protected virtual void StartSowSeedTask(Pot pot)
		{
			new SowSeedTask(pot, this.Seed);
		}

		// Token: 0x04002DE1 RID: 11745
		public SeedDefinition Seed;
	}
}
