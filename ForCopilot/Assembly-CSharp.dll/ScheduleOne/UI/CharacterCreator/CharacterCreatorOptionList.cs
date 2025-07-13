using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B8E RID: 2958
	public class CharacterCreatorOptionList : CharacterCreatorField<string>
	{
		// Token: 0x06004E73 RID: 20083 RVA: 0x0014B778 File Offset: 0x00149978
		protected override void Awake()
		{
			base.Awake();
			if (this.CanSelectNone)
			{
				this.Options.Insert(0, new CharacterCreatorOptionList.Option
				{
					AssetPath = "",
					Label = "None"
				});
			}
			for (int i = 0; i < this.Options.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer);
				gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = this.Options[i].Label;
				string option = this.Options[i].AssetPath;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OptionClicked(option);
				});
				this.optionButtons.Add(gameObject.GetComponent<Button>());
			}
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x0014B864 File Offset: 0x00149A64
		public override void ApplyValue()
		{
			base.ApplyValue();
			Button button = null;
			int i = 0;
			while (i < this.Options.Count)
			{
				if (base.value == this.Options[i].AssetPath)
				{
					this.selectedClothingDefinition = this.Options[i].ClothingItemEquivalent;
					if (this.optionButtons.Count > i)
					{
						button = this.optionButtons[i];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = true;
			}
			this.selectedButton = button;
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = false;
			}
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x0014B91C File Offset: 0x00149B1C
		public void OptionClicked(string option)
		{
			base.value = option;
			CharacterCreatorOptionList.Option option2 = this.Options.FirstOrDefault((CharacterCreatorOptionList.Option o) => o.AssetPath == option);
			if (option2 != null)
			{
				this.selectedClothingDefinition = option2.ClothingItemEquivalent;
			}
			else
			{
				this.selectedClothingDefinition = null;
			}
			this.WriteValue(true);
		}

		// Token: 0x04003AC1 RID: 15041
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x04003AC2 RID: 15042
		[Header("Settings")]
		public bool CanSelectNone = true;

		// Token: 0x04003AC3 RID: 15043
		public List<CharacterCreatorOptionList.Option> Options;

		// Token: 0x04003AC4 RID: 15044
		public GameObject OptionPrefab;

		// Token: 0x04003AC5 RID: 15045
		private List<Button> optionButtons = new List<Button>();

		// Token: 0x04003AC6 RID: 15046
		private Button selectedButton;

		// Token: 0x02000B8F RID: 2959
		[Serializable]
		public class Option
		{
			// Token: 0x04003AC7 RID: 15047
			public string Label;

			// Token: 0x04003AC8 RID: 15048
			public string AssetPath;

			// Token: 0x04003AC9 RID: 15049
			public ClothingDefinition ClothingItemEquivalent;
		}
	}
}
