using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B45 RID: 2885
	public class RecipeSelector : ClipboardScreen
	{
		// Token: 0x06004CF0 RID: 19696 RVA: 0x0014442C File Offset: 0x0014262C
		public void Initialize(string selectionTitle, List<StationRecipe> _options, StationRecipe _selectedOption = null, Action<StationRecipe> _optionCallback = null)
		{
			this.TitleLabel.text = selectionTitle;
			this.options = new List<StationRecipe>();
			this.options.AddRange(_options);
			this.selectedOption = _selectedOption;
			this.optionCallback = _optionCallback;
			this.DeleteOptions();
			this.CreateOptions(this.options);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x00144480 File Offset: 0x00142680
		public override void Open()
		{
			base.Open();
			Debug.Log(this.Container.gameObject.name + " is active: " + this.Container.gameObject.activeSelf.ToString());
			Singleton<ManagementInterface>.Instance.MainScreen.Close();
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001444D9 File Offset: 0x001426D9
		public override void Close()
		{
			base.Close();
			Debug.Log("Closed");
			Singleton<ManagementInterface>.Instance.MainScreen.Open();
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x001444FA File Offset: 0x001426FA
		private void ButtonClicked(StationRecipe option)
		{
			if (this.optionCallback != null)
			{
				this.optionCallback(option);
			}
			this.Close();
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00144518 File Offset: 0x00142718
		private void CreateOptions(List<StationRecipe> options)
		{
			options.Sort((StationRecipe a, StationRecipe b) => a.RecipeTitle.CompareTo(b.RecipeTitle));
			for (int i = 0; i < options.Count; i++)
			{
				StationRecipeEntry component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer).GetComponent<StationRecipeEntry>();
				component.AssignRecipe(options[i]);
				if (options[i] == this.selectedOption)
				{
					component.transform.Find("Selected").gameObject.GetComponent<Image>().color = new Color32(90, 90, 90, byte.MaxValue);
				}
				StationRecipe opt = options[i];
				component.Button.onClick.AddListener(delegate()
				{
					this.ButtonClicked(opt);
				});
				this.optionButtons.Add(component.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00144614 File Offset: 0x00142814
		private void DeleteOptions()
		{
			for (int i = 0; i < this.optionButtons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.optionButtons[i].gameObject);
			}
			this.optionButtons.Clear();
		}

		// Token: 0x0400395E RID: 14686
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x0400395F RID: 14687
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003960 RID: 14688
		public GameObject OptionPrefab;

		// Token: 0x04003961 RID: 14689
		[Header("Settings")]
		public Sprite EmptyOptionSprite;

		// Token: 0x04003962 RID: 14690
		private Coroutine lerpRoutine;

		// Token: 0x04003963 RID: 14691
		private List<StationRecipe> options = new List<StationRecipe>();

		// Token: 0x04003964 RID: 14692
		private StationRecipe selectedOption;

		// Token: 0x04003965 RID: 14693
		private List<RectTransform> optionButtons = new List<RectTransform>();

		// Token: 0x04003966 RID: 14694
		private Action<StationRecipe> optionCallback;
	}
}
