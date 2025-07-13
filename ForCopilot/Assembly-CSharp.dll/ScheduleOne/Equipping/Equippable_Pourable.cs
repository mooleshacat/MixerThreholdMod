using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.PlayerTasks.Tasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000960 RID: 2400
	public class Equippable_Pourable : Equippable_Viewmodel
	{
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x001119B9 File Offset: 0x0010FBB9
		// (set) Token: 0x060040B5 RID: 16565 RVA: 0x001119C1 File Offset: 0x0010FBC1
		public virtual string InteractionLabel { get; set; } = "Pour";

		// Token: 0x060040B6 RID: 16566 RVA: 0x001119CC File Offset: 0x0010FBCC
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
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.InteractionRange, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				Pot componentInParent = raycastHit.collider.GetComponentInParent<Pot>();
				if (componentInParent == null)
				{
					return;
				}
				string empty = string.Empty;
				if (this.CanPour(componentInParent, out empty))
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
					componentInParent.ConfigureInteraction(this.InteractionLabel, InteractableObject.EInteractableState.Default, false);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
					{
						this.StartPourTask(componentInParent);
						return;
					}
				}
				else
				{
					if (empty != string.Empty)
					{
						componentInParent.ConfigureInteraction(empty, InteractableObject.EInteractableState.Invalid, false);
						return;
					}
					componentInParent.ConfigureInteraction(string.Empty, InteractableObject.EInteractableState.Disabled, false);
				}
			}
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x00111AC4 File Offset: 0x0010FCC4
		protected virtual void StartPourTask(Pot pot)
		{
			new PourIntoPotTask(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x00111AD9 File Offset: 0x0010FCD9
		protected virtual bool CanPour(Pot pot, out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x04002E22 RID: 11810
		[Header("Pourable settings")]
		public float InteractionRange = 2.5f;

		// Token: 0x04002E23 RID: 11811
		public Pourable PourablePrefab;
	}
}
