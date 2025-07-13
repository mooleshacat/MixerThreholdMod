using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B4E RID: 2894
	public class ChemistryStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x00145263 File Offset: 0x00143463
		// (set) Token: 0x06004D23 RID: 19747 RVA: 0x0014526B File Offset: 0x0014346B
		public ChemistryStation AssignedStation { get; protected set; }

		// Token: 0x06004D24 RID: 19748 RVA: 0x00145274 File Offset: 0x00143474
		public void Initialize(ChemistryStation oven)
		{
			this.AssignedStation = oven;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x001452B4 File Offset: 0x001434B4
		protected virtual void RefreshUI()
		{
			ChemistryStationConfiguration chemistryStationConfiguration = this.AssignedStation.Configuration as ChemistryStationConfiguration;
			base.SetAssignedNPC(chemistryStationConfiguration.AssignedChemist.SelectedNPC);
			if (chemistryStationConfiguration.Recipe.SelectedRecipe != null)
			{
				this.RecipeEntry.AssignRecipe(chemistryStationConfiguration.Recipe.SelectedRecipe);
				this.RecipeEntry.gameObject.SetActive(true);
				this.NoRecipe.SetActive(false);
				return;
			}
			this.RecipeEntry.gameObject.SetActive(false);
			this.NoRecipe.SetActive(true);
		}

		// Token: 0x04003989 RID: 14729
		[Header("References")]
		public StationRecipeEntry RecipeEntry;

		// Token: 0x0400398A RID: 14730
		public GameObject NoRecipe;
	}
}
