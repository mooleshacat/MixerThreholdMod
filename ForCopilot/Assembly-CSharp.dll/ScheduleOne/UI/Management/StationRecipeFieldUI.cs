using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B3A RID: 2874
	public class StationRecipeFieldUI : MonoBehaviour
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x001428D8 File Offset: 0x00140AD8
		// (set) Token: 0x06004C9F RID: 19615 RVA: 0x001428E0 File Offset: 0x00140AE0
		public List<StationRecipeField> Fields { get; protected set; } = new List<StationRecipeField>();

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001428EC File Offset: 0x00140AEC
		public void Bind(List<StationRecipeField> field)
		{
			this.Fields = new List<StationRecipeField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onRecipeChanged.AddListener(new UnityAction<StationRecipe>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedRecipe);
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x00142958 File Offset: 0x00140B58
		private void Refresh(StationRecipe newVal)
		{
			this.None.gameObject.SetActive(false);
			this.Mixed.gameObject.SetActive(false);
			this.ClearButton.gameObject.SetActive(false);
			this.RecipeEntry.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.ClearButton.gameObject.SetActive(true);
					this.RecipeEntry.AssignRecipe(newVal);
					this.RecipeEntry.gameObject.SetActive(true);
				}
				else
				{
					this.None.SetActive(true);
				}
			}
			else
			{
				this.Mixed.gameObject.SetActive(true);
				this.ClearButton.gameObject.SetActive(true);
			}
			this.ClearButton.gameObject.SetActive(false);
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x00142A2C File Offset: 0x00140C2C
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedRecipe != this.Fields[i + 1].SelectedRecipe)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x00142A80 File Offset: 0x00140C80
		public void Clicked()
		{
			bool flag = this.AreFieldsUniform();
			StationRecipe selectedOption = null;
			if (flag)
			{
				selectedOption = this.Fields[0].SelectedRecipe;
			}
			List<StationRecipe> options = (from x in this.Fields[0].Options
			where x.Unlocked
			select x).ToList<StationRecipe>();
			Singleton<ManagementInterface>.Instance.RecipeSelectorScreen.Initialize("Select Recipe", options, selectedOption, new Action<StationRecipe>(this.OptionSelected));
			Singleton<ManagementInterface>.Instance.RecipeSelectorScreen.Open();
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x00142B18 File Offset: 0x00140D18
		private void OptionSelected(StationRecipe option)
		{
			foreach (StationRecipeField stationRecipeField in this.Fields)
			{
				stationRecipeField.SetRecipe(option, true);
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00142B6C File Offset: 0x00140D6C
		public void ClearClicked()
		{
			foreach (StationRecipeField stationRecipeField in this.Fields)
			{
				stationRecipeField.SetRecipe(null, true);
			}
		}

		// Token: 0x0400391F RID: 14623
		[Header("References")]
		public StationRecipeEntry RecipeEntry;

		// Token: 0x04003920 RID: 14624
		public GameObject None;

		// Token: 0x04003921 RID: 14625
		public GameObject Mixed;

		// Token: 0x04003922 RID: 14626
		public GameObject ClearButton;
	}
}
