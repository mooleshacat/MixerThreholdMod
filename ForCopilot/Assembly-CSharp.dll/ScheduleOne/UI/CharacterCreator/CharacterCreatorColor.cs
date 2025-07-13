using System;
using System.Collections.Generic;
using ScheduleOne.Clothing;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B89 RID: 2953
	public class CharacterCreatorColor : CharacterCreatorField<Color>
	{
		// Token: 0x06004E5E RID: 20062 RVA: 0x0014B414 File Offset: 0x00149614
		protected override void Awake()
		{
			base.Awake();
			if (this.UseClothingColors)
			{
				this.Colors = new List<Color>();
				foreach (EClothingColor color in CharacterCreatorColor.ClothingColorsToUse)
				{
					this.Colors.Add(color.GetActualColor());
				}
			}
			for (int j = 0; j < this.Colors.Count; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer);
				gameObject.transform.Find("Color").GetComponent<Image>().color = this.Colors[j];
				Color col = this.Colors[j];
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OptionClicked(col);
				});
				this.optionButtons.Add(gameObject.GetComponent<Button>());
			}
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x0014B50C File Offset: 0x0014970C
		public override void ApplyValue()
		{
			base.ApplyValue();
			Button button = null;
			for (int i = 0; i < this.Colors.Count; i++)
			{
				if (ClothingColorExtensions.ColorEquals(base.value, this.Colors[i], 0.004f) && i < this.optionButtons.Count)
				{
					button = this.optionButtons[i];
					break;
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

		// Token: 0x06004E60 RID: 20064 RVA: 0x0014B5AD File Offset: 0x001497AD
		public void OptionClicked(Color color)
		{
			base.value = color;
			this.WriteValue(true);
		}

		// Token: 0x04003AAE RID: 15022
		public static EClothingColor[] ClothingColorsToUse = new EClothingColor[]
		{
			EClothingColor.White,
			EClothingColor.LightGrey,
			EClothingColor.DarkGrey,
			EClothingColor.Charcoal,
			EClothingColor.Black,
			EClothingColor.Red,
			EClothingColor.Crimson,
			EClothingColor.Orange,
			EClothingColor.Tan,
			EClothingColor.Brown,
			EClothingColor.Yellow,
			EClothingColor.Lime,
			EClothingColor.DarkGreen,
			EClothingColor.Cyan,
			EClothingColor.SkyBlue,
			EClothingColor.Blue,
			EClothingColor.Navy,
			EClothingColor.Purple,
			EClothingColor.Magenta,
			EClothingColor.BrightPink
		};

		// Token: 0x04003AAF RID: 15023
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x04003AB0 RID: 15024
		[Header("Settings")]
		public bool UseClothingColors;

		// Token: 0x04003AB1 RID: 15025
		public List<Color> Colors;

		// Token: 0x04003AB2 RID: 15026
		public GameObject OptionPrefab;

		// Token: 0x04003AB3 RID: 15027
		private List<Button> optionButtons = new List<Button>();

		// Token: 0x04003AB4 RID: 15028
		private Button selectedButton;
	}
}
